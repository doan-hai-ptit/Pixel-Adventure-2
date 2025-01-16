using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : Enemy
{
    // Start is called before the first frame update
    [SerializeField] private float jumpSpeed = 26.0f;
    [SerializeField] private float jumpDelay = 0.2f;
    [SerializeField] private float jumpTime = 2.0f;
    [SerializeField] private bool canJump = true;
    void Start()
    {
        direction = (int)transform.localScale.x * -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (jumpTime > 0)
            {
                jumpTime -= Time.deltaTime;
                if (jumpTime <= 0)
                {
                    canJump = true;
                    jumpTime = 2.0f;
                }
            }
            if (canJump && jumpDelay > 0)
            {
                jumpDelay -= Time.deltaTime;
                if (jumpDelay <= 0)
                {
                    rb.velocity = new Vector2(speedMove * direction, jumpSpeed);
                    jumpDelay = 0.2f;
                    canJump = false;
                }
            } 
            animator.SetBool("Jump", canJump);
            animator.SetFloat("Yvelocity", rb.velocity.y);
            transform.localScale = new Vector3(direction * -1, 1, 1);
        }
    }
    private bool IsGrounded()
    { 
        return Physics2D.OverlapBox(rb.position + Vector2.down, new Vector2(0.9f, 0.25f), 0.0f, LayerMask.GetMask("Ground"));
    }
}
