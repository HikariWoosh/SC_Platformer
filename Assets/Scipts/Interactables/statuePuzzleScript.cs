using System.Collections.Generic;
using UnityEngine;

public class statuePuzzleScript : MonoBehaviour
{
    [Header("Statue Controls")]

    [SerializeField]
    private List<GameObject> Statues = new List<GameObject>(); // List to hold all statues

    [SerializeField]
    public bool allInPlace; // Bool to detect if all statues are in place

    [SerializeField]
    private GameObject jewel; // Jewel used when puzzle is completed

    [SerializeField]
    private GameObject jewelPoint; // Object the jewel teleports to upon completion

    [SerializeField]
    private AudioSource solvedSound; // Sound effect played upon solving

    // Start is called before the first frame update
    void Start()
    {

        // Populates the statues list with all statues
        foreach (Transform child in transform)
        {
            GameObject statue = child.gameObject;
            Statues.Add(statue);

        }

    }


    // Function run when the puzzle is solved
    public void puzzleSolved()
    {
        allInPlace = true;

        // Checks each statues position to check if they are in place
        foreach (GameObject statue in Statues)
        {
            if (!statue.GetComponent<rotateOnPres>().inPlace)
            {
                // If a single one is wrong, exit the loop
                allInPlace = false;
                break; 
            }
        }

        // If the puzzle is solved move the jewel to the correct position and play solved sound
        if (allInPlace)
        {
            jewel.transform.position = jewelPoint.transform.position;
            solvedSound.Play();
        }
    }

}
