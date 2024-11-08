using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockHeadCollision : MonoBehaviour
{
    [SerializeField] private string animationName;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController player;
    [SerializeField] private bool isCollidingWithPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            if (isCollidingWithPlayer)
            {
                player.ChangeHealth(-1);
                player.Dead();
                isCollidingWithPlayer = false;
            }
            StartCoroutine(Animation());
        }
        else
        {
            if(player == null) player = other.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isCollidingWithPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isCollidingWithPlayer = false;
        }
    }
    IEnumerator Animation()
    {
        animator.SetTrigger(animationName);
        yield return new WaitForSeconds(0.2f);
    }
}
