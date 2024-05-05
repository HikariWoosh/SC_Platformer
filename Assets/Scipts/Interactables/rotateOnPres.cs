using System.Collections;
using UnityEngine;

public class rotateOnPres : MonoBehaviour
{

    [Header("Statue Control")]

    [SerializeField]
    private bool isActive; // Can statue be rotated

    [SerializeField]
    private float pullCooldown; // Cooldown on rotation

    [SerializeField]
    private int rotationIndex; // What position is the statue in

    [SerializeField]
    private int correctIndex; // Correct index for the statue

    [SerializeField]
    private float rotationProgress; // Rotation of the statue

    [SerializeField]
    private float rotationSpeed = 0.1f; // Rotation speed of statue

    [SerializeField]
    public bool inPlace; // Bool to determine if the statue is in the correct position

    [SerializeField]
    private float lastPullTime; // Time since last rotate

    [SerializeField]
    private Quaternion targetRotation; // Desired position of statue

    [SerializeField]
    private bool Pulled; // Bool to determine when the statue is rotating

    [SerializeField]
    private AudioSource turnSound; // Sound played on turn


    [Header("Game Objects")]

    [SerializeField]
    private GameObject foxStatue; // Statue 

    [SerializeField]
    private buttomPrompter buttomPrompt; // Reference to buttomPrompter

    [SerializeField]
    private statuePuzzleScript statuePuzzleScript; // Reference to statuePuzzleScript


    // Start is called before the first frame update
    private void Start()
    {
        Invoke("findCameraPlayer", 0.2f);
        
    }

    private void findCameraPlayer()
    {
        // Caches component
        statuePuzzleScript = FindAnyObjectByType<statuePuzzleScript>();
        buttomPrompt = FindAnyObjectByType<buttomPrompter>();

        statueStart();
    }

    private void Update()
    {
        // If E is pressed, the pull cooldown isnt active, the lever is in range, and the puzzle hasnt been solved, allow for the lever to be pulled
        if (Input.GetKeyDown(KeyCode.E)  && Time.time >= lastPullTime + pullCooldown && isActive && !statuePuzzleScript.allInPlace)
        {
            lastPullTime = Time.time;
            Pulled = true;
        }
    }


    // When player enters the hitbox of statue
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            // Hide all other prompts and display E
            buttomPrompt.hidePrompts();
            if (!statuePuzzleScript.allInPlace)
            {
                buttomPrompt.showE();
            }
            
            // Allow for lever to be pulled
            isActive = true;
        }
    }

    // When the player is within range
    private void OnTriggerStay(Collider other)
    {
        // If player pulled the lever and in range, 
        if (other.tag.Equals("Player") && isActive && Pulled)
        {
            // Rotate the statue
            StartCoroutine(foxRotate());
            Pulled = false;
        }
    }

    // When the player leaves the range, disable button press and hide prompt
    private void OnTriggerExit(Collider other)
    {
        isActive = false;
        buttomPrompt.hidePrompts();
    }

    // Checks if the statues are in the correct position and check if puzzle completed
    private void statueStart()
    {
        if (rotationIndex == correctIndex)
        {
            inPlace = true;
            statuePuzzleScript.puzzleSolved();
        }
        else
        {
            inPlace = false;
        }

    }

    // Coroutine to rotate the fox over a period of time
    private IEnumerator foxRotate()
    {
        // Reset rotation progress
        rotationProgress = 0f;

        // Set the target rotation
        targetRotation = foxStatue.transform.rotation * Quaternion.Euler(0, 90, 0);

        turnSound.Play();

        // While rotation progress is less than 1, interpolate the rotation
        while (rotationProgress < 0.1)
        {
            // How much the statue has rotated through its cycle
            rotationProgress += Time.deltaTime * rotationSpeed;

            // Slerp the statues rotation based on the rotation progress
            foxStatue.transform.rotation = Quaternion.Slerp(foxStatue.transform.rotation, targetRotation, rotationProgress);

            yield return null;
          
        }

        // Update the rotation index and check if the statue is in the correct position
        if (rotationIndex == 4)
        {
            rotationIndex = 1;
        }
        else
        {
            rotationIndex += 1;
        }

        statueStart();
    }

}
