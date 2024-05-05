using UnityEngine;


public class rotateScript : MonoBehaviour
{
    [Header("Rotation Controls")]

    [SerializeField]
    private float rotationSpeed; // Speed of rotation

    void Update()
    {
        // Rotates the object on its Z-Axis
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
