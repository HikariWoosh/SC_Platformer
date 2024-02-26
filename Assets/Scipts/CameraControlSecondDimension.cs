using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlSecondDimension : MonoBehaviour
{
    [SerializeField]
    private Transform target; // The target the camera will follow

    [SerializeField]
    private Vector3 offset; // Offset between the player and the camera

    // Start is called before the first frame update
    void Start()
    {
        // Sets the offset position
        offset = target.position - transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Update the camera's position based on the playes Z position
        Vector3 newPosition = transform.position;
        newPosition.z = target.position.z - offset.z;
        newPosition.y = target.position.y - offset.y;
        newPosition.x = target.position.x - offset.x;
        transform.position = newPosition;

        // Change the Z axis whilst maintaing others
        transform.eulerAngles = new Vector3(
            target.eulerAngles.x,
            transform.eulerAngles.y,
            target.eulerAngles.z
        );
    }
}