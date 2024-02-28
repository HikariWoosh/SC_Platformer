using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Variables that control player movement
    [Header("Player Controls")]
    [SerializeField]
    private float moveSpeed; // Controls how fast the player moves

    [SerializeField]
    private float jumpHeight; // Controls how high the player jumps

    [SerializeField]
    private float gravity; // Effects how fast the player falls in the air

    [SerializeField]
    private float gravitySpeed; // Used to control how strongly gravity is applied to the player

    [SerializeField]
    private float rotateSpeed; // Controls how quickly the player model turns in different directions

    [SerializeField]
    private float distToGround = 1.0001f; // How far the player is off the ground

    private Vector3 moveDirection; // Controls the direction the player moves in


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


    [Header("Camera Switching")]
    [SerializeField]
    private Camera MainCamera;

    [SerializeField]
    private Camera Camera2D;

    private CharacterController cc; // The character controller component is declared here


    // Start is called before the first frame update
    void Start()
    {
        cc = gameObject.GetComponent<CharacterController>(); // Assigns the character controller component to the variable 'cc'
    }

    // Update is called once per frame
    void Update()
    {
        // Update coyote time counter
        if (isGrounded())
        {
            coyoteTimeCounter = coyoteTimeDuration;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Check for jump including coyote time
        if ((coyoteTimeCounter > 0 || isGrounded()) && Input.GetButtonDown("Jump"))
        {
            moveDirection.y = jumpHeight;
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
        if (isGrounded() && coyoteTimeCounter <= 0)
        {
            moveDirection.y = 0f;
            if (Input.GetButtonDown("Jump"))
            {
                // Uses the jumpHeight variable to tell the character where they should move to on the Y
                moveDirection.y = jumpHeight;
            }
        }

        // Allows for variable jump heights
        if (Input.GetButtonUp("Jump") && moveDirection.y > 0f)
        {
            // Uses the jumpHeight variable to tell the character where they should move to on the Y
            moveDirection.y = moveDirection.y * .5f;
        }

        // Applies the players upwards force and modifies it depending on gravity
        moveDirection.y = moveDirection.y + (gravity * gravitySpeed * Time.deltaTime);

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
            if (moveDirection != Vector3.zero)
            {
                // Rotation is created and applied using Quaternion.Slerp (Linear interpolation for rotation) for a smoothing effect
                Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
            }
        }


          // Animation handling
        anim.SetBool("isFalling", CheckFalling());
        anim.SetBool("isGrounded", isGrounded());
        anim.SetFloat("Speed", (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))));
    }

    bool CheckFalling()
    {
        // Check if the player is grounded before considering them falling
        if (!isGrounded())
        {
            // Player is only falling if they are moving down
            bool isFalling = cc.velocity.y < (-9.81 * Time.deltaTime);

            // Returns the value of isFalling
            if (isFalling)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    bool isGrounded()
    {
        // Define the edge points of the capsule
        Vector3[] edges = new Vector3[8];
        Vector3 capsuleCenter = transform.position;
        float capsuleRadius = GetComponent<CapsuleCollider>().radius;

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
            if (Physics.Raycast(edge, Vector3.down, distToGround))
            {
                // Visualize the raycast
                Debug.DrawRay(edge, Vector3.down * distToGround, Color.green);
                anyEdgeGrounded = true;
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
