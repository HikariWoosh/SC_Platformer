using UnityEngine;

public class Blocked2Di : MonoBehaviour
{
    [Header("Game Objects")]

    [SerializeField]
    private GameObject Obsticle; // Reference to the obsticle the script is using

    [SerializeField]
    private Camera MainCamera; // Main camera being used by the player


    [Header("Controls")]

    [SerializeField]
    private bool isActive; // Bool to check camera state

    // Update is called once per frame
    private void Start()
    {
        Invoke("findCamera", 0.2f);
    }

    // Caches camera
    private void findCamera()
    {
        MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Checks the cameras state to decide if the object is active or not
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
