using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class convayerScript : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private HealthControl healthControl;

    [SerializeField]
    private Vector3 forwardDirection;

    [SerializeField]
    private bool forward;

    [SerializeField]
    private int speed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        healthControl = GameObject.Find("gameManager").GetComponent<HealthControl>();
    }

    private void Update()
    {
        if (forward)
        {
            player.movePlayer(-forwardDirection, speed); // Pass the forward direction to the movePlayer function 
        }

        if (healthControl.Health == 0)
        {
            forward = false;
        }
    }

    // When a collider triggers collision with this object
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") // If the collider is the player
        {
            // Get the forward direction of the object this script is attached to
            forwardDirection = transform.forward;
            forward = true;
           
        }
    }

    // When the collider stops colliding with this object
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") // If the collider is the player
        {
            forward = false; // Stop moving the player
        }
    }
}
