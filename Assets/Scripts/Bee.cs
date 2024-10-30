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
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 0.4f;
    private Rigidbody2D rbPlayer;
    private float randomPhaseY;
    private float randomPhaseX;
    private bool collided = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rbPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        randomPhaseY = Random.Range(0f, Mathf.PI * 2);
        randomPhaseX = Random.Range(0f, Mathf.PI * 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsHitted() && !isDead)
        {
            rbPlayer.velocity = Vector2.zero;
            rbPlayer.AddForce(Vector2.up * 26f, ForceMode2D.Impulse);
            Hit();
        }
        else if(!collided && CollidedWithPlayer()){
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            if(!player.isDead) player.Dead();
            collided = true;
            
        }
        if (!isDead)
        {
            Idle();
            
        }
        
        if (IsPlayerInRange(radius))
        {
            Attack();
        }
    }

    private bool IsHitted()
    {
        return Physics2D.OverlapBox(transform.position + new Vector3(0, 0.65f, 0), new Vector2(1.3f, 0.45f), 0.0f, LayerMask.GetMask("Player"));
    }

    private bool CollidedWithPlayer()
    {
        return Physics2D.OverlapBox(transform.position + new Vector3(0, -0.3f, 0), new Vector2(1.3f, 1.5f), 0.0f, LayerMask.GetMask("Player"));
    }
    
    public override void Idle()
    {
        float velocityY = Mathf.Sin(Time.time * hoverFrequency + randomPhaseY) * hoverAmplitude;
        float velocityX = Mathf.Cos(Time.time * hoverFrequency + randomPhaseX) * hoverAmplitude;
        rb.velocity = new Vector2(velocityX, velocityY);
    }

    public override void Attack()
    {
        if (fireRate > 0)
        {
            fireRate -= Time.deltaTime;
            if (fireRate <= 0)
            {
                StartCoroutine(Launch());
            }
        }
    }

    IEnumerator Launch()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.4f);
        Instantiate(bulletPrefab, transform.position + new Vector3(0, -0.687f, 0), Quaternion.identity);
        fireRate = 0.4f;
    }
    
}
