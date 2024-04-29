using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [Header("Checkpoint Control")]
    [SerializeField]
    private Vector3 spawnLocation; // Position of checkpoint location

    [SerializeField]
    private Renderer flagRender; // Relates to the checkpoints renderer

    [SerializeField]
    private Material flagOn; // flagOn Material (Green)

    [SerializeField]
    private Material flagOff; // flagOff Material (Red)


    [Header("Game Objects")]
    [SerializeField]
    private HealthControl healthControl; // Reference to the health control script

    // Start is called before the first frame update
    void Start()
    {
        Invoke("findHealthControl", 0.2f);
    }

    private void findHealthControl()
    {
        healthControl = FindAnyObjectByType<HealthControl>(); // Finds the health controller
    }

    private void OnTriggerEnter(Collider other) // If anything collides with the checkpoint runs
    {
        // Checks to make sure its the player interacting with the checkpoint
        if(other.tag.Equals("Player"))
        {
            spawnLocation = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z); // sets new spawn location equal to that of the checkpoint position
            healthControl.setCheckpoint(spawnLocation); // Passes the spawn location to the setCheckpoint in the healthControl script
            checkpointDetect(); // Handles the color of the checkpoints flag
            
        }
    }


    private void checkpointDetect()
    {
        CheckpointController[] checkpoints = FindObjectsOfType<CheckpointController>(); // Finds all the checkpoints in the game

        // For every checpoint, disable them, setting their flag to red.
        foreach(CheckpointController StoredCheckpoint in checkpoints)
        {
            StoredCheckpoint.checkpointDisable();
        }

        // For the current checkpoint, change the flag to green instead.
        for (int i = 0; i < flagRender.materials.Length; i++)
        {
            if (flagRender.materials[i].name.Contains("flagOff"))
            {
                flagRender.materials[i].color = flagOn.color;
                flagRender.materials[i].name = flagOn.name;
            }
        }
    }

    // Disables the checkpoint by setting the material values to those of flagOff
    private void checkpointDisable()
    {
        for (int i = 0; i < flagRender.materials.Length; i++)
        {
            // Disables currently enabled checkpoint
            if (flagRender.materials[i].name.Contains("flagOn"))
            {
                flagRender.materials[i].color = flagOff.color;
                flagRender.materials[i].name = flagOff.name;
            }
        }
    }

}
