using UnityEngine;

public class DontDestoryGameManager : MonoBehaviour
{
    [SerializeField]
    private static GameObject gameManagerInstance;


    // Allows the object to persist over scenes if it does not already exist
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
