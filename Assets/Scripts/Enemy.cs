using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public float speedMove;
    public bool isDead = false;
    public float radius;
    public int direction = 1;
    public bool isChangingDirection = false;
    public int health = 1;
    public bool isOtherForm = false;
    protected virtual void Idle()
    {
        Debug.Log("Idle");
    }

    public virtual void Move()
    {
        Debug.Log("Move");
    }
    
    public virtual void Attack()
    {
        Debug.Log("Attack");
    }

    public virtual void Hitted()
    {
        Rigidbody2D rbPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        rbPlayer.velocity = Vector2.zero;
        rbPlayer.AddForce(Vector2.up * 26f, ForceMode2D.Impulse);
        health--;
        if (health <= 0)
        {
            StartCoroutine(Death());
        }
        else
        {
            ChangeForm();
        }
    }
    protected virtual void ChangeForm()
    {
        Debug.Log("ChangeForm");
    }
    public virtual void Hit()
    {
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (!player.isDead)
        {
            player.ChangeHealth(-1);
            player.Dead();
        }
    }

    public IEnumerator ChangeDirection()
    {
        if (!isOtherForm)
        {
            isChangingDirection = true;
            yield return new WaitForSeconds(1f);
            direction *= -1;
            isChangingDirection = false;
        }
        else
        {
            animator.SetTrigger("WallHit");
            SceneController.instance.ShakeCamera(1.5f, 0.125f);
            direction *= -1;
        }
    }
    protected bool IsPlayerInRange(float range)
    {
        return Physics2D.OverlapCircle(transform.position, range, LayerMask.GetMask("Player"));
    }
    
    IEnumerator Death()
    {
        isDead = true;
        SceneController.instance.ShakeCamera(5, 0.125f);
        animator.SetTrigger("Dead");
        rb.velocity = Vector2.zero;
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.enabled = false;
        yield return new WaitForSeconds(0.1f);
        rb.AddForce(Vector2.up * 26f, ForceMode2D.Impulse);
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 10;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    
}
