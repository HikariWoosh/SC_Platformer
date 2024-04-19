using UnityEngine;

public class turretScript : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private Transform turret;

    [SerializeField]
    private HealthControl healthControl;

    [SerializeField]
    private Vector3 ogPosition;

    [SerializeField]
    private Vector3 targetPosition;

    [SerializeField]
    private Transform barrel;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private float distance;

    [SerializeField]
    private bool fire;

    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float fireRate;

    [SerializeField]
    private float nextShot;

    [SerializeField]
    private float inRange;

    [SerializeField]
    private AudioSource soundEffect;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        healthControl = GameObject.Find("gameManager").GetComponent<HealthControl>();
        ogPosition = turret.transform.position;
        targetPosition = ogPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthControl.Health == 0)
        {
            Reset();
        }

        if (fire) 
        {
            targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);
            turret.position = Vector3.Lerp(turret.position, targetPosition, moveSpeed * Time.deltaTime);
            //turret.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
            turret.LookAt(player);
            if(Time.time >= nextShot)
            {
                nextShot = Time.time + 1f / fireRate;
                shoot();
            }
        }
    }

    void shoot()
    {
        soundEffect.Play();
        GameObject clone = Instantiate(bullet, barrel.position, transform.rotation);
        clone.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
        Destroy(clone, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") // If the collider is the player
        {
            fire = true;
        }
    }

    // When the collider stops colliding with this object
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") // If the collider is the player
        {
            Reset();
        }
       
    }

    private void Reset()
    {
        fire = false;
        targetPosition = ogPosition;
        turret.position = Vector3.Lerp(turret.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
