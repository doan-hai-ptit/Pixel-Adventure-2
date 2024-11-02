using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb2d;
    public Vector2 velocity;
    public Vector2 startPosition;
    public Vector2 endPosition;
    private float speedMove = 4;
    private bool canMove = true;
    private Animator animator;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (velocity.y > 0)
        {
            if (transform.position.y < startPosition.y || transform.position.y > endPosition.y)
            {
                //canMove = false;
                //transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
                rb2d.velocity = Vector2.zero;
                animator.SetBool("On", false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (canMove)
            {
                //canMove = true
                rb2d.velocity = velocity;
                animator.SetBool("On", true);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (canMove)
            {
                rb2d.velocity = velocity * -1;
                Debug.Log(velocity * -1);
                animator.SetBool("On", true);
            }
        }
    }
}
