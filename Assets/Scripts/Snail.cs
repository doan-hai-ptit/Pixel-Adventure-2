using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : Enemy
{
    // Start is called before the first frame update
    [SerializeField] private GameObject wallCollison;
    [SerializeField] private GameObject snailWithoutShell;
    void Start()
    {
        health = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (isChangingDirection)
        {
            Idle();
        }
        else if(!isChangingDirection || !isDead)
        {
            Move();
        }
        //Debug.Log(rb.velocity);
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
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

    protected override void Idle()
    {
        rb.velocity = Vector2.zero;
    }
    
    protected override void ChangeForm()
    {
        if (!isOtherForm)
        {
            isOtherForm = true;
            speedMove = 0;
            wallCollison.transform.position = transform.position + new Vector3(direction * -0.57f, 0, 0);
            StartCoroutine(DelayForAnimation());
            
        }
        else
        {
            animator.SetBool("isShell", true);
            speedMove = 12;
        }
        animator.SetTrigger("Hit");
    }

    IEnumerator DelayForAnimation()
    {
        yield return new WaitForSeconds(0.2f);
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.05f);
        snailWithoutShell.SetActive(true);
        yield return new WaitForSeconds(5f);
        Destroy(snailWithoutShell);
    }
}
