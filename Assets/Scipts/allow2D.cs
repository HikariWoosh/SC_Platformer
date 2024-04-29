using UnityEngine;


public class allow2D : MonoBehaviour
{
    [SerializeField]
    private CameraControl control;

    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private CameraControlSecondDimension control2;

    [SerializeField]
    private HealthControl healthControl;

    [SerializeField]
    private buttomPrompter buttomPrompt;

    [SerializeField]
    private Vector3 zoneOffset;

    [SerializeField]
    private Quaternion zoneRotation;

    [SerializeField]
    private bool allow2DMovement;



    private void Start()
    {
        findCamera();
    }

    private void Update()
    {
        if (healthControl.Health < 1)
        {
            control.canSwitch = false;
            control.triggerLeave();
        }
    }

    private void findCamera()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        buttomPrompt = FindAnyObjectByType<buttomPrompter>();
        control = GameObject.Find("Main Camera").GetComponent<CameraControl>();
        control2 = GameObject.Find("2d Camera").GetComponent<CameraControlSecondDimension>();
        healthControl = GameObject.Find("gameManager").GetComponent<HealthControl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") // If the collider is the player
        {
            buttomPrompt.hidePrompts();
            buttomPrompt.showG();

            control2.changeOffset(zoneOffset, zoneRotation);
            control.canSwitch = true;
            player.Movement2D = allow2DMovement;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") // If the collider is the player
        {
            buttomPrompt.hidePrompts();
            control.canSwitch = false;
            control.triggerLeave();
        }
    }
}
