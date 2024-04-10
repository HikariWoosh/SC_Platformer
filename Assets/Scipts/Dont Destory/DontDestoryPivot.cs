using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestoryPivot : MonoBehaviour
{
    [SerializeField]
    private static GameObject pivotInstance;

    void Awake()
    {
        if (pivotInstance != null)
        {
            Destroy(gameObject);
            return;
        }

        pivotInstance = gameObject;

        DontDestroyOnLoad(gameObject);
    }
}
