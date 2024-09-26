using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineController : MonoBehaviour
{
    Animator animator;
    public float jumpForce = 10f;
    
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
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        animator.SetBool("Jump", true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        animator.SetBool("Jump", false);
    }
}
