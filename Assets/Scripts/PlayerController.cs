using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerController : MonoBehaviour
{
    // Variables related to player character movement
    public InputAction moveAction;
    //public InputAction jumpAction;
    public float moveSpeed = 400.0f;
    public float jumpSpeed = 2.0f;
    public float distance = 1f;
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
    
    // Start is called before the first frame update
    void Start()
    {
        //jumpAction.Enable();
        moveAction.Enable();
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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

        animator.SetFloat("Direction", moveDirection);
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        animator.SetFloat("yVelocity", rigidbody2d.velocity.y);
        animator.SetFloat("WallSlidingDirection", wallSlidingDirection);
        animator.SetBool("Jump", Input.GetButtonDown("Jump") || (!IsGrounded() && !IsWalled()));
        animator.SetBool("WallJump", IsWalled()&&!IsGrounded() );
        
        //if (IsGrounded())
        //{
            //Time.timeScale = 0.0f;
        //}
        if (Input.GetButtonDown("Jump") && (IsGrounded() || IsWalled()))
        {
            Jump();
                    
        }
     
    }
        

    private void FixedUpdate()
    {
        //if (!isWallSliding)
        //{
            rigidbody2d.velocity = new Vector2(horizontal * moveSpeed * Time.deltaTime, rigidbody2d.velocity.y);
        //}
        //rigidbody2d.velocity = new Vector2(horizontal * moveSpeed * Time.deltaTime, rigidbody2d.velocity.y);
        

        WallSide();
        
    }

    private void Jump()
    {
        animator.SetBool("Jump", true);
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpSpeed);
        
    }

    private bool IsGrounded()
    { 
        return Physics2D.OverlapBox(rigidbody2d.position + Vector2.down, new Vector2(0.9f, 0.1f), 0.0f, LayerMask.GetMask("Ground"));
            
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
        return Physics2D.OverlapBox(rigidbody2d.position + new Vector2(0.6f, 0), new Vector2(0.1f, distance - 0.6f), 0.0f, LayerMask.GetMask("Wall"));
    }
    private bool IsLeftWallSliding()
    {
        return Physics2D.OverlapBox(rigidbody2d.position - new Vector2(0.6f, 0), new Vector2(0.1f, distance - 0.6f), 0.0f, LayerMask.GetMask("Wall"));
    }
}
