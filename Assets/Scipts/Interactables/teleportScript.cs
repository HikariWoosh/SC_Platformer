using UnityEngine;

public class teleportScript : MonoBehaviour
{

    [Header("Game Objects")]

    [SerializeField]
    private HealthControl HealthControl; // Reference to health control script

    [SerializeField]
    private AudioSource Music; // Audio of current area

    [SerializeField]
    private GameObject linkedParticleObject; // Particle object emitted by the teleporter

    [SerializeField]
    private ParticleSystem linkedParticle; // Particle emitter used by the teleporter

    [SerializeField]
    private GameObject linkedJewel; // Jewel required to power teleporter

    [SerializeField]
    private GameObject BGM; // BGM of current level



    private void Start()
    {
        Invoke("findPlayer", 0.2f);
    }

    private void Update()
    {
        // if the Jewel linked to the teleporter is collected, display particle effects
        if (!linkedJewel.activeSelf && !linkedParticleObject.activeSelf)
        {
            linkedParticleObject.SetActive(true);
            linkedParticle.Play();
        }
    }

    private void findPlayer()
    {
        // Cache the health controller
        HealthControl = FindAnyObjectByType<HealthControl>();
    }

    // Resets the music of the current scene
    private void resetMusic()
    {
        Music.Stop();
        Music.enabled = false;

        BGM.SetActive(true);
    }

    // When the player interacts with the teleporter
    private void OnTriggerEnter(Collider other)
    {
        // If the jewel has been collected, teleports the player to spawn 
        if (other.gameObject.tag == "Player" && !linkedJewel.activeSelf) 
        {
            HealthControl.RoT();
            resetMusic();
        }
    }



}
