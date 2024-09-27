using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineController : MonoBehaviour
{
    Animator animator;
    public float jumpForce = 43.5f;
    public float jumpTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(0, jumpForce - rb.velocity.y), ForceMode2D.Impulse);
        
        //Debug.Log(rb.velocity.y);
        animator.SetBool("Jump", true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        animator.SetBool("Jump", false);
    }
}
