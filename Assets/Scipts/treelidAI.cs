using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class treelidAI : MonoBehaviour
{

    [SerializeField]
    private Transform treelid;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector3 startPos;

    [SerializeField]
    private Vector3 movePosition;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject deathParticles; // Particles that are emitted upon death

    [SerializeField]
    private GameObject autumnJewel;

    [SerializeField]
    private float attackCounter;

    [SerializeField]
    private HealthControl healthControl;

    [SerializeField]
    private AudioSource deathSound;


    [Header("Enemy Controls")]

    [SerializeField]
    private float aggroRange;

    [SerializeField]
    private float returnTime;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float distance;

    [SerializeField]
    private float distanceFromPlayer;


    [Header("Waypoints")]

    [SerializeField]
    private bool roaming;

    [SerializeField]
    private List<GameObject> waypoints = new List<GameObject>(); 

    [SerializeField]
    private int currentWaypointIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        healthControl = GameObject.Find("gameManager").GetComponent<HealthControl>();
        startPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(startPos, target.position);
        distanceFromPlayer = Vector3.Distance(transform.position, target.position);

        if (transform.position.y >= 3)
        {
            animator.SetBool("running", false);
        }

        if (distanceFromPlayer <= 10f)
        {
            attackCounter = attackCounter + 0.1f;
        }
        else
        {
            attackCounter = 0;
        }


        if (transform.position.y < 3) 
        {

            if (distance <= aggroRange && healthControl.Health > 0)
            {
                chase();
                if(attackCounter > 5)
                {
                    attack();
                }
            }

            if (distance > aggroRange && !roaming)
            {
                deAggro();
            }

            if (roaming)
            {
                animator.SetBool("running", true);

                Vector3 currentWaypointPosition = waypoints[currentWaypointIndex].transform.position;

                if (Vector3.Distance(transform.position, currentWaypointPosition) <= 0.1f)
                {
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
                }

                transform.LookAt(currentWaypointPosition);
                transform.position = Vector3.MoveTowards(transform.position, currentWaypointPosition, speed * Time.deltaTime);
            }
        }

    }

    void chase()
    {
        roaming = false;

        animator.SetBool("running", true);
        movePosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(movePosition);
        transform.position = Vector3.MoveTowards(treelid.position, movePosition, speed * Time.deltaTime);
    }

    void deAggro()
    {

        if (Vector3.Distance(transform.position, startPos) > 5f)
        {
            transform.LookAt(startPos);
            transform.position = Vector3.Lerp(transform.position, startPos, returnTime * Time.deltaTime);
        }
        else
        {
            transform.position = startPos;
            roaming = true;
            animator.SetBool("running", false);
        }
    }

    void attack()
    {
        StartCoroutine(attackScript(1f));
    }

    IEnumerator attackScript(float damage)
    {
        animator.SetBool("attack", true);
        healthControl.damagePlayer(1);
        yield return new WaitForSeconds(damage);
        animator.SetBool("attack", false);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("water"))
        {
            GameObject Particles = Instantiate(deathParticles, transform.position, transform.rotation);
            Destroy(Particles, 1f);
            autumnJewel.transform.position = transform.position;
            deathSound.Play();
            Destroy(this.gameObject);
        }
    }

}
