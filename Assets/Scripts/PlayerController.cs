using System;
using System.Collections;
using System.Collections.Generic;
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
   
    // Start is called before the first frame update
    void Start()
    {
        jumpAction.Enable();
        moveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        move = moveAction.ReadValue<float>();
        if (jumpAction.IsPressed())
        {
            Jump();
            
        }
        
        Debug.Log(rigidbody2d.velocity);
       
    }

    private void FixedUpdate()
    {
        rigidbody2d.velocity = new Vector2(move * moveSpeed * Time.deltaTime, rigidbody2d.velocity.y);
        //Debug.Log(rigidbody2d.velocity);
    }

    private void Jump()
    {
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpSpeed);
        
    }

    private bool IsGrounded()
    {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position, Vector2.down, distance, LayerMask.GetMask("Ground"));
            return hit.collider != null;
    }
}
