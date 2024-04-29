using UnityEngine;

public class teleportScript : MonoBehaviour
{

    [Header("Game Objects")]

    [SerializeField]
    private HealthControl HealthControl;

    [SerializeField]
    private AudioSource Music;

    [SerializeField]
    private GameObject linkedParticleObject;

    [SerializeField]
    private ParticleSystem linkedParticle;

    [SerializeField]
    private GameObject linkedJewel;

    [SerializeField]
    private GameObject BGM;



    private void Start()
    {
        Invoke("findPlayer", 0.2f);
    }

    private void Update()
    {
        if (!linkedJewel.activeSelf && !linkedParticleObject.activeSelf)
        {
            linkedParticleObject.SetActive(true);
            linkedParticle.Play();
        }
    }

    private void findPlayer()
    {
        HealthControl = FindAnyObjectByType<HealthControl>();
    }

    private void resetMusic()
    {
        Music.Stop();
        Music.enabled = false;

        BGM.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !linkedJewel.activeSelf) // If the collider is the player
        {
            HealthControl.RoT();
            resetMusic();
        }
    }



}
