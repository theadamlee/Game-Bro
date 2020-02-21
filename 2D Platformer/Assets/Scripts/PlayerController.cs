using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    private float movementInputDirection;

    Rigidbody2D rb;

    public float moveSpeed;

    public float jumpForce;

    public Transform groundCheck;

    private bool isGrounded;

    public float groundCheckRadius;

    public LayerMask whatIsGround;

    private bool canJump;
      

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
        CheckIfCanJump();
    }

    private void FixedUpdate()
    {
        applyMovement();
        CheckSurroundings();
    }

    private void checkInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey("w") || Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void CheckIfCanJump()
    {
        if (isGrounded)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    }

    private void Jump()
    {
        if(canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
       
        
    }

    private void applyMovement()
    {
        rb.velocity = new Vector2(moveSpeed * movementInputDirection, rb.velocity.y);
    }

    private void CheckSurroundings()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

}
