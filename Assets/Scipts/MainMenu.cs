using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    public AudioMixer myMix;

    [SerializeField]
    public Slider vol_Slider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetVolume();
        }

        if (PlayerPrefs.HasKey("qualityLevel"))
        {
            LoadQuality();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetVolume()
    {
        float volume = vol_Slider.value;
        myMix.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    private void LoadVolume() 
    {
        vol_Slider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    public void QualitySet(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("qualityLevel", qualityIndex);

    }

    private void LoadQuality()
    {
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("qualityLevel"));
    }

}
