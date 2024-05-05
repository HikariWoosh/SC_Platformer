using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yetAnotherTeleport : MonoBehaviour
{
    [Header("Game Objects")]

    [SerializeField]
    private InGameMenu gameMenu; // Reference to the InGameMenu

    private void Start()
    {
        // Caches game menu
        gameMenu = FindAnyObjectByType<InGameMenu>();
    }


    // When the player touches the telporter
    private void OnTriggerEnter(Collider other)
    {
        // If save data has already beat the tutorial, teleport the player to level select
        if (other.gameObject.tag == "Player" && PlayerPrefs.HasKey("tutorialCompleted")) // If the collider is the player
        {
            gameMenu.Interstice();
        }
    }
}
