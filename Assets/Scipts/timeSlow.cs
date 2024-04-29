using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class timeSlow : MonoBehaviour
{

    [Header("Time Slow Variables")]
    [SerializeField]
    public bool isTimeSlow;

    [SerializeField]
    private bool slowTimeUnlocked;

    [SerializeField]
    private float timeSlowDuration;

    [SerializeField]
    private float timeSlowRecharge;

    [SerializeField]
    private float timeSlowDrain;

    [SerializeField]
    private float timeSlowMax;

    [SerializeField]
    public bool noCostForSlow;


    [Header("Game Objects")]
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private Image timeFilter;

    [SerializeField]
    private PlayerController playerCharacter; // Refers to the players PlayerController

    [SerializeField]
    private GameObject UI;

    [SerializeField]
    private Image EosCrystal;

    // Update is called once per frame
    void Update()
    {
        if (slowTimeUnlocked && !UI.GetComponent<InGameMenu>().isPaused)
        {
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
                if (timeSlowDuration <= timeSlowMax)
                {
                    timeSlowDuration += timeSlowRecharge * Time.deltaTime;
                }

            }

            EosCrystal.fillAmount = timeSlowDuration / timeSlowMax;
        }

    }

    public void SlowTime()
    {
        isTimeSlow = true;
        timeFilter.enabled = true;

        Time.timeScale = 0.5f;

        audioMixer.SetFloat("MasterPitch", 0.95f);
        audioMixer.SetFloat("MasterDepth", 0.4f);
        playerCharacter.GetComponent<PlayerController>().gravitySpeed = 2;

    }



    public void UnSlowTime()
    {
        isTimeSlow = false;
        timeFilter.enabled = false;

        Time.timeScale = 1f;

        audioMixer.SetFloat("MasterPitch", 1f);
        audioMixer.SetFloat("MasterDepth", 0f);
        playerCharacter.GetComponent<PlayerController>().gravitySpeed = 4;

    }
}
