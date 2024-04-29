using UnityEngine;

public class CameraControlSecondDimension : MonoBehaviour
{
    [Header("Camera Controls")]
    [SerializeField]
    private Transform target; // The target the camera will follow

    [SerializeField]
    private Vector3 offset; // Offset between the player and the camera

    [SerializeField]
    private Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        // Sets the offset position
        offset = target.position - transform.position;
    }

    public void changeOffset(Vector3 newOffset, Quaternion newRotation)
    {
        offset = newOffset;
        rotation = newRotation;
    }

    // Called once per frame after Update()
    void LateUpdate()
    {
        // Create a new Vector3 position based on the players position
        Vector3 newPosition = transform.position;
        newPosition.z = target.position.z - offset.z;
        newPosition.y = target.position.y - offset.y;
        newPosition.x = target.position.x - offset.x;
        transform.position = newPosition;

        // Change the 2d cameras position based on the player
        transform.rotation = rotation;
    }
}