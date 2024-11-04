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
    public Vector2 velocity;
    public Vector2 startPosition;
    public Vector2 endPosition;
    public bool horizontal  = false;
    private float speedMove = 4;
    private bool canMove = true;
    private int direction = 0;
    void Start()
    {

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
        Debug.Log(direction); 
        
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!horizontal)
            {
                Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
                if (rb.position.y - rb2d.position.y >= 1.05f && Mathf.Abs(rb2d.position.x - rb.position.x) <= 1.5f && Mathf.Abs(rb.velocity.y - rb2d.velocity.y) <= 0.1f)
                {
                    direction = 1;
                    isPlayerOnPlatform = true;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!horizontal)
            {
                Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (Mathf.Abs(rb2d.position.y - rb.position.y) >= 1.2f || Mathf.Abs(rb2d.position.x - rb.position.x) >= 1.5f)
                {
                    direction = -1;
                    isPlayerOnPlatform = false;
                }
            }
        }
    }
}
