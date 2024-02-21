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

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private Transform Pivot;

    [SerializeField]
    private float rotateSpeed;

    [SerializeField]
    private GameObject playerModel;


    private Vector3 moveDirection;

    private CharacterController cc;


    // Start is called before the first frame update
    void Start()
    {
        cc = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        float yStore = moveDirection.y;

        moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
        moveDirection = moveDirection.normalized * moveSpeed;

        // Check for jump
        moveDirection.y = yStore;
        if (cc.isGrounded)
        {
            moveDirection.y = 0f;
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpHeight;
            }
        }

      

        moveDirection.y = moveDirection.y + (gravity * gravitySpeed * Time.deltaTime);

        // Time.deltaTime is the time since the last frame e.g 60fps = 1/60s
        cc.Move(moveDirection * Time.deltaTime);

        //If the player is moving make them face the correct direction
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(0f, Pivot.rotation.eulerAngles.y, 0f);

            // If the player is moving, calculate new rotation
            if (moveDirection != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));

                // Apply the rotation gradually
                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
            }
        }

        // Animation handling
        anim.SetBool("isGrounded", cc.isGrounded);
        anim.SetFloat("Speed", (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))));
    }

}
