using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    // Variables that control player movement
    [Header("Player Controls")]
    [SerializeField]
    public float moveSpeed; // Controls how fast the player moves

    [SerializeField]
    public float originalMoveSpeed; // Reference to the players base movespeed

    [SerializeField]
    private float jumpHeight; // Controls how high the player jumps

    [SerializeField]
    private float gravity; // Effects how fast the player falls in the air

    [SerializeField]
    public float gravitySpeed; // Used to control how strongly gravity is applied to the player

    [SerializeField]
    private float rotateSpeed; // Controls how quickly the player model turns in different directions

    [SerializeField]
    private float distToGround = 1.0001f; // How far the player is off the ground

    [SerializeField]
    private Vector3 moveDirection; // Controls the direction the player moves in

    [SerializeField]
    private bool onGround;

    [SerializeField]
    private bool sceneLoaded;



    [Header("Dash Settings")]
    [SerializeField]
    public bool canDash; // Bool to control the players ability to dash

    [SerializeField]
    public float dashDuration; // Controls the duration of a dash e.g how far it goes

    [SerializeField]
    public float elapsedTime; // Measures how much time has passed during a dash

    [SerializeField]
    private float dashCooldown; // Cooldown value between dashes

    [SerializeField]
    private float cooldownTimer;


    //Variables related to 'Coyote Time'
    [Header("Coyote Time Settings")]
    [SerializeField]
    private float coyoteTimeDuration = 0.1f; // Amount of time the player is given after leaving a platform to perform a jump

    [SerializeField]
    private float coyoteTimeCounter; // A timer to track how long the player has to perfrom their jump


    [Header("Game Objects")]
    [SerializeField]
    private GameObject playerModel; // The model the capsule uses is assigned here

    [SerializeField]
    private Transform Pivot;  // A pivot used for camera rotations

    [SerializeField]
    private Animator anim; // Used to control animaiton 

    [SerializeField]
    private GameObject UI;

    [SerializeField]
    public Image StelCrystal;


    [Header("Camera Switching")]
    [SerializeField]
    private Camera MainCamera; // Main 3D Camera

    [SerializeField]
    private Camera Camera2D; // Main 2D Camera


    [Header("Cache")]

    private CharacterController cc; // The character controller component is declared here

    private float capsuleRadius;

    private LayerMask CheckpointsLayer;


    [Header("Sound Effects")]
    [SerializeField]
    private AudioSource jumpSoundEffect; // Jumping sound effect

    [SerializeField]
    private AudioSource dashSoundEffect; // Dashing sound effect


    // Start is called before the first frame update
    void Start()
    {
        cc = gameObject.GetComponent<CharacterController>(); // Assigns the character controller component to the variable 'cc'
        capsuleRadius = gameObject.GetComponent<CapsuleCollider>().radius;
        CheckpointsLayer = LayerMask.NameToLayer("Checkpoints");
    }

    // Update is called once per frame
    void Update()
    {
        if (cc.enabled && sceneLoaded)
        {

            if (cc.isGrounded)
            {
                moveDirection.y = 0;
            }

            // Update coyote time counter
            if (isGrounded())
            {
                onGround = true;
                coyoteTimeCounter = coyoteTimeDuration;
            }
            else
            {
                onGround = false;
                coyoteTimeCounter -= Time.deltaTime;
            }

            // Check for jump including coyote time
            if ((coyoteTimeCounter > 0 || onGround) && Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpHeight;
                jumpSoundEffect.Play();
                coyoteTimeCounter = 0; // Reset coyote time counter when jump is executed
            }

            // Store the current y direction, this is to avoid it being normalized.
            float yStore = moveDirection.y;

            // Controls movement
            if (MainCamera.enabled)
            {
                // If MainCamera is enabled, movement depends on camera orientation
                moveDirection = (Pivot.forward * Input.GetAxisRaw("Vertical")) + (Pivot.right * Input.GetAxisRaw("Horizontal"));
            }
            else if (Camera2D.enabled)
            {
                // If Camera2D is enabled, movement is fixed along world axis
                moveDirection = (Vector3.forward * Input.GetAxisRaw("Horizontal")) + (-Vector3.right * Input.GetAxisRaw("Vertical"));
            }

            moveDirection = moveDirection.normalized * moveSpeed;

            // Check for jump
            moveDirection.y = yStore;
            // Allowing jump only if grounded and coyote time is over
            if (onGround && coyoteTimeCounter <= 0)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    // Uses the jumpHeight variable to tell the character where they should move to on the Y
                    moveDirection.y = jumpHeight;
                    jumpSoundEffect.Play();
                }
            }

            // Allows for variable jump heights
            if (Input.GetButtonUp("Jump") && moveDirection.y > 0f)
            {
                // Uses the jumpHeight variable to tell the character where they should move to on the Y
                moveDirection.y = moveDirection.y * .5f;
            }

            // Applies the players upwards force and modifies it depending on gravity
            moveDirection.y += gravity * gravitySpeed * Time.deltaTime;

            // Allows user to dash
            if (Input.GetButtonDown("Dash") && canDash && !UI.GetComponent<InGameMenu>().isPaused)
            {
                Dash();
            }

            // Time.deltaTime is the time since the last frame e.g 60fps = 1/60s
            cc.Move(moveDirection * Time.deltaTime);

            //If the player is moving make them face the correct direction
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                if (MainCamera.enabled)
                {
                    transform.rotation = Quaternion.Euler(0f, Pivot.rotation.eulerAngles.y, 0f);
                }
                else if (Camera2D.enabled)
                {
                    // If Camera2D is enabled, keep the player's rotation aligned with the world
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }

                // If the player is moving, calculate new rotation
                if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                {
                    // Rotation is created and applied using Quaternion.Slerp (Linear interpolation for rotation) for a smoothing effect
                    Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
                    playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
                }


            }

            //If the player is moving make them face the correct direction
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 && moveSpeed < originalMoveSpeed)
            {
                if (moveSpeed < 18)
                {
                    moveSpeed += 0.4f;
                }
            }
            else
            {
                if (moveSpeed > 5)
                {
                    moveSpeed -= 0.4f;
                }
            }


            // Animation handling
            anim.SetBool("isFalling", CheckFalling());
            anim.SetBool("isGrounded", onGround);
            anim.SetBool("isDashing", elapsedTime > 0 && elapsedTime < 0.2);
            anim.SetFloat("Speed", (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))));
        }
    }

    public void Dash()
    {
        // Get the direction the player is facing
        Vector3 playerForward = playerModel.transform.forward;
        // Get the direction the camera is facing
        Vector3 cameraForward = MainCamera.transform.forward;

        // Apply the Camera's y direction to the player direction
        playerForward.y = cameraForward.y;

        Vector3 dashDirection = (playerForward);

        StartCoroutine(Dashing(dashDirection));

    }

    IEnumerator Dashing(Vector3 dashDirection)
    {
        canDash = false;

        StelCrystal.fillAmount = 0;

        moveSpeed = 23f;

        dashSoundEffect.Play();

        StartCoroutine(DashCooldown());

        // For the duration of the dash, move the player towards the dash direction
        while (elapsedTime < dashDuration)
        {
            cc.Move(dashDirection * moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Restore the original move speed
        elapsedTime = 0f;
        moveSpeed = originalMoveSpeed;

    }

    IEnumerator DashCooldown()
    {
        cooldownTimer = 0f;

        // Waits until the user is grounded before starting the cooldown period
        StartCoroutine(DashCooldownGrounded());

        yield return new WaitForSeconds(dashCooldown);

        while (cooldownTimer < dashCooldown && cooldownTimer != 5)
        {
            // Update fill amount based on cooldown progress
            StelCrystal.fillAmount = cooldownTimer / dashCooldown;
            cooldownTimer += Time.deltaTime;
            yield return null;
        }

        // Reset fill amount to 1 and enable dash
        if (!canDash)
        {
            StelCrystal.fillAmount = 1;
            canDash = true;
        }

    }

    IEnumerator DashCooldownGrounded()
    {
        if (!onGround)
        {
            yield return new WaitUntil(() => onGround);

            // Reset fill amount to 1 and enable dash
            StelCrystal.fillAmount = 1;
            cooldownTimer = 5;
            canDash = true;

        }
    }


    bool CheckFalling()
    {
        // Player is only falling if they are moving down
        bool isFalling = cc.velocity.y < (-9.81 * Time.deltaTime);

        // Check if the player isnt grounded and have negative y velocity
        if (!isGrounded() && isFalling)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public bool isGrounded()
    {
        // Define the edge points of the capsule
        Vector3[] edges = new Vector3[8];
        Vector3 capsuleCenter = transform.position;

        // Calculate the positions of the edges
        edges[0] = capsuleCenter + Vector3.right * capsuleRadius; // North
        edges[1] = capsuleCenter - Vector3.forward * capsuleRadius; // East 
        edges[2] = capsuleCenter - Vector3.right * capsuleRadius; // West 
        edges[3] = capsuleCenter + Vector3.forward * capsuleRadius; // South 
        edges[4] = capsuleCenter + (Vector3.right - Vector3.forward) * capsuleRadius; // North East 
        edges[5] = capsuleCenter + (Vector3.right + Vector3.forward) * capsuleRadius; // North West 
        edges[6] = capsuleCenter - (Vector3.right - Vector3.forward) * capsuleRadius; // South East 
        edges[7] = capsuleCenter - (Vector3.right + Vector3.forward) * capsuleRadius; // South East 

        // Checks if any edge is grounded
        bool anyEdgeGrounded = false;
        foreach (Vector3 edge in edges)
        {
            RaycastHit hit;
            if (Physics.Raycast(edge, Vector3.down, out hit, distToGround))
            {

                // Check if the hit object is not on the "Checkpoint" layer
                if (hit.collider.gameObject.layer != CheckpointsLayer)
                {
                    // Visualize the raycast
                    Debug.DrawRay(edge, Vector3.down * distToGround, Color.green);

                    anyEdgeGrounded = true;
                }
            }
            else
            {
                // Visualize the raycast
                Debug.DrawRay(edge, Vector3.down * distToGround, Color.red);
            }
        }

        return anyEdgeGrounded;
    }

}