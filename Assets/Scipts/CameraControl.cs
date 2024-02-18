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
    private Transform Pivot;

    // Start is called before the first frame update
    void Start()
    {
        offset = target.position - transform.position;

        Pivot.transform.position = target.transform.position;
        Pivot.transform.parent = target.transform;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float horizontalCamera = Input.GetAxis("Mouse X") * rotateSpeed;
        target.Rotate(0, horizontalCamera, 0);

        float verticalCamera = Input.GetAxis("Mouse Y") * rotateSpeed;
        Pivot.Rotate(-verticalCamera, 0, 0);

        float yAngle = target.eulerAngles.y;
        float xAngle = Pivot.eulerAngles.x;
   
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
