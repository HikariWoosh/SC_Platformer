using UnityEngine;

public class timeSlowArea : MonoBehaviour
{
    [SerializeField]
    private timeSlow script;

    [SerializeField]
    private HealthControl healthControl;



    // Start is called before the first frame update
    void Start()
    {
        findObjects();
    }

    private void findObjects()
    {
        script = FindAnyObjectByType<timeSlow>();
        healthControl = GameObject.Find("gameManager").GetComponent<HealthControl>();
    }

    private void Update()
    {
        if (healthControl.Health < 1)
        {

            script.UnSlowTime();
            script.noCostForSlow = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        script.noCostForSlow = true;
        script.SlowTime();
        
    }

    private void OnTriggerExit(Collider other)
    {
        script.UnSlowTime();
        script.noCostForSlow = false;
    }

}
