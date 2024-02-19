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
    public Animator anim;

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
        // Animation handling
        anim.SetBool("isGrounded", cc.isGrounded);
        anim.SetFloat("Speed", (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))));

        float yStore = moveDirection.y;

        moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
        moveDirection = moveDirection.normalized * moveSpeed;

        moveDirection.y = yStore;


        // Check for jump
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

        // Animation handling
        anim.SetBool("isGrounded", cc.isGrounded);
        anim.SetFloat("Speed", (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))));
    }

}
