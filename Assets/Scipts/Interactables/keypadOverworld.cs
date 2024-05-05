using UnityEngine;


public class keypadOverworld : MonoBehaviour
{

    [Header("Lever Controls")]


    [SerializeField]
    private float pullCooldown; // Cooldown to access keypad

    [SerializeField]
    private float lastPullTime; // Time since last interaction

    [SerializeField]
    private bool isActive; // Is the keypad able to access

    [SerializeField]
    private bool Pulled; // Has the player attempted to interact

    [SerializeField]
    private bool keypadUsed; // Has the keypad been used

    [SerializeField]
    private bool canPull; // Can the player interact


    [Header("Game Objects")]

    [SerializeField]
    private GameObject Keypad; // Reference to keypad object

    [SerializeField]
    private Canvas keyCanvas; // The canvas the keypad is in

    [SerializeField]
    private GameObject MainCamera; // Reference to players camera

    [SerializeField]
    private InGameMenu ingameMenu; // Reference to the ingamemenu script

    [SerializeField]
    private SphereCollider Collision; // Interaction range of the keypad

    [SerializeField]
    private buttomPrompter prompt; // Reference to the button prompt script


    private void Start()
    {
        Invoke("findPlayer", 0.2f);
    }   

    private void findPlayer()
    {
        // Caches components
        MainCamera = GameObject.Find("Main Camera");
        prompt = GameObject.Find("ButtonPromptUI").GetComponent<buttomPrompter>();
        ingameMenu = FindAnyObjectByType<InGameMenu>();
    }

    void LateUpdate()
    {
        // Checks if the player is trying to, and is able to interact with the keypad
        if (Input.GetKeyDown(KeyCode.E) && keypadUsed != true && Time.time >= lastPullTime + pullCooldown && canPull)
        {
            lastPullTime = Time.time;
            Pulled = true;
        }

        // Changes the sorting order so the keypad is only ontop of the UI when the game is unpaused.
        if (isActive && !ingameMenu.isPaused) 
        {
            keyCanvas.sortingOrder = 6;
            if (Cursor.visible == false)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else
        {
            keyCanvas.sortingOrder = 1;
        }
    }

    public void closeKeypad()
    {
        // Makes the cursor invisable and resets camera rotation
        Cursor.visible = false;
        MainCamera.GetComponent<CameraControl>().SetRotateSpeed(1.5f);
        Cursor.lockState = CursorLockMode.Locked;

        // Disables the keypad
        Pulled = false;
        isActive = false;
        Keypad.SetActive(false);
    }

    // Function to open the keypad
    private void openKeypad()
    {
        // Makes the cursor visable and locks the camera in place
        Cursor.visible = true;
        MainCamera.GetComponent<CameraControl>().SetRotateSpeed(0);
        Cursor.lockState = CursorLockMode.None;

        // Enables the keypad
        Pulled = false;
        isActive = true;
        Keypad.SetActive(true);

    }

    private void OnTriggerEnter(Collider other)
    {
        // If the collider is the player
        if (other.gameObject.tag == "Player")
        {
            // Allows the player to open the keypad and gives them a visual prompt
            canPull = true;
            prompt.showE();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // IF the player is within interaction range, depenidng on if the interaction is true, open/close the keypad
        if (other.tag.Equals("Player") && isActive && Pulled)
        {
            closeKeypad();
        }

        if (other.tag.Equals("Player") && !isActive && Pulled)
        {
            openKeypad();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the collider is the player
        if (other.gameObject.tag == "Player") 
        {
            // Disables the players ability to pull the lever and hides prompt/keypad
            canPull = false;
            prompt.hidePrompts();
            closeKeypad();
        }
    }
}
