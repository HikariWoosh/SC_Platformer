using UnityEngine;

public class leverScript : MonoBehaviour
{
    [Header("Lever Controls")]


    [SerializeField]
    private bool isActive; // Bool to control if the lever be pulled

    [SerializeField]
    private float pullCooldown; // Cooldown on the interaction

    [SerializeField]
    private float lastPullTime; // Time since last interaction

    [SerializeField]
    private bool Pulled; // Bool to check if the lever is pulled

    [SerializeField]
    private bool leverUsed; // Bool to check if the lever has already been used


    [Header("Game Objects")]


    [SerializeField]
    private GameObject Lever; // Lever object

    [SerializeField]
    private GameObject TwoDLever; // 2d lever object

    [SerializeField]
    private Camera MainCamera; // Main camera

    [SerializeField]
    private SphereCollider Collision; // Interaction range for lever

    [SerializeField]
    private buttomPrompter buttomPrompt; // Reference to button prompt script

    [SerializeField]
    private GameObject Open; // Open state of the door

    [SerializeField]
    private GameObject Close; // Close state of the door

    [SerializeField]
    private AudioSource leverSound; // Sound used when lever is pulled

    [SerializeField]
    private Animator anim; // Used to control animaiton 

    private void Start()
    {
        Invoke("findCameraPlayer", 0.2f);
    }

    private void findCameraPlayer()
    {
        // Caches components
        MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        buttomPrompt = FindAnyObjectByType<buttomPrompter>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (MainCamera != null)
        {
            // Checks which state the camera is in
            isActive = MainCamera.enabled;

            if (!isActive)
            {
                // Allows the lever to be pulled
                Lever.SetActive(true);
                Collision.enabled = true;
                TwoDLever.SetActive(false);
            }

            else
            {
                // Deactivates the lever
                Lever.SetActive(false);
                Collision.enabled = false;
                TwoDLever.SetActive(true);
            }

            // Checks if the lever is able to be pulled
            if (Input.GetKeyDown(KeyCode.E) && leverUsed != true && Time.time >= lastPullTime + pullCooldown && !isActive)
            {
                lastPullTime = Time.time;
                Pulled = true;
            }
        }

    }


    // Displays a button prompt upon entering interaction range
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            buttomPrompt.hidePrompts();
            buttomPrompt.showE();
        }
    }

    // If the player is in range and pulls the lever
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player") && !isActive && Pulled)
        {
            // Plays the animation and sound linked to the action
            anim.SetTrigger("Pulled");
            leverSound.Play();
            
            // Alters the 2d version in relation to the 3d one
            TwoDLever.transform.rotation *= Quaternion.Euler(0, 180, 0);

            // Changes the door state and makes the lever used
            Close.SetActive(false);
            Pulled = false;
            leverUsed = true;
        }
    }

    // Hides button prompts on leaving the interaction range
    private void OnTriggerExit(Collider other)
    {
        buttomPrompt.hidePrompts();
    }

}