using System.Runtime.CompilerServices;
using UnityEngine;

public class DontDestroyCamera : MonoBehaviour
{
    [SerializeField]
    private static GameObject cameraInstance;

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