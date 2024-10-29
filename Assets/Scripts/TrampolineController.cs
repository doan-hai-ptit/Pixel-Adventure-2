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
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        SceneController.instance.ShakeCamera(3, 0.125f);
        //Debug.Log(rb.velocity.y);
        animator.SetBool("Jump", true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        animator.SetBool("Jump", false);
    }
}
