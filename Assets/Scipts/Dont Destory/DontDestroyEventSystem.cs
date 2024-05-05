using UnityEngine;

public class DontDestroyEventSystem : MonoBehaviour
{
    [SerializeField]
    private static GameObject eventSystemInstance;


    // Allows the object to persist over scenes if it does not already exist
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
