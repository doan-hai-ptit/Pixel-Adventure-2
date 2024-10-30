using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bee : Enemy
{
    [SerializeField] private float hoverAmplitude = 5f;  // Biên độ dao động (mức độ lên xuống)
    [SerializeField] private float hoverFrequency = 0.5f; 
    private float randomPhaseY;
    private float randomPhaseX;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        randomPhaseY = Random.Range(0f, Mathf.PI * 2);
        randomPhaseX = Random.Range(0f, Mathf.PI * 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Idle();
            
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * 26f, ForceMode2D.Impulse);
            Hit();
        }
    }

    public override void Idle()
    {
        float velocityY = Mathf.Sin(Time.time * hoverFrequency + randomPhaseY) * hoverAmplitude;
        float velocityX = Mathf.Cos(Time.time * hoverFrequency + randomPhaseX) * hoverAmplitude;
        rb.velocity = new Vector2(velocityX, velocityY);
    }
}
