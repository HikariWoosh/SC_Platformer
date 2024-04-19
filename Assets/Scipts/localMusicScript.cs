using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class localMusicScript : MonoBehaviour
{

    [SerializeField]
    private AudioSource Music;

    [SerializeField]
    private GameObject BGM;

    void Start()
    {
        // Load the audio clip into memory
        Music.clip.LoadAudioData();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") // If the collider is the player
        {
            Music.enabled = true;  
            Music.Play();

            BGM.SetActive(false);
        }
    }

    // When the collider stops colliding with this object
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") // If the collider is the player
        {
            Music.Stop();
            Music.enabled = false;

            BGM.SetActive(true);
        }

    }
}
