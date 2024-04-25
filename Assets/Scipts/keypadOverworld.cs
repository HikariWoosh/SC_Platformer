using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class keypadOverworld : MonoBehaviour
{

    [Header("Lever Controls")]


    [SerializeField]
    private float pullCooldown;

    [SerializeField]
    private float lastPullTime;

    [SerializeField]
    private bool isActive;

    [SerializeField]
    private bool Pulled;

    [SerializeField]
    private bool keypadUsed;

    [SerializeField]
    private bool canPull;


    [Header("Game Objects")]

    [SerializeField]
    private GameObject Keypad;

    [SerializeField]
    private Canvas keyCanvas;

    [SerializeField]
    private GameObject Player;

    [SerializeField]
    private GameObject MainCamera;

    [SerializeField]
    private InGameMenu ingameMenu;

    [SerializeField]
    private SphereCollider Collision;


    private void Start()
    {
        Invoke("findPlayer", 0.2f);
    }

    private void findPlayer()
    {
        MainCamera = GameObject.Find("Main Camera");
        Player = GameObject.Find("Player");
        ingameMenu = FindAnyObjectByType<InGameMenu>();
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E) && keypadUsed != true && Time.time >= lastPullTime + pullCooldown && canPull)
        {
            lastPullTime = Time.time;
            Pulled = true;
        }

        if (isActive && !ingameMenu.isPaused) 
        {
            keyCanvas.sortingOrder = 6;
            if (Cursor.visible == false)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else
        {
            keyCanvas.sortingOrder = 1;
        }
    }

    public void closeKeypad()
    {
        Cursor.visible = false;
        MainCamera.GetComponent<CameraControl>().SetRotateSpeed(1.5f);
        Cursor.lockState = CursorLockMode.Locked;

        Pulled = false;
        isActive = false;
        Keypad.SetActive(false);
    }

    private void openKeypad()
    {
        Cursor.visible = true;
        MainCamera.GetComponent<CameraControl>().SetRotateSpeed(0);
        Cursor.lockState = CursorLockMode.None;

        Pulled = false;
        isActive = true;
        Keypad.SetActive(true);

    }

    private void OnTriggerEnter(Collider other)
    {
        canPull = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player") && isActive && Pulled)
        {
            closeKeypad();
        }

        if (other.tag.Equals("Player") && !isActive && Pulled)
        {
            openKeypad();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") // If the collider is the player
        {
            canPull = false;
            closeKeypad();
        }
    }
}
