using System.Collections.Generic;
using UnityEngine;

public class statuePuzzleScript : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> Statues = new List<GameObject>();

    [SerializeField]
    public bool allInPlace;

    [SerializeField]
    private GameObject jewel;

    [SerializeField]
    private Vector3 startPoint;

    [SerializeField]
    private GameObject jewelPoint;

    [SerializeField]
    private AudioSource solvedSound;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = jewel.transform.position;

        foreach (Transform child in transform)
        {
            GameObject statue = child.gameObject;
            Statues.Add(statue);

        }

    }


    public void puzzleSolved()
    {
        allInPlace = true;

        foreach (GameObject statue in Statues)
        {
            if (!statue.GetComponent<rotateOnPres>().inPlace)
            {
                allInPlace = false;
                break; 
            }
        }

        if (allInPlace)
        {
            jewel.transform.position = jewelPoint.transform.position;
            solvedSound.Play();
        }
        else
        {
            jewel.transform.position = startPoint;
        }

    }

}
