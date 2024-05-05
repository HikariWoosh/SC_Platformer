using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;


public class MainMenu : MonoBehaviour
{
    [Header("Mixers")]

    [SerializeField]
    private AudioMixer myMix; // Reference to the audio mixer


    [Header("Sliders")]

    [SerializeField]
    private Slider master_Slider; // Slider used to control master volume

    [SerializeField]
    private Slider music_Slider; // Slider used to control music volume

    [SerializeField]
    private Slider sfx_Slider; // Slider used to control SFX volume

    [Header("Dropdowns")]

    [SerializeField]
    private TMP_Dropdown resolutionSelect; // Resolution dropdown

    [SerializeField]
    private TMP_Dropdown fullscreenSelect; // Fullscreen dropdown

    [SerializeField]
    private TMP_Dropdown qualitySelect; // Quality dropdown


    [Header("Game Objects")]

    [SerializeField]
    private Animator transition; // Reference to the transitions animator

    [SerializeField]
    private float transitionTime; // Amount of time for a screen transition

    Resolution[] resolutions;

    private void Start()
    {
        // Caches the transition component and loads resoulutions
        transition = GameObject.Find("SceneTransition").GetComponent<Animator>();
        LoadResolutions();

        // Checks to see if the player has values in place for each setting, if not sets them
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            LoadMasterVolume();
        }
        else
        {
            SetMasterVolume();
        }

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadMusicVolume();
        }
        else
        {
            SetMusicVolume();
        }

        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadSFXVolume();
        }
        else
        {
            SetSFXVolume();
        }


        if (PlayerPrefs.HasKey("fullscreen"))
        {
            LoadFullscreen();
        }

        if (PlayerPrefs.HasKey("qualityLevel"))
        {
            LoadQuality();
        }
        else
        {
            QualitySet(0);
        }
    }

    // Function used when play is pressed
    public void PlayGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(LoadLevel("Beginning Sequence"));
    }

    // Function used to load new levels
    private IEnumerator LoadLevel(string Level)
    {
        // Starts the transition effect
        transition.SetBool("Fade", true);
        yield return new WaitForSeconds(transitionTime);

        // Loads the new level
        SceneManager.LoadScene(Level);

        // Checks to see if the tutorial has been beaten
        if (Level == "Beginning Sequence" && PlayerPrefs.HasKey("tutorialCompleted") && PlayerPrefs.GetInt("tutorialCompleted") == 1)
        {
            yield return null;
        }
        else
        {
            transition.SetBool("Fade", false);
        }

       
    }

    // Exits the application when the quit button is pressed
    public void QuitGame()
    {
        Application.Quit();
    }

    // Function used to control the master volume
    public void SetMasterVolume()
    {
        // Gets the new sound value
        float volume = master_Slider.value;

        // Sets the new value
        myMix.SetFloat("Master", Mathf.Log10(volume) * 20);

        // Saves the new value
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    // Function used to load the players master volume setting 
    private void LoadMasterVolume()
    {
        master_Slider.value = PlayerPrefs.GetFloat("masterVolume");
    }

    // Function used to control the music volume
    public void SetMusicVolume()
    {
        float volume = music_Slider.value;
        myMix.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    // Function used to load the players music volume setting 
    private void LoadMusicVolume()
    {
        music_Slider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    // Function used to control the SFX volume
    public void SetSFXVolume()
    {
        float volume = sfx_Slider.value;
        myMix.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    // Function used to load the players SFX volume setting 
    private void LoadSFXVolume()
    {
        sfx_Slider.value = PlayerPrefs.GetFloat("sfxVolume");
    }

    // Sets the quality of the game using unity quality settings
    public void QualitySet(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("qualityLevel", qualityIndex);

    }

    // Loads the players quality settings
    private void LoadQuality()
    {
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("qualityLevel"));
        qualitySelect.value = PlayerPrefs.GetInt("qualityLevel");

        // Refresh the value shown by the game UI
        resolutionSelect.RefreshShownValue();
    }

    // Controls fullscreen setting
    public void FullscreenSet(int fullscreenIndex)
    {
        // Either enables or disables fullscreen and saves the value
        if (fullscreenIndex == 0)
        {
            Screen.fullScreen = true;
            PlayerPrefs.SetInt("fullscreen", fullscreenIndex);
        }
        else
        {
            Screen.fullScreen = false;
            PlayerPrefs.SetInt("fullscreen", fullscreenIndex);
        }

    }

    // Loads the players fullscreen value
    private void LoadFullscreen()
    {
        int Loaded = PlayerPrefs.GetInt("fullscreen");
        FullscreenSet(Loaded);

        // Refreshes the value shown by the UI
        fullscreenSelect.value = PlayerPrefs.GetInt("fullscreen");
        fullscreenSelect.RefreshShownValue();
    }

    // Sets the players resolution depending on the option selected from the array
    public void SetResolutin(int ResolutionIndex)
    {
        Resolution resolution = resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // Loads the players resoultion
    private void LoadResolutions()
    {
        // Puts all possible resolutions into an array
        resolutions = Screen.resolutions;

        // Clears out all options in resolution dropdown
        resolutionSelect.ClearOptions();

        // List of string which will become options
        List<string> options = new List<string>();
        int currentResolution = 0;

        // Loops through each element in array and create an option for the list
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            // If the users resolution is found, set that as the current resolutions
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolution = i;
            }
        }

        // Adds the option list to the dropdwon
        resolutionSelect.AddOptions(options);
        resolutionSelect.value = currentResolution;
        resolutionSelect.RefreshShownValue();
    }
}
