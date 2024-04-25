using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform2DScript : MonoBehaviour
{

    [Header("Platform Controls")]

    [SerializeField]
    private bool isActive;


    [Header("Game Objects")]


    [SerializeField]
    private GameObject Platform;

    [SerializeField]
    private GameObject TwoDPlatform;

    [SerializeField]
    private Camera MainCamera;

    private void Start()
    {
        Invoke("findCameraPlayer", 0.01f);
    }

    private void findCameraPlayer()
    {
        MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (MainCamera != null)
        {

            isActive = MainCamera.enabled;

            if (!isActive)
            {
                Platform.SetActive(true);
                TwoDPlatform.SetActive(false);
            }

            else
            {
                Platform.SetActive(false);
                TwoDPlatform.SetActive(true);
            }
        }
    }

}
