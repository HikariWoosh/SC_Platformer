using UnityEngine;

public class DontDestoryPivot : MonoBehaviour
{
    [SerializeField]
    private static GameObject pivotInstance;


    // Allows the object to persist over scenes if it does not already exist
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
