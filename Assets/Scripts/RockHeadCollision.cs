using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockHeadCollision : MonoBehaviour
{
    [SerializeField] private string animationName;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D playerBody;
    // Start is called before the first frame update
    private bool isColliding;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) StartCoroutine(Animation());
        else
        {
            if(playerBody == null) playerBody = other.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerBody.velocity.magnitude < 0.1f)
            {
                PlayerController playerController = other.GetComponent<PlayerController>();
                playerController.ChangeHealth(-1);
                playerController.Dead();
            }
        };
    }
    IEnumerator Animation()
    {
        isColliding = true;
        animator.SetTrigger(animationName);
        yield return new WaitForSeconds(0.2f);
        isColliding = false;
    }
}
