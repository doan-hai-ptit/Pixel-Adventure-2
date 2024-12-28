using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Enemy
{
    Rigidbody2D playerRigidBody2D;
    private bool run = true;
    // Start is called before the first frame update
    void Start()
    {
        direction = (int)transform.localScale.x * -1;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        animator.SetBool("Run", run && !isDead);
    }
    public override void Move()
    {
        if(isDead) rb.velocity = new Vector2(0, rb.velocity.y);
        else
        {
            transform.localScale = new Vector3(direction * -1, 1, 1);
            rb.velocity = new Vector2(speedMove * direction, rb.velocity.y);
        }
    }

    public override void Attack()
    {
        if(playerRigidBody2D == null) playerRigidBody2D = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        if (playerRigidBody2D.transform.position.x - this.transform.position.x > 0) this.direction = 1;
        else this.direction = -1;
        this.transform.localScale = new Vector3(direction * -1, 1, 1);
        rb.velocity = new Vector2( direction * speedMove, 0);
    }
    
}