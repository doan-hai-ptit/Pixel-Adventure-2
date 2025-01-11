using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FatBird : Enemy
{
    [SerializeField] private float hoverAmplitude = 5f;  // Biên độ dao động (mức độ lên xuống)
    [SerializeField] private float hoverFrequency = 0.5f; 
    [SerializeField] private bool returnStartPosition = false;
    [SerializeField] private ParticleSystem particle1;
    [SerializeField] private ParticleSystem particle2;
    private Vector2 startPosition;
    private float randomPhaseY;
    private bool isAttacking = false;
    [SerializeField] private float time = 0.45f;
    // Start is called before the first frame update
    void Start()
    {
        //this.enemyName = this.GetType().Name;
        randomPhaseY = Random.Range(0f, Mathf.PI * 2);
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead && !isAttacking)
        {
            if(!returnStartPosition) Idle();
            else
            {
                ReturnStartPosition();
            }
               
        }
        if (!isAttacking && IsPlayerInRangeBotSide(width, height))
        {
            isAttacking = true;
        }

        if (isAttacking)
        {
            Attack();
        }
        if (time > 0 && !isAttacking)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                particle1.Play();
                particle2.Play();
                time = 0.45f;
            }
        }
        animator.SetBool("Attack", isAttacking);
    }
    protected override void Idle()
    {
        float velocityY = Mathf.Sin(Time.time * hoverFrequency + randomPhaseY) * hoverAmplitude;
        rb.velocity = new Vector2(0, velocityY);
    }

    public override void Attack()
    {
        isAttacking = true;
        rb.velocity = new Vector2(0, speedMove);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isAttacking = false;
            time = 0.45f;
            returnStartPosition = true;
            GameController.instance.ShakeCamera(1, 0.125f);
        }
    }

    private void ReturnStartPosition()
    {
        if (!isAttacking && transform.position.y < startPosition.y)
        {
            rb.velocity = new Vector2(0, hoverAmplitude * 2);
        }
        else
        {
            returnStartPosition = false;
        }
    }
}
