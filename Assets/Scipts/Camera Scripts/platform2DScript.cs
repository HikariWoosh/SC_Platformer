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
        Invoke("findCamera", 0.01f);
    }

    // Caches camera
    private void findCamera()
    {
        MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void LateUpdate()
    {
        // Checks the cameras state to decide if the object is active or not
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
