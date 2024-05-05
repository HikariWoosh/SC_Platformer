using UnityEngine;


public class allow2D : MonoBehaviour
{
    [Header("Script References")]

    [SerializeField]
    private CameraControl control; // Reference to the CameraControl script

    [SerializeField]
    private PlayerController player; // Reference to the PlayerController script

    [SerializeField]
    private CameraControlSecondDimension control2; // Reference to the Camera2d script

    [SerializeField]
    private HealthControl healthControl; // Reference to the HealthControl script

    [SerializeField]
    private buttomPrompter buttomPrompt; // Reference to the buttonPrompt script


    [Header("2d Controls")]

    [SerializeField]
    private Vector3 zoneOffset; // Offset variable unique to each Fracture Zone

    [SerializeField]
    private Quaternion zoneRotation; // Rotation variable unique to each Fracture Zone

    [SerializeField]
    private bool allow2DMovement; // Bool to decide how the player moves depending on rotation



    private void Start()
    {
        findCamera();
    }

    private void Update()
    {
        // Forces the player out of 2d view if they die
        if (healthControl.Health < 1)
        {
            control.canSwitch = false;
            control.triggerLeave();
        }
    }


    // Caches game objects
    private void findCamera()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        buttomPrompt = FindAnyObjectByType<buttomPrompter>();
        control = GameObject.Find("Main Camera").GetComponent<CameraControl>();
        control2 = GameObject.Find("2d Camera").GetComponent<CameraControlSecondDimension>();
        healthControl = GameObject.Find("gameManager").GetComponent<HealthControl>();
    }


    // Detects when the player enters
    private void OnTriggerEnter(Collider other)
    {
        // If the collider is the player
        if (other.gameObject.tag == "Player")
        {
            // Shows the button prompt
            buttomPrompt.hidePrompts();
            buttomPrompt.showG();

            // Alters the 2d cameras offset and allows the player to change camera
            control2.changeOffset(zoneOffset, zoneRotation);
            control.canSwitch = true;
            player.Movement2D = allow2DMovement;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the collider is the player
        if (other.gameObject.tag == "Player") 
        {
            // Hides the button prompt and switchs the player to 3D
            buttomPrompt.hidePrompts();
            control.canSwitch = false;
            control.triggerLeave();
        }
    }
}
