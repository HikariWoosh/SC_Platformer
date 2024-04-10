using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocked2Di : MonoBehaviour
{
    [SerializeField]
    private GameObject Obsticle;

    [SerializeField]
    private Camera MainCamera;

    [SerializeField]
    private bool isActive;

    // Update is called once per frame
    private void Start()
    {
        Invoke("findCamera", 0.2f);
    }

    private void findCamera()
    {
        MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (MainCamera != null)
        {
            isActive = MainCamera.enabled;

            if (isActive)
            {
                Obsticle.SetActive(true);
            }
            else
            {
                Obsticle.SetActive(false);
            }
        }
    }
}
