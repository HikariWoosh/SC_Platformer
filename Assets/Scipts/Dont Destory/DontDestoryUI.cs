using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestoryUI : MonoBehaviour
{
    [SerializeField]
    private static GameObject UIInstance;

    void Awake()
    {
        if (UIInstance != null)
        {
            Destroy(gameObject);
            return;
        }

        UIInstance = gameObject;

        DontDestroyOnLoad(gameObject);
    }
}
