using System.Runtime.CompilerServices;
using UnityEngine;

public class DontDestroyPlayer : MonoBehaviour
{
    [SerializeField]
    private static GameObject playerInstance;

    void Awake()
    {
        if (playerInstance != null)
        {
            Destroy(gameObject);
            return;
        }

        playerInstance = gameObject;

        DontDestroyOnLoad(gameObject);
    }
}