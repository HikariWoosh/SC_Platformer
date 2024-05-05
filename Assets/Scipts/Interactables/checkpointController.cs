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
        // Caches heatlh controller
        healthControl = FindAnyObjectByType<HealthControl>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        // Checks to make sure its the player interacting with the checkpoint
        if(other.tag.Equals("Player"))
        {
            // sets new spawn location equal to that of the checkpoint position
            spawnLocation = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

             // Passes the spawn location to the setCheckpoint in the healthControl script
            healthControl.setCheckpoint(spawnLocation);
            checkpointDetect();
            
        }
    }


    private void checkpointDetect()
    {
        // Finds all the checkpoints in the game
        CheckpointController[] checkpoints = FindObjectsOfType<CheckpointController>();

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
