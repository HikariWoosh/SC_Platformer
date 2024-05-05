using UnityEngine;

public class DontDestory2DCamera : MonoBehaviour
{
    [SerializeField]
    private static GameObject camera2DInstance;


    // Allows the object to persist over scenes if it does not already exist
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
