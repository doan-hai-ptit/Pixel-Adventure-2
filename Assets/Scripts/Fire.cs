using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    Animator animator;
    [SerializeField] LayerMask playerMask;
    [SerializeField] bool isHit = false;
    [SerializeField] private GameObject player;
    private float activeTime = 0.9f;
    private float activationDelay = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHit && IsHitted())
        { 
            isHit = true;
            animator.SetTrigger("Hit");
            StartCoroutine(ActivateFireTrap());
        }
        
        
    }

    private bool IsHitted()
    {
        return Physics2D.OverlapBox(this.transform.position - new Vector3(0f, 0.5f, 0), new Vector2(0.9f, 1f), 0f, playerMask);
    }

    private bool isBurned()
    {
        return Physics2D.OverlapBox(this.transform.position + new Vector3(0f, 0.5f, 0), new Vector2(0.4f, 4f), 0f, playerMask);
    }

    private IEnumerator ActivateFireTrap()
    {
        yield return new WaitForSeconds(activationDelay);
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (isBurned())
        {
            if (!playerController.isDead)
            {
                playerController.ChangeHealth(-1);
                playerController.Dead();
            }
        }
        yield return new WaitForSeconds(activeTime);
        this.isHit = false;
    }
}
