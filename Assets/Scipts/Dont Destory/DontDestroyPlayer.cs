using UnityEngine;

public class DontDestroyPlayer : MonoBehaviour
{
    [SerializeField]
    private static GameObject playerInstance;


    // Allows the object to persist over scenes if it does not already exist
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