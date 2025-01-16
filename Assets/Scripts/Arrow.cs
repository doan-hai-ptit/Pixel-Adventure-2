using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private ResetArrow resetArrow;
    private float resetTimer = 5.0f;
    public float jumpForce = 30f;
    // Start is called before the first frame update
    void Start()
    {
        float startFrame = Random.Range (0.0f, 0.6f);
        animator.Play("Idle",0, startFrame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            GameController.instance.ShakeCamera(1, 0.125f);
            animator.SetBool("Hit", true);
            spriteRenderer.enabled = false;
            boxCollider.enabled = false;
            resetArrow.resetTime = resetTimer;
            //this.gameObject.SetActive(false);
            //GameController.instance.CollectObj(this.gameObject);
        }
    }
}