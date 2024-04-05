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
    void LateUpdate()
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
