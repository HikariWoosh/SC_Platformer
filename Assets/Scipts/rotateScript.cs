using UnityEngine;


public class rotateScript : MonoBehaviour
{

    [SerializeField]
    private float rotationSpeed;

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
