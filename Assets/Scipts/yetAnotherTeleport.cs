using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yetAnotherTeleport : MonoBehaviour
{
    [Header("Game Objects")]

    [SerializeField]
    private InGameMenu gameMenu;

    private void Start()
    {
        gameMenu = FindAnyObjectByType<InGameMenu>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && PlayerPrefs.HasKey("tutorialCompleted") && PlayerPrefs.GetInt("tutorialCompleted") == 1) // If the collider is the player
        {
            gameMenu.Interstice();
        }
    }
}
