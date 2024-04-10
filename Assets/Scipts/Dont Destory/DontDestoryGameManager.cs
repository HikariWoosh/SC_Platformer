using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestoryGameManager : MonoBehaviour
{
    [SerializeField]
    private static GameObject gameManagerInstance;

    void Awake()
    {
        if (gameManagerInstance != null)
        {
            Destroy(gameObject);
            return;
        }

        gameManagerInstance = gameObject;

        DontDestroyOnLoad(gameObject);
    }
}
