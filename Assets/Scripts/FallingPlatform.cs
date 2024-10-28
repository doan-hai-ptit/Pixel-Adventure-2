using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    //[SerializeField] private bool isOn = true;
    [SerializeField] private float timeToFall = 1.2f;
    [SerializeField] private bool isFalling = false;
    Animator animator;
    Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("On", true);
        if (isFalling)
        {
            if (timeToFall > 0)
            {
                timeToFall -= Time.deltaTime;
                if (timeToFall <= 0)
                {
                    animator.SetBool("On", false);// chua set dc
                    StartCoroutine(Falling());
                }
            }
        }
    }

    IEnumerator Falling()
    {
        yield return new WaitForSeconds(1f);
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        rb2d.gravityScale = 5;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            if (Mathf.Abs(rb.velocity.y) < 0.1)
            {
                isFalling = true;
            }
        }
    }
    
}
