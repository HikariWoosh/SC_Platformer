using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyTransition : MonoBehaviour
{
    [SerializeField]
    private static GameObject transitionSystemInstance;

    void Awake()
    {

        if (transitionSystemInstance != null)
        {
            Destroy(gameObject);
            return;
        }

        transitionSystemInstance = gameObject;

        DontDestroyOnLoad(gameObject);
    }
}
