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
    public InputAction jumpAction;
    public float moveSpeed = 400.0f;
    public float jumpSpeed = 2.0f;
    public float distance = 1f;
    Rigidbody2D rigidbody2d;
    float move;
    //private float horizontal;
    
    // Variables related to checking isground
    public Transform groundCheck;
    
    // Variables related to checking isWalled
    public Transform wallCheck;
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;
    
    // Variables related to animation
    Animator animator;
    float moveDirection = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        jumpAction.Enable();
        moveAction.Enable();
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //horizontal = Input.GetAxisRaw("Horizontal");
        move = moveAction.ReadValue<float>();
        if (!Mathf.Approximately(move, 0.0f))
        {
           moveDirection = move;

        }

        animator.SetFloat("Direction", moveDirection);
        animator.SetFloat("Speed", Mathf.Abs(move));
        animator.SetFloat("yVelocity", rigidbody2d.velocity.y);
        animator.SetBool("Jump", !IsGrounded());
        //if (IsGrounded())
        //{
            //Time.timeScale = 0.0f;
        //}
     
    }

    private void FixedUpdate()
    {
        rigidbody2d.velocity = new Vector2(move * moveSpeed * Time.deltaTime, rigidbody2d.velocity.y);
        //rigidbody2d.velocity = new Vector2(horizontal * moveSpeed * Time.deltaTime, rigidbody2d.velocity.y);
        if (jumpAction.IsPressed() && IsGrounded())
        {
            Jump();
                    
        }
        WallSide();
        Debug.Log(Vector2.down);
    }

    private void Jump()
    {
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpSpeed);
        
    }

    private bool IsGrounded()
    {
           return Physics2D.OverlapBox(rigidbody2d.position + Vector2.down, new Vector2(distance, 0.1f), 0.0f, LayerMask.GetMask("Ground"));
            
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(rigidbody2d.position, distance, LayerMask.GetMask("Wall"));
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
}
