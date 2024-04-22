using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportScript : MonoBehaviour
{

    [Header("Game Objects")]

    [SerializeField]
    private GameObject Player;

    [SerializeField]
    private HealthControl HealthControl;

    [SerializeField]
    private AudioSource Music;

    [SerializeField]
    private GameObject BGM;



    private void Start()
    {
        Invoke("findPlayer", 0.2f);
    }

    private void findPlayer()
    {
        Player = GameObject.Find("Player");
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
        if (other.gameObject.tag == "Player") // If the collider is the player
        {
            HealthControl.RoT();
            resetMusic();
        }
    }



}
