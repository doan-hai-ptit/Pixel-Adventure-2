using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public new Renderer renderer;
    public string enemyName;
    public float speedMove;
    public bool isDead = false;
    public float radius;
    public float width;
    public float height;
    public int direction = 1;
    public bool isChangingDirection = false;
    public int health = 1;
    public bool isOtherForm = false;
    
    protected virtual void Awake() // Dùng virtual để lớp con có thể override nếu cần.
    {
        enemyName = this.GetType().Name;
    }
    
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
        if (!isDead)
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
    }
    protected virtual void ChangeForm()
    {
        Debug.Log("ChangeForm");
    }
    public virtual void Hit()
    {
        if (!isDead)
        {
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            if (!player.isDead)
            {
                player.ChangeHealth(-1);
                player.Dead();
            }
        }
    }

    public virtual IEnumerator ChangeDirection(float secondsDuration)
    {
        if (!isOtherForm)
        {
            isChangingDirection = true;
            yield return new WaitForSeconds(secondsDuration);
            direction *= -1;
            isChangingDirection = false;
        }
        else
        {
            animator.SetTrigger("WallHit");
            if(IsInCameraView()) GameController.instance.ShakeCamera(1.5f, 0.125f);
            direction *= -1;
        }
    }
    protected bool IsPlayerInRange(float range)
    {
        return Physics2D.OverlapCircle(transform.position, range, LayerMask.GetMask("Player"));
    }

    protected bool IsPlayerInRangeOneSide (float width, float height)
    {
        return Physics2D.OverlapBox(transform.position + new Vector3(width/2 * direction, 0, 0), new Vector2(width, height), 0,LayerMask.GetMask("Player"));
    }
    protected bool IsPlayerInRangeBotSide (float width, float height)
    {
        return Physics2D.OverlapBox(transform.position - new Vector3(0, height/2, 0), new Vector2(width, height), 0,LayerMask.GetMask("Player"));
    }
    protected bool IsPlayerInRangeTwoSide (float width, float height)
    {
        return Physics2D.OverlapBox(transform.position, new Vector2(width, height), 0,LayerMask.GetMask("Player"));
    }
    
    protected bool IsInCameraView()
    {
        return renderer.isVisible;
    }
    
    IEnumerator Death()
    {
        isDead = true;
        GameController.instance.UpdateEnemysList(this.enemyName);
        GameController.instance.ShakeCamera(5, 0.125f);
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Dead");
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.enabled = false;
        yield return new WaitForSeconds(0.1f);
        rb.AddForce(Vector2.up * 26f, ForceMode2D.Impulse);
        rb.gravityScale = 10;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    
}
