using UnityEngine;

public class skyboxManager : MonoBehaviour
{
    [Header("Skybox Controls")]

    [SerializeField]
    private float skySpeed; // Speed of rotation

    // Update is called once per frame
    void Update()
    {
        // Rotates the skybox
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * skySpeed);
    }
}
