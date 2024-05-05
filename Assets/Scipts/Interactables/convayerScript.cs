using UnityEngine;

public class convayerScript : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField]
    private PlayerController player; // Reference to player controller script

    [SerializeField]
    private HealthControl healthControl; // Reference to the health controller script


    [Header("Conveyer Controls")]
    [SerializeField]
    private Vector3 forwardDirection; // Forward direction to move the player in

    [SerializeField]
    private bool forward; // If the player should be moved

    [SerializeField]
    private int speed; // Speed at which the player willbe moved


    // Start is called before the first frame update
    void Start()
    {
        // Caches components
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        healthControl = GameObject.Find("gameManager").GetComponent<HealthControl>();
    }

    private void Update()
    {
        if (forward)
        {
            // Pass the forward direction to the movePlayer function 
            player.movePlayer(-forwardDirection, speed);
        }

        if (healthControl.Health == 0)
        {
            // If the player dies, disable the forward movemment
            forward = false;
        }
    }

    // When a collider triggers collision with this object
    private void OnTriggerEnter(Collider other)
    {
        // If the collider is the player
        if (other.gameObject.tag == "Player")
        {
            // Get the forward direction of the object this script is attached to
            forwardDirection = transform.forward;
            forward = true;
           
        }
    }

    // When the collider stops colliding with this object
    private void OnTriggerExit(Collider other)
    {
        // If the collider is the player
        if (other.gameObject.tag == "Player")
        {
            // Stop moving the player
            forward = false; 
        }
    }
}
