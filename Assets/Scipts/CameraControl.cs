using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private float rotateSpeed;

    [SerializeField]
    private Transform verticalPivot;

    [SerializeField]
    private Transform horizontalPivot;

    [SerializeField]
    private float maxView;

    [SerializeField]
    private float minView;

    [SerializeField]
    private bool invertY;

    // Start is called before the first frame update
    void Start()
    {
        offset = target.position - transform.position;

        verticalPivot.transform.position = target.transform.position;
        horizontalPivot.transform.position = target.transform.position;

        verticalPivot.transform.parent = horizontalPivot.transform;
        horizontalPivot.transform.parent = null;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        verticalPivot.transform.position = target.transform.position;

        float horizontalCamera = Input.GetAxis("Mouse X") * rotateSpeed;
        horizontalPivot.Rotate(0, horizontalCamera, 0);

        float verticalCamera = Input.GetAxis("Mouse Y") * rotateSpeed;
        if (invertY)
        {
            verticalPivot.Rotate(-verticalCamera, 0, 0);
        }
        else
        {
            verticalPivot.Rotate(verticalCamera, 0, 0);
        }

        if (verticalPivot.rotation.eulerAngles.x > maxView && verticalPivot.rotation.eulerAngles.x < 180.0f)
        {
            verticalPivot.rotation = Quaternion.Euler(maxView, verticalPivot.eulerAngles.y, 0.0f);
        }

        if (verticalPivot.rotation.eulerAngles.x > 180.0f && verticalPivot.rotation.eulerAngles.x < 360f + minView)
        {
            verticalPivot.rotation = Quaternion.Euler(360.0f + minView, verticalPivot.eulerAngles.y, 0.0f);
        }

        float yAngle = verticalPivot.eulerAngles.y;
        float xAngle = verticalPivot.eulerAngles.x;
   
        Quaternion rotationValue = Quaternion.Euler(xAngle, yAngle, 0);
        transform.position = target.position - (rotationValue * offset);



        //transform.position = target.position - offset;

        if (transform.position.y < target.position.y)
        {
            transform.position = new Vector3(transform.position.x, target.position.y - 0.9f, transform.position.z);
        }

        transform.LookAt(target);
    }
}
