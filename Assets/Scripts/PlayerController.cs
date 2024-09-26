using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerController : MonoBehaviour
{
    // Variables related to player character movement
    //public InputAction moveAction;
    public float moveSpeed = 400.0f;
    public float jumpSpeed = 26.0f;
    public float doubleJumpSpeed = 50.0f;
    public float distance = 1f;
    private bool doubleJump = false;
    private bool hasDoubleJump = false;
    private bool hasWallJump = false;
    Rigidbody2D rigidbody2d;
    //float move;
    private float horizontal;
    
    // Variables related to checking isground
    public Transform groundCheck;
    
    // Variables related to checking isWalled
    public Transform wallCheck;
    private bool isWallSliding = false;
    private float wallSlidingDirection = 0;
    private float wallSlidingSpeed = 2f;
    
    // Variables related to animation
    Animator animator;
    float moveDirection = 1f;
    
    // Variables related to testing
    public bool isgrounded;
    public bool iswalled;
    
    // Start is called before the first frame update
    void Start()
    {
        //jumpAction.Enable();
        //moveAction.Enable();
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;
    }

    // Update is called once per frame
    void Update()
    {
        isgrounded = IsGrounded();
        iswalled = IsWalled();
        horizontal = Input.GetAxisRaw("Horizontal");
        //move = moveAction.ReadValue<float>();
        if (!Mathf.Approximately(horizontal, 0.0f))
        {
           moveDirection = horizontal;

        }
        if (IsLeftWallSliding())
        {
            wallSlidingDirection = -1.0f;
        }
        else if (IsRightWallSliding())
        {
            wallSlidingDirection = 1.0f;
        }

        if ((IsGrounded() || IsWalled()) && rigidbody2d.velocity.y <= 0)
        {
            doubleJump = false;
            hasDoubleJump = false;
            hasWallJump = false;
        }
        else if(!hasDoubleJump)
        {
            doubleJump = true;
        }
        if (Input.GetButtonDown("Jump"))
        {
            if (!IsWalled())
            {
                if (IsGrounded() || doubleJump)
                {
                    if(doubleJump) hasDoubleJump = true;
                    doubleJump = !doubleJump;
                    Jump();
                }
            }
            else
            {
                doubleJump = !doubleJump;
                WallJump();
            }
        }
        //Debug.Log(rigidbody2d.velocity);
        
        //Set animations
        animator.SetFloat("Direction", moveDirection);
        animator.SetFloat("Speed", Mathf.Abs(rigidbody2d.velocity.x));
        animator.SetFloat("yVelocity", rigidbody2d.velocity.y);
        animator.SetFloat("WallSlidingDirection", wallSlidingDirection);
        animator.SetBool("WallJump", IsWalled()&&!IsGrounded() );
        animator.SetBool("Jump", Input.GetButtonDown("Jump") || (!IsGrounded()));
        animator.SetBool("DoubleJump", doubleJump);
        
     
    }
        

    private void FixedUpdate()
    {
        if (!isWallSliding || (moveDirection != wallSlidingDirection && !hasWallJump)) // loi wallJump o day
        {
            
            rigidbody2d.velocity = new Vector2(horizontal * moveSpeed * Time.deltaTime, rigidbody2d.velocity.y);
        }
        // rigidbody2d.velocity = new Vector2(horizontal * moveSpeed * Time.deltaTime, rigidbody2d.velocity.y);
        
        
        WallSide();
    }

    private void Jump()
    {
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpSpeed);
    }
    private void WallJump()
    {   
        hasWallJump = true;
        rigidbody2d.velocity = new Vector2(-20.0f * wallSlidingDirection, jumpSpeed);
        moveDirection = -wallSlidingDirection;
    }

    private void DoubleJump()
    {
        //animator.SetTrigger("DoubleJump");
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, doubleJumpSpeed);
    }
    private bool IsGrounded()
    { 
        return Physics2D.OverlapBox(rigidbody2d.position + Vector2.down, new Vector2(0.9f, 0.1f), 0.0f, LayerMask.GetMask("Ground")) || Physics2D.OverlapBox(rigidbody2d.position + Vector2.down, new Vector2(1.0f, 0.1f), 0.0f, LayerMask.GetMask("Platform"));
            
    }

    private bool IsWalled()
    {
        return IsLeftWallSliding() || IsRightWallSliding();
    }

    private void WallSide()
    {
        if (IsWalled() && !IsGrounded() && moveDirection != 0.0f)
        {
            isWallSliding = true;
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, Mathf.Clamp(rigidbody2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }
    private bool IsRightWallSliding()
    {
        return Physics2D.OverlapBox(rigidbody2d.position + new Vector2(0.6f, 0), new Vector2(0.05f, distance), 0.0f, LayerMask.GetMask("Wall"));
    }
    private bool IsLeftWallSliding()
    {
        return Physics2D.OverlapBox(rigidbody2d.position - new Vector2(0.6f, 0), new Vector2(0.05f, distance), 0.0f, LayerMask.GetMask("Wall"));
    }

    
    
}
