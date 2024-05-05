using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class timerScript : MonoBehaviour
{
    [Header("Timer control")]

    [SerializeField]
    private double time; // Used to count the time

    [SerializeField]
    private bool timeOn = true; // Bool to control if the timer is active

    [SerializeField]
    private TMP_Text timeText; // Text for the time to be displayed in

    [SerializeField]
    private TMP_Text MstimeText; // Text for the milliseconds


    // Start is called before the first frame update
    void Start()
    {
        // Initalises the timer
        timeOn = true;
        time = -1;
        updateTime((float)time);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeOn)
        {
            // Incrememnts the time value
           time += Time.deltaTime;


            if (time >= 3599998.000) // Check if time exceeds the limit
            {
                // Set time to the limit and stops counting
                time = 3599998.000; 
                timeOn = false; 
            }
            else
            {
                timeOn = true;
            }
            
            updateTime((float)time);
        }
    }

    // Function used to update the timer amount
    void updateTime(float timeAmount)
    {
        // Amount the timer is incremented by
        timeAmount += 1;

        // Value of each part of the timer
        double hours = Mathf.FloorToInt(timeAmount / 3600);
        double minutes = Mathf.FloorToInt((timeAmount % 3600) / 60);
        double seconds = Mathf.FloorToInt(timeAmount % 60);
        double milliseconds = Mathf.FloorToInt((timeAmount * 1000) % 1000);

        // Formatting the timers hours minutes and seconds
        timeText.text = string.Format("{0}:{1:00}:{2:00}.", hours, minutes, seconds);

        // Formatting the milliseconds
        MstimeText.text = string.Format("{0:000}", milliseconds);
    }

    // Executed when a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Resets the timer
        time = -1;
        updateTime((float)time);

        // Checks if the timer should be active in the current scene
        if (scene.name == "The Interstice" || scene.name == "Main Menu" || scene.name == "RoTComplete")
        {
            timeOn = false;
        }
        else
        {
            timeOn = true;
        }

    }
}