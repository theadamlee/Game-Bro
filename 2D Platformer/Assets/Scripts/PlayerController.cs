using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    public Transform groundCheck;
    public Transform wallCheck;
    private Animator anim;

    private float movementInputDirection;
    public float moveSpeed;  
    public float groundCheckRadius;
    public float jumpForce;
    public float wallCheckDistance;
    public float wallSlideSpeed;

    public int amountOfJumps;
    private int amountOfJumpsLeft;
    private int facingDirection = 1; 

    private bool isGrounded;
    private bool canJump;
    private bool isWalking;
    private bool isFacingRight = true;
    private bool isWallSliding;
    private bool isTouchingWall;
    private bool isPushing;

    public LayerMask whatIsGround;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
        CheckIfCanJump();
        CheckMovementDirection();
        CheckIfWallSliding();
    }

    private void FixedUpdate()
    {
        applyMovement();
        CheckSurroundings();
        UpdateAnimations();
    }

    private void CheckIfWallSliding()
    {
        
        if (isTouchingWall && !isGrounded && rb.velocity.y <= 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }

    }


    private void CheckIfPushing()
    {

        if (isTouchingWall && movementInputDirection != 0)
        {
            isPushing = true;
        }


    }


    private void checkInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void CheckIfCanJump()
    {
        if ((isGrounded && rb.velocity.y <= 0.01) || isWallSliding)
        {
            amountOfJumpsLeft = amountOfJumps;
        }
        if(amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }

    private void Jump()
    {
        if(canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
        }
             
    }

    private void applyMovement()
    {

      
       
        rb.velocity = new Vector2(moveSpeed * movementInputDirection, rb.velocity.y);
        
              
        if (isWallSliding)
        {
            if(rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
    }

    private void CheckSurroundings()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);

        anim.SetFloat("SpeedY", rb.velocity.y);

        anim.SetBool("isGrounded", isGrounded);

        anim.SetBool("isWallSliding", isWallSliding);

        anim.SetBool("isPushing", isPushing);
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

        if (rb.velocity.x < -0.1 && isGrounded || rb.velocity.x > 0.1 && isGrounded)
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
        if (!isWallSliding)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0f);
        }  
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }

}
