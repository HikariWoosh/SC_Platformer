using UnityEngine;

public class turretScript : MonoBehaviour
{
    [Header("Game Objects")]

    [SerializeField]
    private Transform player; // Players position

    [SerializeField]
    private Transform turret; // Turrets position

    [SerializeField]
    private HealthControl healthControl; // Reference to the Health Control script

    [SerializeField]
    private Vector3 ogPosition; // The deafult position of the turret

    [SerializeField]
    private Vector3 targetPosition; // The position the turret wishes to move to

    [SerializeField]
    private Transform barrel; // Position of the turrets barrel 

    [SerializeField]
    private GameObject bullet; // Reference to the bullet


    [Header("Turret Controls")]

    [SerializeField]
    private bool fire; // Checks if the enemy can shoot

    [SerializeField]
    private float projectileSpeed; // Speed of the bullet

    [SerializeField]
    private float moveSpeed; // Speed of the turret

    [SerializeField]
    private float fireRate; // How often the turret can fire

    [SerializeField]
    private float nextShot; // Counter towards the enemies next shot

    [SerializeField]
    private AudioSource soundEffect; // Sound effect that is played by the turret

    // Start is called before the first frame update
    void Start()
    {
        // Caches important game objects and values
        player = GameObject.Find("Player").transform;
        healthControl = GameObject.Find("gameManager").GetComponent<HealthControl>();

        ogPosition = turret.transform.position;
        targetPosition = ogPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // If the player dies reset to deafult position
        if (healthControl.Health == 0)
        {
            Reset();
        }

        if (fire) 
        {
            // Makes the enemy follow the player and look at them
            targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);
            turret.position = Vector3.Lerp(turret.position, targetPosition, moveSpeed * Time.deltaTime);
            turret.LookAt(player);

            // Checks to see if the next bullet can be fired
            if(Time.time >= nextShot)
            {
                nextShot = Time.time + 1f / fireRate;
                shoot();
            }
        }
    }

    // Shoots a bullet at the player
    void shoot()
    {
        soundEffect.Play();

        // Creates a clone of the bullet and fires it towards the player
        GameObject clone = Instantiate(bullet, barrel.position, transform.rotation);
        clone.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
        Destroy(clone, 1);
    }

    // When the player enters this objects detection range
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            fire = true;
        }
    }

    // When the player stops colliding with this objects detection range
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Reset();
        }
       
    }

    // Resets the turret to its deafult position
    private void Reset()
    {
        fire = false;
        targetPosition = ogPosition;
        turret.position = Vector3.Lerp(turret.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
