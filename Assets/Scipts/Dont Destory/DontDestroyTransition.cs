using UnityEngine;

public class DontDestroyTransition : MonoBehaviour
{
    [SerializeField]
    private static GameObject transitionSystemInstance;


    // Allows the object to persist over scenes if it does not already exist
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
