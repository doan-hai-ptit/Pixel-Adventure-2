using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private bool isPlayerOnPlatform = false;
    [SerializeField] private Animator animator;
    [SerializeField] private float speedMove = 100;
    public bool L2R = true;
    private Rigidbody2D rigidbody2dPlayer;
    public Vector2 velocity;
    public Vector2 startPosition;
    public Vector2 endPosition;
    public bool horizontal  = false;
    private Vector3 previousPosition;
    private int direction = 0;
    void Start()
    {
        if (horizontal)
        {
            animator.SetBool("On", true);
            direction = 1;
            
        }
        else direction = 0;
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = velocity * direction;
        if (!horizontal)
        {
            if (transform.position.y <= startPosition.y)
            {
                if (isPlayerOnPlatform)
                {
                    direction = 1;
                    animator.SetBool("On", true);
                }
                else
                {
                    direction = 0;
                    animator.SetBool("On", false);
                }
            }
            else if (transform.position.y >= endPosition.y)
            {
                if (isPlayerOnPlatform)
                {
                    direction = 0;
                    animator.SetBool("On", false);
                }
                else
                {
                    direction = -1;
                    animator.SetBool("On", true);
                }
            }
        }
        else
        {
            if (L2R)
            {
                if (transform.position.x <= startPosition.x)
                {
                    StartCoroutine(ChangeDirection());
                }
                else if (transform.position.x >= endPosition.x)
                {
                    StartCoroutine(ChangeDirection());
                }
            }
            else
            {
                if (transform.position.x >= startPosition.x)
                {
                    StartCoroutine(ChangeDirection());
                }
                else if (transform.position.x <= endPosition.x)
                {
                    StartCoroutine(ChangeDirection());
                }
            }
            if (isPlayerOnPlatform)
            {
                Vector2 move = new Vector2(speedMove * Time.fixedDeltaTime, 0) * direction; 
                if (rigidbody2dPlayer.velocity.magnitude < 1.0f) rigidbody2dPlayer.MovePosition(rigidbody2dPlayer.position + move);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(rigidbody2dPlayer == null) rigidbody2dPlayer = other.gameObject.GetComponent<Rigidbody2D>();
            //Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            if (rigidbody2dPlayer.position.y - rb2d.position.y >= 1.05f && Mathf.Abs(rb2d.position.x - rigidbody2dPlayer.position.x) <= 1.5f && Mathf.Abs(rigidbody2dPlayer.velocity.y - rb2d.velocity.y) <= 0.1f)
            {
                isPlayerOnPlatform = true;
                if (!horizontal)
                {
                    direction = 1;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (Mathf.Abs(rb2d.position.y - rigidbody2dPlayer.position.y) >= 1.2f || Mathf.Abs(rb2d.position.x - rigidbody2dPlayer.position.x) >= 1.5f)
            {
                isPlayerOnPlatform = false;
                if (!horizontal)
                {
                    direction = -1;
                }
            }
        }
    }

    IEnumerator ChangeDirection()
    {
        direction = 0;
        animator.SetBool("On", false);
        yield return new WaitForSeconds(0.4f);
        animator.SetBool("On", true);
        if (L2R)
        {
            if (transform.position.x <= startPosition.x + 0.5f)
            {
                direction = 1;
            }
            else if (transform.position.x >= endPosition.x-0.5f)
            {
                direction = -1;
            }
        }
        else
        {
            if (transform.position.x >= startPosition.x - 0.5f)
            {
                direction = -1;
            }
            else if (transform.position.x <= endPosition.x+0.5f)
            {
                direction = 1;
            }
        }
    }
}
