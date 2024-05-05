using UnityEngine;

public class teleportBeginning : MonoBehaviour
{
    [Header("Game Objects")]

    [SerializeField]
    private GameObject linkedParticleObject; // Particle object used by teleporter

    [SerializeField]
    private ParticleSystem linkedParticle; // Particle emitter used by teleporter

    [SerializeField]
    private GameObject linkedJewel; // Jewel required to power teleporter

    [SerializeField]
    private InGameMenu gameMenu; // Reference to InGameMenu script

    private void Start()
    {
        // Cahces the game menu
        gameMenu = FindAnyObjectByType<InGameMenu>();
    }

    private void Update()
    {
        // If the player has collected the linked jewel, display particle effect
        if (!linkedJewel.activeSelf && !linkedParticleObject.activeSelf)
        {
            linkedParticleObject.SetActive(true);
            linkedParticle.Play();
        }
    }

    // When the player collides, teleport the player
    private void OnTriggerEnter(Collider other)
    {
        // Ensures the respective jewel has been collected
        if (other.gameObject.tag == "Player" && !linkedJewel.activeSelf) 
        {
            // Updates the players save data
            PlayerPrefs.SetInt("tutorialCompleted", 1);
            gameMenu.Interstice(); 
        }
    }

}
