using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestory2DCamera : MonoBehaviour
{
    [SerializeField]
    private static GameObject camera2DInstance;

    void Awake()
    {
        if (camera2DInstance != null)
        {
            Destroy(gameObject);
            return;
        }

        camera2DInstance = gameObject;

        DontDestroyOnLoad(gameObject);
    }
}
