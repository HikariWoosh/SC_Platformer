using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraControl : MonoBehaviour
{

    [Header("Game Objects")]
    [SerializeField]
    private Transform target; // The target the camera will follow

    [SerializeField]
    private Transform verticalPivot; // The vertical camera pivot ('Pivot')

    [SerializeField]
    private Transform horizontalPivot; // The horizontal camera pivot ('HPivot')

    [SerializeField]
    private Transform followPoint; // The transform of the object the camera follows

    [SerializeField]
    private HealthControl healthControl; // Reference to health control script


    [Header("Camera Controls")]
    [SerializeField]
    private Vector3 offset; // Offset between the player and the camera

    [SerializeField]
    private float rotateSpeed; // How fast the camera rotates around the player

    [SerializeField]
    private float maxView; // The maximum a player can look upwards

    [SerializeField]
    private float minView; // The maximum a player can look downwards

    [SerializeField]
    private bool invertY; // Control setting


    [Header("Camera Switching")]
    [SerializeField]
    public Camera MainCamera; // Main 3d Camera Field

    [SerializeField]
    private Camera Camera2D; // Field for the 2d camera

    [SerializeField]
    public GameObject Camera2DView; // Reference to the image used to render the 2d view

    [SerializeField]
    public bool canSwitch; // Bool used to control if the user can change camera state

    [SerializeField]
    private buttomPrompter prompt; // Reference to the script used to control button prompts


    [Header("Layer Masks")]
    private int playerLayerMask;

    private int checkPointLayerMask;

    private int cameraIgnoreLayerMask;

    private int ignoreLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        // Sets the offset position
        offset = new Vector3(0, -5, 20);

        // Sets the pivot locations
        verticalPivot.transform.position = target.transform.position;
        horizontalPivot.transform.position = target.transform.position;

        // Sets the horizontal pivot as the parent of the vertical pivot
        verticalPivot.transform.parent = horizontalPivot.transform;

        healthControl = FindAnyObjectByType<HealthControl>();
        prompt = FindAnyObjectByType<buttomPrompter>();

        SceneManager.sceneLoaded += OnSceneLoaded;

        // Group layer masks
        playerLayerMask = 1 << target.gameObject.layer;
        checkPointLayerMask = 1 << LayerMask.NameToLayer("Checkpoints");
        cameraIgnoreLayerMask = 1 << LayerMask.NameToLayer("CameraIgnore");
        int playerAndCheckpointLayerMask = playerLayerMask | checkPointLayerMask | cameraIgnoreLayerMask;
        ignoreLayerMask = ~playerAndCheckpointLayerMask;

    }


    // Called once per frame after Update()
    void LateUpdate()
    {
        // Button to switch camera view
        if (Input.GetKeyDown(KeyCode.G) && SceneManager.GetActiveScene().name != "Main Menu" && canSwitch)
        {
            Show2DView();
        }

        // 3D camera control
        if (MainCamera.enabled == true)
        {
            // Sets the pivot to the same position as the player
            verticalPivot.transform.position = target.transform.position;

            // Controls the pivots rotations depending on how the player moves their mouse
            float horizontalCamera = Input.GetAxis("Mouse X") * rotateSpeed;
            horizontalPivot.Rotate(0, horizontalCamera, 0);

            float verticalCamera = Input.GetAxis("Mouse Y") * rotateSpeed;
            // Controls the Y invert (e.g if mouse goes up, camera goes down)
            if (invertY)
            {
                verticalPivot.Rotate(-verticalCamera, 0, 0);
            }
            else
            {
                verticalPivot.Rotate(verticalCamera, 0, 0);
            }


            // Controls how far up the player can look
            if (verticalPivot.rotation.eulerAngles.x > maxView && verticalPivot.rotation.eulerAngles.x < 180.0f)
            {
                verticalPivot.rotation = Quaternion.Euler(maxView, verticalPivot.eulerAngles.y, 0.0f);
            }


            // Controls how low down the player can look
            if (verticalPivot.rotation.eulerAngles.x > 180.0f && verticalPivot.rotation.eulerAngles.x < 360f + minView)
            {
                verticalPivot.rotation = Quaternion.Euler(360.0f + minView, verticalPivot.eulerAngles.y, 0.0f);
            }

            // Takes in the rotation of the pivots (vPivot inherits hPivots x)
            float yAngle = verticalPivot.eulerAngles.y;
            float xAngle = verticalPivot.eulerAngles.x;


            // Applies the pivot values
            Quaternion rotationValue = Quaternion.Euler(xAngle, yAngle, 0);
            transform.position = target.position - (rotationValue * offset);


            // Calculate desired camera position
            Vector3 desiredPosition = target.position - (rotationValue * offset);


            // Perform raycast to check for collision
            RaycastHit hit;

            if (Physics.Linecast(followPoint.position, desiredPosition, out hit, ignoreLayerMask))
            {
                Vector3 collisionNormalOffset = hit.normal * 0.1f;

                // If a collision occurs with anything except the player, set camera position to hit point
                transform.position = hit.point + collisionNormalOffset;
            }
            else
            {
                // If no collision, set camera position as desired position
                transform.position = desiredPosition;
            }

            // Makes the camera look at the player
            transform.LookAt(followPoint);
        }
    }

    // Used by other scripts to alter the cameras state
    public void triggerLeave()
    {
        if (!MainCamera.enabled && !healthControl.Transition && healthControl.Health > 0)
        {
            StartCoroutine(Show2DViewCoroutine());
        }

    }


    // Changes the rotation speed of the camera
    public void SetRotateSpeed(float speed)
    {
        rotateSpeed = speed;
    }

    // Function to switch the camera to a 2d view
    public void Show2DView()
    {
        if (!healthControl.Transition && healthControl.Health > 0)
        {
            StartCoroutine(Show2DViewCoroutine());
        }
    }

    // Main function used to switch the camera between 2d and 3d 
    private IEnumerator Show2DViewCoroutine()
    {
        yield return StartCoroutine(healthControl.Fade());

        // If already in 2D then switch back to 3D and vice versa
        if (MainCamera.enabled == true)
        {
            transform.LookAt(target);
            MainCamera.enabled = false;

            prompt.hidePrompts();

            Camera2D.enabled = true;
            Camera2DView.SetActive(true);
 
        }
        else
        {
            prompt.hidePrompts();

            MainCamera.enabled = true;
            Camera2D.enabled = false;
            Camera2DView.SetActive(false);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main Menu")
        {
            MainCamera.GetComponent<AudioListener>().enabled = false;
        }
        else
        {
            MainCamera.GetComponent<AudioListener>().enabled = true;
        }

    }
}