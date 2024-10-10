using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // Variables related to player character movement
    //public InputAction moveAction;
    public float moveSpeed = 400.0f;
    public float jumpSpeed = 26.0f;
    public float doubleJumpSpeed = 50.0f;
    public float distance = 1f;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask platformLayer;
    private bool doubleJump = false;
    private bool hasDoubleJump = false;
    private bool hasWallJump = false;
    Rigidbody2D rigidbody2d;
    
    //float move;
    private float horizontal;
    private bool isWallSliding = false;
    private float wallSlidingDirection = 0;
    private float wallSlidingSpeed = 1f;
    
    // Variables related to animation
    Animator animator;
    float moveDirection = 1f;
    
    // Variables related to testing
    public bool isground;
    public bool iswall;
    public float xVelocity = -2;
    public float yVelocity = 20;
    
    // Variables related to hitting
    public bool isDead {set; get;}
    private Vector2 startPosition;
    
    //Variables related to changing scene
    public int numberOfFruitsMax = 100;
    private int numberOfFruits;
    
    // Start is called before the first frame update
    void Start()
    {
        //jumpAction.Enable();
        //moveAction.Enable();
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        isDead = false;
        startPosition = transform.position;
        numberOfFruits = numberOfFruitsMax;
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;
    }

    // Update is called once per frame
    void Update()
    {
        isground = IsGrounded();
        iswall = IsWalled();
        horizontal = Input.GetAxisRaw("Horizontal");
        //move = moveAction.ReadValue<float>();
        
        //Flip player when moving left-right
        if (horizontal > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontal < -0.01)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        
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
        else if(!hasDoubleJump && !IsGrounded() && !IsWalled())
        {
            doubleJump = true;
        }

        if (!isDead)
        {
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
        }
        
        
        //Set animations
        //animator.SetFloat("Direction", moveDirection);
        animator.SetFloat("Speed", Mathf.Abs(rigidbody2d.velocity.x));
        animator.SetFloat("yVelocity", rigidbody2d.velocity.y);
        //animator.SetFloat("WallSlidingDirection", wallSlidingDirection);
        animator.SetBool("WallJump", IsWalled()&& !IsGrounded());
        //animator.SetBool("Hit", isDead);
        animator.SetBool("Jump", !IsGrounded() && !isDead); // loi thi them input.getbuttondown(jump);
        animator.SetBool("DoubleJump", doubleJump);
    }
        

    private void FixedUpdate()
    {
        if (!isDead)
        {
            if (!isWallSliding || (moveDirection != wallSlidingDirection && !hasWallJump)) // loi wallJump o day
            {
                
                rigidbody2d.velocity = new Vector2(horizontal * moveSpeed, rigidbody2d.velocity.y);
            }
            WallSide();
        }
        else
        {
            if (rigidbody2d.velocity.y < 0)
            {
                rigidbody2d.velocity = new Vector2(moveDirection * -1, Math.Clamp(rigidbody2d.velocity.y, -25f, jumpSpeed));
            }
        }
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
    
    private bool IsGrounded()
    { 
        //return Physics2D.OverlapBox(rigidbody2d.position + Vector2.down, new Vector2(0.9f, 0.1f), 0.0f, LayerMask.GetMask("Ground")) || Physics2D.OverlapBox(rigidbody2d.position + Vector2.down, new Vector2(1.0f, 0.1f), 0.0f, LayerMask.GetMask("Platform")) || Physics2D.OverlapBox(rigidbody2d.position + Vector2.down, new Vector2(1.0f, 0.1f), 0.0f, LayerMask.GetMask("DeadZone"));
        return Physics2D.OverlapBox(rigidbody2d.position + Vector2.down, new Vector2(0.9f, 0.1f), 0.0f, groundLayer);
    }

    private bool IsWalled()
    {
        //return Physics2D.OverlapBox(rigidbody2d.position + new Vector2(0.6f, -0.22f), new Vector2(0.05f, distance), 0.0f, LayerMask.GetMask("Wall")) || Physics2D.OverlapBox(rigidbody2d.position + new Vector2(0.6f, -0.22f), new Vector2(0.05f, distance), 0.0f, LayerMask.GetMask("DeadZone"));
        return IsLeftWallSliding() || IsRightWallSliding();
    }

    private void WallSide()
    {
        if (IsWalled() && !IsGrounded())
        {
            isWallSliding = true;
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, Mathf.Clamp(rigidbody2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
            if (wallSlidingDirection > 0.01f)
            {
                transform.localScale = Vector3.one;
            }
            else if (wallSlidingDirection < -0.01)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            isWallSliding = false;
        }
    }
    private bool IsRightWallSliding()
    {
        return Physics2D.OverlapBox(rigidbody2d.position + new Vector2(0.6f, -0.2f), new Vector2(0.05f, distance), 0.0f, wallLayer);
    }
    private bool IsLeftWallSliding()
    {
        return Physics2D.OverlapBox(rigidbody2d.position - new Vector2(0.6f, 0.2f), new Vector2(0.05f, distance), 0.0f, wallLayer);
    }

    public void Dead()
    {
        isDead = true;
        animator.SetTrigger("Hit");
        rigidbody2d.AddForce(new Vector2(rigidbody2d.velocity.x * xVelocity, yVelocity - rigidbody2d.velocity.y), ForceMode2D.Impulse);
        BoxCollider2D collider2D = GetComponent<BoxCollider2D>();
        collider2D.enabled = false;
        StartCoroutine(Respawn(2.0f));
    }

    IEnumerator Respawn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        BoxCollider2D collider2D = GetComponent<BoxCollider2D>();
        animator.Play("Idle");
        isDead = false;
        collider2D.enabled = true;
        transform.position = startPosition;
    }

    public void CollectedFruit()
    {
        numberOfFruits--;
        ChangeScene();
    }

    private void ChangeScene()
    {
        if (numberOfFruits <= 0)
        {
            SceneController.instance.NextScene();
        }

        //numberOfFruits++;
    }
    
}
