using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class timerScript : MonoBehaviour
{

    [SerializeField]
    private double time;

    [SerializeField]
    private bool timeOn = true;

    [SerializeField]
    private TMP_Text timeText;

    [SerializeField]
    private TMP_Text MstimeText;


    // Start is called before the first frame update
    void Start()
    {
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

           time += Time.deltaTime;


            if (time >= 3599998.000) // Check if time exceeds the limit
            {
                time = 3599998.000; // Set time to the limit
                timeOn = false; // Stop counting
            }
            else
            {
                timeOn = true;
            }
            updateTime((float)time);
        }
    }

    void updateTime(float timeAmount)
    {
        timeAmount += 1;

        double hours = Mathf.FloorToInt(timeAmount / 3600);
        double minutes = Mathf.FloorToInt((timeAmount % 3600) / 60);
        double seconds = Mathf.FloorToInt(timeAmount % 60);
        double milliseconds = Mathf.FloorToInt((timeAmount * 1000) % 1000);

        timeText.text = string.Format("{0}:{1:00}:{2:00}.", hours, minutes, seconds);

        MstimeText.text = string.Format("{0:000}", milliseconds);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        time = -1;
        updateTime((float)time);
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