using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    [Header("Game Objects")]
    [SerializeField]
    private Transform target; // The target the camera will follow

    [SerializeField]
    private Transform verticalPivot; // The vertical camera pivot ('Pivot')

    [SerializeField]
    private Transform horizontalPivot; // The horizontal camera pivot ('HPivot')


    [Header("Camera Controls")]
    [SerializeField]
    private Vector3 offset; // Offset between the player and the camera

    [SerializeField]
    private float rotateSpeed; // How fast the camera rotates around the player

    [SerializeField]
    private float maxView; // The maximum a player can look upwards

    [SerializeField]
    private float minView; // The maximum a player can look downwards

    [SerializeField]
    private bool invertY; // Control setting


    // Start is called before the first frame update
    void Start()
    {
        // Sets the offset position
        offset = target.position - transform.position; 

        // Sets the pivot locations
        verticalPivot.transform.position = target.transform.position; 
        horizontalPivot.transform.position = target.transform.position;

        // Sets the horizontral pivot as the parent of the vertical pivot
        verticalPivot.transform.parent = horizontalPivot.transform;
        horizontalPivot.transform.parent = null;


        // Makes it so the cursor cannot be seen during gameplay
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Sets the pivot to the same position as the player
        verticalPivot.transform.position = target.transform.position;


        // Controls the pivots rotations depending on how the player moves their mouse
        float horizontalCamera = Input.GetAxis("Mouse X") * rotateSpeed;
        horizontalPivot.Rotate(0, horizontalCamera, 0);

        float verticalCamera = Input.GetAxis("Mouse Y") * rotateSpeed;
        // Controls the Y invert (e.g if mouse goes up, camera goes down)
        if (invertY)
        {
            verticalPivot.Rotate(-verticalCamera, 0, 0);
        }
        else
        {
            verticalPivot.Rotate(verticalCamera, 0, 0);
        }


        // Controls how far up the player can look
        if (verticalPivot.rotation.eulerAngles.x > maxView && verticalPivot.rotation.eulerAngles.x < 180.0f)
        {
            verticalPivot.rotation = Quaternion.Euler(maxView, verticalPivot.eulerAngles.y, 0.0f);
        }

        // Controls how low down the player can look
        if (verticalPivot.rotation.eulerAngles.x > 180.0f && verticalPivot.rotation.eulerAngles.x < 360f + minView)
        {
            verticalPivot.rotation = Quaternion.Euler(360.0f + minView, verticalPivot.eulerAngles.y, 0.0f);
        }

        // Takes in the rotation of the pivots (vPivot inherits hPivots x)
        float yAngle = verticalPivot.eulerAngles.y;
        float xAngle = verticalPivot.eulerAngles.x;
   

        // Applies the pivot values
        Quaternion rotationValue = Quaternion.Euler(xAngle, yAngle, 0);
        transform.position = target.position - (rotationValue * offset);


        // Prevents the camera from flipping
        if (transform.position.y < target.position.y)
        {
            transform.position = new Vector3(transform.position.x, target.position.y - 0.9f, transform.position.z);
        }


        // Makes the camera look at the player
        transform.LookAt(target);
    }
}
