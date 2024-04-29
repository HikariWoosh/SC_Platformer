using UnityEngine.SceneManagement;
using UnityEngine;

public class teleportBeginning : MonoBehaviour
{
    [Header("Game Objects")]

    [SerializeField]
    private GameObject linkedParticleObject;

    [SerializeField]
    private ParticleSystem linkedParticle;

    [SerializeField]
    private GameObject linkedJewel;

    [SerializeField]
    private InGameMenu gameMenu;

    private void Start()
    {
        gameMenu = FindAnyObjectByType<InGameMenu>();
    }

    private void Update()
    {
        if (!linkedJewel.activeSelf && !linkedParticleObject.activeSelf)
        {
            linkedParticleObject.SetActive(true);
            linkedParticle.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !linkedJewel.activeSelf) // If the collider is the player
        {
            PlayerPrefs.SetInt("tutorialCompleted", 1);
            gameMenu.Interstice(); 
        }
    }

}
