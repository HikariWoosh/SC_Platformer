using UnityEngine;

public class localMusicScript : MonoBehaviour
{
    [Header("Game Objects")]

    [SerializeField]
    private AudioSource Music; // New music for the are

    [SerializeField]
    private GameObject BGM; // BGM used in the scene

    void Start()
    {
        // Load the audio clip into memory
        Music.clip.LoadAudioData();
    }

    // When the player enters the collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            // Enable new music and disable scene BGM
            Music.enabled = true;  
            Music.Play();

            BGM.SetActive(false);
        }
    }

    // When the collider stops colliding with this object
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            //Disables new music and re-enables scene BGM
            Music.Stop();
            Music.enabled = false;

            BGM.SetActive(true);
        }

    }
}
