using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using Unity.VisualScripting;


public class MainMenu : MonoBehaviour
{
    [Header("Mixers")]

    [SerializeField]
    private AudioMixer myMix;


    [Header("Sliders")]

    [SerializeField]
    private Slider master_Slider;

    [SerializeField]
    private Slider music_Slider;

    [SerializeField]
    private Slider sfx_Slider;

    [Header("Dropdowns")]

    [SerializeField]
    private TMP_Dropdown resolutionSelect;

    [SerializeField]
    private TMP_Dropdown fullscreenSelect;

    [SerializeField]
    private TMP_Dropdown qualitySelect;


    [Header("Game Objects")]
    [SerializeField]
    private CameraControl cameraControl;

    [SerializeField]
    private Animator transition;

    [SerializeField]
    private float transitionTime;

    [SerializeField]
    private InGameMenu gameMenu;

    Resolution[] resolutions;

    private void Start()
    {
        transition = GameObject.Find("SceneTransition").GetComponent<Animator>();
        cameraControl = FindAnyObjectByType<CameraControl>();
        gameMenu = GameObject.Find("UI").GetComponent<InGameMenu>();

        LoadResolutions();

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

    public void PlayGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(LoadLevel("Beginning Sequence"));
    }

    private IEnumerator LoadLevel(string Level)
    {
        transition.SetBool("Fade", true);

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(Level);

        if (Level == "Beginning Sequence" && PlayerPrefs.HasKey("tutorialCompleted") && PlayerPrefs.GetInt("tutorialCompleted") == 1)
        {
            Debug.Log("Grabbing Assets");
        }
        else
        {
            transition.SetBool("Fade", false);
        }

       
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetMasterVolume()
    {
        float volume = master_Slider.value;
        myMix.SetFloat("Master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    private void LoadMasterVolume()
    {
        master_Slider.value = PlayerPrefs.GetFloat("masterVolume");
    }

    public void SetMusicVolume()
    {
        float volume = music_Slider.value;
        myMix.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    private void LoadMusicVolume()
    {
        music_Slider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    public void SetSFXVolume()
    {
        float volume = sfx_Slider.value;
        myMix.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    private void LoadSFXVolume()
    {
        sfx_Slider.value = PlayerPrefs.GetFloat("sfxVolume");
    }

    public void QualitySet(int qualityIndex)
    {
        Debug.Log(qualityIndex);
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("qualityLevel", qualityIndex);

    }

    private void LoadQuality()
    {
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("qualityLevel"));
        qualitySelect.value = PlayerPrefs.GetInt("qualityLevel");
        resolutionSelect.RefreshShownValue();
    }

    public void FullscreenSet(int fullscreenIndex)
    {
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
    private void LoadFullscreen()
    {
        int Loaded = PlayerPrefs.GetInt("fullscreen");
        FullscreenSet(Loaded);
        fullscreenSelect.value = PlayerPrefs.GetInt("fullscreen");
        fullscreenSelect.RefreshShownValue();
    }

    public void SetResolutin(int ResolutionIndex)
    {
        Resolution resolution = resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void LoadResolutions()
    {
        resolutions = Screen.resolutions; // Gets resolutions of current screen

        resolutionSelect.ClearOptions(); // Clears out all options in resolution dropdown

        List<string> options = new List<string>(); // List of string which will become options

        int currentResolution = 0;

        // Loops through each element in array and create an option for the list
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

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
