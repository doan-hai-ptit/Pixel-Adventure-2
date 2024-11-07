using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bat : Enemy
{
    Rigidbody2D playerRigidBody2D;
    // Start is called before the first frame update
    [SerializeField] private bool isSleeping = false;
    [SerializeField] private float cellingOutTime = 0.5f;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead && !isSleeping && IsPlayerInRange(radius))
        {
            if (cellingOutTime > 0)
            {
                animator.SetBool("Attack", true);
                cellingOutTime -= Time.deltaTime;
            }
            else Attack();
        }
        else if ((!isDead && isSleeping))
        {
            Move();
        }
    }

    public override void Move()
    {
        rb.velocity = new Vector2(0, speedMove);
    }

    public override void Attack()
    {
        if(playerRigidBody2D == null) playerRigidBody2D = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        Vector2 dir = (playerRigidBody2D.transform.position - this.transform.position).normalized;
        if (dir.x > 0) this.direction = 1;
        else this.direction = -1;
        this.transform.localScale = new Vector3(direction * -1, 1, 1);
        rb.velocity = dir * speedMove;
    }


    public override void Hit()
    {
        if (!isDead)
        {
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            if (!player.isDead)
            {
                player.ChangeHealth(-1);
                player.Dead();
            }
            this.isSleeping = true;
            this.playerRigidBody2D = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (this.isSleeping && other.gameObject.CompareTag("Ground"))
        {
            this.isSleeping = false;
            this.cellingOutTime = 0.5f;
            animator.SetBool("Attack", false);
        }
    }

    //IEnumerator CellingOut()
    //{
    //    
    //    
    //    this.isCellingOut = false;
    //    yield return new WaitForSeconds(0.35f);
    //}
}