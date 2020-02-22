using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    private float movementInputDirection;

    private Rigidbody2D rb;

    private Animator anim;

    public float moveSpeed;

    public float jumpForce;

    public Transform groundCheck;

    private bool isGrounded;

    public float groundCheckRadius;

    public LayerMask whatIsGround;

    private bool canJump;

    private bool isWalking;

    private bool isFacingRight = true;
      

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
        CheckIfCanJump();
        CheckMovementDirection();
    }

    private void FixedUpdate()
    {
        applyMovement();
        CheckSurroundings();
        UpdateAnimations();
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
        if (isGrounded && rb.velocity.y <= 0.01)
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

    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
    }

    private void CheckMovementDirection()
    {
        if(isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        if (rb.velocity.x != 0 && isGrounded)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

}
