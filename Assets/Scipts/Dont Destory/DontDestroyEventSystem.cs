using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyEventSystem : MonoBehaviour
{
    [SerializeField]
    private static GameObject eventSystemInstance;

    void Awake()
    {
        if (eventSystemInstance != null)
        {
            Destroy(gameObject);
            return;
        }

        eventSystemInstance = gameObject;

        DontDestroyOnLoad(gameObject);
    }
}
