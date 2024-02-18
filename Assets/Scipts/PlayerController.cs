using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float jumpHeight;

    [SerializeField]
    private float gravity;

    [SerializeField]
    private float gravitySpeed;

    private Vector3 moveDirection;

    public CharacterController cc;


    // Start is called before the first frame update
    void Start()
    {
        cc = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 0f, Input.GetAxis("Vertical") * moveSpeed);

        if(Input.GetButtonDown("Jump"))
        {
            moveDirection.y = jumpHeight;
        }

        moveDirection.y = moveDirection.y + (gravity * gravitySpeed * Time.deltaTime);

        // Time.deltaTime is the time since the last frame e.g 60fps = 1/60s
        cc.Move(moveDirection * Time.deltaTime);
    }

}
