using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public float speedMove;
    public bool isDead = false;
    public virtual void Idle()
    {
        Debug.Log("Idle");
    }

    public virtual void Attack()
    {
        
    }

    public virtual void Hit()
    {
        StartCoroutine(Death());
    }

    public bool IsPlayerInRange()
    {
        return Physics2D.OverlapCircle(transform.position, 0.2f, LayerMask.GetMask("Player"));
    }

    IEnumerator Death()
    {
        isDead = true;
        SceneController.instance.ShakeCamera(5, 0.125f);
        animator.SetTrigger("Hit");
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.1f);
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.enabled = false;
        //rb.AddForce(Vector2.up * 26f, ForceMode2D.Impulse);
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 10;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
