using UnityEngine;

public class DontDestoryUI : MonoBehaviour
{
    [SerializeField]
    private static GameObject UIInstance;


    // Allows the object to persist over scenes if it does not already exist
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
