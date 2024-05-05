using UnityEngine;

public class DontDestroyCamera : MonoBehaviour
{
    [SerializeField]
    private static GameObject cameraInstance;


    // Allows the object to persist over scenes if it does not already exist
    void Awake()
    {
        if (cameraInstance != null)
        {
            Destroy(gameObject);
            return;
        }

        cameraInstance = gameObject;

        DontDestroyOnLoad(gameObject);
    }
}