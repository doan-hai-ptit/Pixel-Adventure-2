using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : Enemy
{
    // Start is called before the first frame update
    [SerializeField] private GameObject wallCollison;
    void Start()
    {
        health = 3;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isChangingDirection ||isDead)
        {
            Idle();
        }
        else
        {
            Move();
        }
        //Debug.Log(rb.velocity);
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }

    public override void Move()
    {
        transform.localScale = new Vector3(direction * -1, 1, 1);
        rb.velocity = new Vector2(speedMove * direction, rb.velocity.y);
    }

    protected override void Idle()
    {
        rb.velocity = Vector2.zero;
    }
    public override void Hit()
    {
        Debug.Log("Snail Hit");
    }
    
    protected override void ChangeForm()
    {
        if (!isOtherForm)
        {
            isOtherForm = true;
            speedMove = 0;
            wallCollison.transform.position = transform.position + new Vector3(direction * -0.57f, 0, 0);
        }
        else
        {
            animator.SetBool("isShell", true);
            speedMove = 18;
        }
        animator.SetTrigger("Hit");
    }
}
