using UnityEngine;

public class timeSlowArea : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField]
    private timeSlow script; // Reference to time slow script

    [SerializeField]
    private HealthControl healthControl; // Reference to health control script



    // Start is called before the first frame update
    void Start()
    {
        findObjects();
    }

    private void findObjects()
    {
        // Caches game components
        script = FindAnyObjectByType<timeSlow>();
        healthControl = GameObject.Find("gameManager").GetComponent<HealthControl>();
    }

    private void Update()
    {
        // If the player dies, unslow time
        if (healthControl.Health < 1)
        {

            script.UnSlowTime();
            script.noCostForSlow = false;
        }
    }

    // When the player enters the zone, slow time
    private void OnTriggerEnter(Collider other)
    {
        script.noCostForSlow = true;
        script.SlowTime();
        
    }

    // When the player leaves the zone, unslow time
    private void OnTriggerExit(Collider other)
    {
        script.UnSlowTime();
        script.noCostForSlow = false;
    }

}
