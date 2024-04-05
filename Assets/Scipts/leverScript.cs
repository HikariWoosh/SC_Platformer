using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leverScript : MonoBehaviour
{
    [Header("Lever Controls")]


    [SerializeField]
    private bool isActive;

    [SerializeField]
    private float pullCooldown;

    [SerializeField]
    private float lastPullTime;

    [SerializeField]
    private bool Pulled;

    [SerializeField]
    private bool leverUsed;


    [Header("Game Objects")]


    [SerializeField]
    private GameObject Lever;

    [SerializeField]
    private GameObject TwoDLever;

    [SerializeField]
    private Camera MainCamera;

    [SerializeField]
    private Camera subCamera;

    [SerializeField]
    private GameObject Player;

    [SerializeField]
    private SphereCollider Collision;

    [SerializeField]
    private Animator anim; // Used to control animaiton 




    // Update is called once per frame
    void LateUpdate()
    {
        isActive = MainCamera.enabled;

        if (!isActive)
        {
            Lever.SetActive(true);
            Collision.enabled = true;
            TwoDLever.SetActive(false);
        }

        else
        {
            Lever.SetActive(false);
            Collision.enabled= false;
            TwoDLever.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.E) && leverUsed != true && Time.time >= lastPullTime + pullCooldown)
        {
            lastPullTime = Time.time;
            Pulled = true;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player") && !isActive && Pulled)
        {
            Debug.Log("Player pressed the 'E' key.");
            anim.SetTrigger("Pulled");
            Pulled = false;
            leverUsed = true;
        }
    }
    
}