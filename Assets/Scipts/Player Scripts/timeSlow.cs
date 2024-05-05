using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class timeSlow : MonoBehaviour
{

    [Header("Time Slow Variables")]
    [SerializeField]
    public bool isTimeSlow; // Checks if time is currently slowed

    [SerializeField]
    public bool slowTimeUnlocked; // Checks if the ability is unlocked

    [SerializeField]
    private float timeSlowDuration; // Amount of time slowed

    [SerializeField]
    private float timeSlowRecharge; // Recharge rate of ability

    [SerializeField]
    private float timeSlowDrain; // Drain rate of ability

    [SerializeField]
    private float timeSlowMax; // Max duration of ability

    [SerializeField]
    public bool noCostForSlow; // Used to toggle unlimited time slowing 


    [Header("Game Objects")]
    [SerializeField]
    private AudioMixer audioMixer; // Referece to the audio mixer 

    [SerializeField]
    private Image timeFilter; // Filter placed over screen during time slow

    [SerializeField]
    private PlayerController playerCharacter; // Refers to the players PlayerController

    [SerializeField]
    private GameObject UI; // Reference to the UI 

    [SerializeField]
    private Image EosCrystal; // Reference to the ability image

    [SerializeField]
    private Image EosCrystalLoad; // Reference to the ability empty image

    [SerializeField]
    private Image EosCrystalLocked; // Reference to the locked ability image


    // Update is called once per frame
    private void Start()
    {
        // Checks if the ability to slow time has been unlocked 
        if (PlayerPrefs.HasKey("timeSlowing") && PlayerPrefs.GetInt("timeSlowing") == 1)
        {
            slowTimeUnlocked = true;
        }
    }

    void Update()
    {
        if (slowTimeUnlocked && !UI.GetComponent<InGameMenu>().isPaused)
        {
            // Manages the ability icon respective to game state
            if(EosCrystal.enabled == false)
            {
                EosCrystal.enabled = true;
                EosCrystalLoad.enabled = true;

                EosCrystalLocked.enabled = false;
            }

            // If the player activates time slow either using left alt or right click, either enable or disable
            if (Input.GetButtonDown("SlowTime") && SceneManager.GetActiveScene().name != "Main Menu" && !noCostForSlow)
            {
                if (isTimeSlow)
                {
                    UnSlowTime();
                }
                else
                {
                    SlowTime();
                }
            }

            // If there is a cost to use time slow, deactivate when duration is 0
            if (isTimeSlow && !noCostForSlow)
            {
                timeSlowDuration -= timeSlowDrain * Time.deltaTime;
                if (timeSlowDuration <= 0f)
                {
                    UnSlowTime();
                }
            }
            else
            {
                // Allows for unlimited time slow
                if (timeSlowDuration <= timeSlowMax)
                {
                    timeSlowDuration += timeSlowRecharge * Time.deltaTime;
                }

            }

            // Updates the icons visual to show how much ability is left to use
            EosCrystal.fillAmount = timeSlowDuration / timeSlowMax;
        }
        else
        {
            // Manages the ability icon respective to game state
            EosCrystalLocked.enabled = true;

            EosCrystal.enabled = false;
            EosCrystalLoad.enabled = false;
        }

    }

    // Run when time slow is started
    public void SlowTime()
    {
        // Slows time and removes filter
        isTimeSlow = true;
        timeFilter.enabled = true;

        // Alters time scale to half that of the original amount
        Time.timeScale = 0.5f;

        // Adds sound filters and halfs the players gravity
        audioMixer.SetFloat("MasterPitch", 0.95f);
        audioMixer.SetFloat("MasterDepth", 0.4f);
        playerCharacter.GetComponent<PlayerController>().gravitySpeed = 2;

    }


    // Run when time slow runs out / is toggled
    public void UnSlowTime()
    {
        // Unslows time and removes filter
        isTimeSlow = false;
        timeFilter.enabled = false;

        // Returns time scale to correct amount
        Time.timeScale = 1f;

        // Removes sound filters and resets players gravity
        audioMixer.SetFloat("MasterPitch", 1f);
        audioMixer.SetFloat("MasterDepth", 0f);
        playerCharacter.GetComponent<PlayerController>().gravitySpeed = 4;

    }
}
