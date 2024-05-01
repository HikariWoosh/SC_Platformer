using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class rotateOnPres : MonoBehaviour
{

    [Header("Controls")]
    [SerializeField]
    private GameObject foxStatue;

    [SerializeField]
    private bool isActive;

    [SerializeField]
    private float pullCooldown;

    [SerializeField]
    private float lastPullTime;

    [SerializeField]
    private bool Pulled;

    [SerializeField]
    private buttomPrompter buttomPrompt;

    [SerializeField]
    private statuePuzzleScript statuePuzzleScript;

    [SerializeField]
    private int rotationIndex;

    [SerializeField]
    private int correctIndex;

    [SerializeField]
    public bool inPlace;

    [SerializeField]
    private Quaternion targetRotation;

    // Start is called before the first frame update
    private void Start()
    {
        Invoke("findCameraPlayer", 0.2f);

        statueStart();
    }

    private void findCameraPlayer()
    {
        statuePuzzleScript = FindAnyObjectByType<statuePuzzleScript>();
        buttomPrompt = FindAnyObjectByType<buttomPrompter>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)  && Time.time >= lastPullTime + pullCooldown && isActive && !statuePuzzleScript.allInPlace)
        {
            lastPullTime = Time.time;
            Pulled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            buttomPrompt.hidePrompts();
            if (!statuePuzzleScript.allInPlace)
            {
                buttomPrompt.showE();
            }
            isActive = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player") && isActive && Pulled)
        {
            statuePuzzleScript.puzzleSolved();
            foxRotate();
            Pulled = false;
        }
    }

    private void statueStart()
    {

        if (rotationIndex == correctIndex)
        {
            inPlace = true;
        }
        else
        {
            inPlace = false;
        }
    }

    private void foxRotate()
    {
        targetRotation = foxStatue.transform.rotation * Quaternion.Euler(0, 90, 0);
        foxStatue.transform.rotation = Quaternion.Slerp(foxStatue.transform.rotation, targetRotation, 2f);
        if(rotationIndex == 4){
            rotationIndex = 1;
        }
        else {
            rotationIndex += 1;
        }

        if(rotationIndex == correctIndex)
        {
            inPlace = true;
        }
        else
        {
            inPlace = false;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        isActive = false;
        buttomPrompt.hidePrompts();
    }
}
