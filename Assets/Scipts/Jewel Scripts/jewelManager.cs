using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JewelManager : MonoBehaviour
{
    [Header("Jewel Controls")]

    [SerializeField]
    private List<GameObject> jewels = new List<GameObject>(); // List to hold each Jewel

    [SerializeField]
    public int maxJewelCount; // Max amount of jewels 

    [SerializeField]
    private int jewelCount; // Current amount of jewels

    [SerializeField]
    private AudioSource collectSound; // Audio when collected


    [Header("Game Objects")]

    [SerializeField]
    private InGameMenu gameMenu; // Reference to InGameMenu script

    [SerializeField]
    private timeSlow timeSlow; // Reference to timeSlow script

    // Start is called before the first frame update
    void Start()
    {
        // Cache components
        gameMenu = GameObject.Find("UI").GetComponent<InGameMenu>();
        timeSlow = GameObject.Find("gameManager").GetComponent<timeSlow>();

        // For each child Jewel, add it to the list
        foreach (Transform child in transform)
        {
            GameObject jewelObject = child.gameObject;
            jewels.Add(jewelObject);
     
        }

        // Set max value and current value to length of the list
        maxJewelCount = jewels.Count;
        jewelCount = jewels.Count;
    }

    // Function run when the player collects a jewel
    public void jewelCollected(GameObject Collected)
    {
        // If the picked up jewel is in the list
        if (Collected != null && jewels.Contains(Collected))
        {
            collectSound.Play();

            // Remove the jewel and recalculate the count
            jewels.Remove(Collected);

            jewelCount = jewels.Count;
        }

        // If there is nothing left in the list, run allJewelsCollected()
        if (jewelCount == 0)
        {
            allJewelsCollected();
        }
    }

    // Function run when every jewel is collected
    public void allJewelsCollected()
    {
        // Depending on the scene, teleport the player somewhere
        if (SceneManager.GetActiveScene().name == "Realm Of Time")
        {
            if(timeSlow.slowTimeUnlocked != true)
            {
                gameMenu.RoTC();
            }
            else
            {
                gameMenu.Interstice();
            }
        }
    }
}
