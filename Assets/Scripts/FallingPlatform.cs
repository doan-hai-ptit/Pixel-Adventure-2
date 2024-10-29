using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private bool isOn = true;
    [SerializeField] private float timeToFall = 1.2f;
    [SerializeField] private bool isFalling = false;
    [SerializeField] private float hoverAmplitude = 0.3f;  // Biên độ dao động (mức độ lên xuống)
    [SerializeField] private float hoverFrequency = 2f;  
    [SerializeField] private float hoverAmplitudeWhenFall = 2f;  // Biên độ dao động (mức độ lên xuống)
    [SerializeField] private float hoverFrequencyWhenFall = 8f;  
    [SerializeField] private GameObject airEffect;
    Animator animator;
    Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        //rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("On", isOn);
        if (isFalling)
        {
            if (timeToFall > 0)
            {
                PrepareToFall();
                timeToFall -= Time.deltaTime;
                if (timeToFall <= 0)
                {
                    
                    StartCoroutine(Falling());
                }
            }
        }
        else
        {
            Swing();
        }
    }

    IEnumerator Falling()
    {
        airEffect.SetActive(false);
        isOn = false;
        yield return new WaitForSeconds(0.25f);
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        rb2d.gravityScale = 10;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            if (Mathf.Abs(rb.velocity.y - rb2d.velocity.y) < 0.1)
            {
                isFalling = true;
            }
        }
    }

    private void Swing()
    {
        float velocityY = Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        float velocityX = Mathf.Cos(Time.time * hoverFrequency) * hoverAmplitude;
        rb2d.velocity = new Vector2(velocityX, velocityY);
    }

    private void PrepareToFall()
    {
        float velocityY = Mathf.Sin(Time.time * hoverFrequencyWhenFall) * hoverAmplitudeWhenFall;
        rb2d.velocity = new Vector2(0, velocityY);
    }
}
