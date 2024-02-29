using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skyboxManager : MonoBehaviour
{
    [Header("Skybox Controls")]
    [SerializeField]
    private float skySpeed;

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * skySpeed); // Rotates the skybox
    }
}
