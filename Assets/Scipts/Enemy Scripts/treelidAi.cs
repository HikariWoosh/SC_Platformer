using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class treelidAI : MonoBehaviour
{
    [Header("Enemy Info")]

    [SerializeField]
    private Transform treelid; // Reference to the enemy

    [SerializeField]
    private Transform target; // Reference to the player

    [SerializeField]
    private Vector3 startPos; // The starting position of the enemy

    [SerializeField]
    private Vector3 movePosition; // The position the enemy wants to move to

    [SerializeField]
    private Animator animator; // The animator used by the enemy

    [SerializeField]
    private GameObject deathParticles; // Particles that are emitted upon death

    [SerializeField]
    private GameObject autumnJewel; // Jewel linked to the enemy

    [SerializeField]
    private float attackCounter; // Counter until the enemy attacks

    [SerializeField]
    private HealthControl healthControl; // Reference to the HealthConrol script

    [SerializeField]
    private AudioSource deathSound; // Audio that is played upon enemy death

    [SerializeField]
    private AudioSource alertSound; // Audio that is played upon enemy death

    [SerializeField]
    private GameObject detected; // ! mark linked to enemy


    [Header("Enemy Controls")]

    [SerializeField]
    private float aggroRange; // The aggro range of the enemy

    [SerializeField]
    private float returnTime; // The amount of time it takes for the enemy to return to start position

    [SerializeField]
    private float speed; // The speed at which the enemy moves

    [SerializeField]
    private float distance; // The distance the player is from the start position

    [SerializeField]
    private float distanceFromPlayer; // The distance from the player

    [SerializeField]
    private Vector3 direction; // Direction the enemy wants to go towards

    [SerializeField]
    private Quaternion targetRotation; // Rotation the enemy wants to face

    [SerializeField]
    private float rotationProgress; // Rotation of the enemy

    [SerializeField]
    private float rotationSpeed = 0.1f; // Rotation speed of enemy


    [Header("Waypoints")]

    [SerializeField]
    private bool roaming; // Bool to check if the enemy should patrol

    [SerializeField]
    private List<GameObject> waypoints = new List<GameObject>();  // List of waypoints the enemy visits

    [SerializeField]
    private int currentWaypointIndex = 0; // Index used to control which waypoint is visited


    // Start is called before the first frame update
    void Start()
    {
        // Caches game objects
        target = GameObject.Find("Player").transform;
        healthControl = GameObject.Find("gameManager").GetComponent<HealthControl>();
        startPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        // Gets the players distance from the enemy and start position
        distance = Vector3.Distance(startPos, target.position);
        distanceFromPlayer = Vector3.Distance(transform.position, target.position);

        if (transform.position.y >= 3)
        {
            animator.SetBool("running", false);
        }

        // Checks if the player is in attack range
        if (distanceFromPlayer <= 10f)
        {
            attackCounter = attackCounter + 0.1f;
        }
        else
        {
            attackCounter = 0;
        }

        // Ensures the enemy cannot act when airborn 
        if (transform.position.y < 3) 
        {

            // Aggros the player if they are within range (and alive)
            if (distance <= aggroRange && healthControl.Health > 0)
            {
                chase();
                
                // Attacks the player if in range for too long
                if(attackCounter > 5)
                {
                    attack();
                }
            }

            // deaggros the player when out of range
            if (distance > aggroRange && !roaming)
            {
                deAggro();
                transform.position = Vector3.Lerp(transform.position, startPos, returnTime * Time.deltaTime);
            }

            // Run when the enemy is roaming and the player is out of range
            if (roaming)
            {
                animator.SetBool("running", true);

                // Stores the location of the waypoint to be visited
                Vector3 currentWaypointPosition = waypoints[currentWaypointIndex].transform.position;

                // Increments waypoint index when reached
                if (Vector3.Distance(transform.position, currentWaypointPosition) <= 0.1f)
                {
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
                }


                transform.LookAt(currentWaypointPosition);
                transform.position = Vector3.MoveTowards(transform.position, currentWaypointPosition, speed * Time.deltaTime);
            }
        }

    }

    // Follows the player around
    void chase()
    {
        if (roaming)
        {
            StartCoroutine(detect());
            alertSound.Play();
        }
        roaming = false;
        animator.SetBool("running", true);
        movePosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(movePosition);
        transform.position = Vector3.MoveTowards(treelid.position, movePosition, speed * Time.deltaTime);
    }

    // Displays a "!" over the enemy
    IEnumerator detect()
    {
        detected.SetActive(true);
        yield return new WaitForSeconds(1f);
        detected.SetActive(false);
    }

    // Function used when the player is too far from the island
    void deAggro()
    {
        // Returns to starting position when close enough
        if (Vector3.Distance(transform.position, startPos) > 5f)
        {
            StartCoroutine(smoothRotate(startPos));
        }
        else
        {
            transform.position = startPos;
            roaming = true;
            animator.SetBool("running", false);
        }
    }

    IEnumerator smoothRotate(Vector3 lookAt)
    {
        // Reset rotation progress
        rotationProgress = 0f;

        // Calculate the rotation
        direction = (lookAt - transform.position).normalized;
        targetRotation = Quaternion.LookRotation(direction);


        while (rotationProgress < 0.1)
        {
            // How much the enemy has rotated through its cycle
            rotationProgress += Time.deltaTime * rotationSpeed;

            // Slerp the enemy rotation based on the rotation progress
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationProgress);

            yield return null;

        }
    }


    // Function run when the player is too close to the enemy
    void attack()
    {
        StartCoroutine(attackScript(1f));
    }

    // Coroutine that controls the attack state of the enemy
    IEnumerator attackScript(float damage)
    {
        // Deals damage to the player and plays the attack animation
        animator.SetBool("attack", true);
        healthControl.damagePlayer(1);
        yield return new WaitForSeconds(damage);
        animator.SetBool("attack", false);

    }

    // Function used to detect when water is collided with
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("water"))
        {
            // Outputs death effect & sound and places the jewel on the enemies position
            GameObject Particles = Instantiate(deathParticles, transform.position, transform.rotation);
            Destroy(Particles, 1f);
            autumnJewel.transform.position = transform.position;
            deathSound.Play();
            Destroy(this.gameObject);
        }
    }

}
