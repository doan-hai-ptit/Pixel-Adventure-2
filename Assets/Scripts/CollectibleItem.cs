using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//using Random = System.Random;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb2d;
    bool collected = false;
    float timeDestroyed = 0.25f;
    float startFrame = 0f;
    private void Start()
    {
        startFrame = Random.Range (0.0f, 0.2f);
        animator.Play("Idle",0, startFrame);
    }

    void Update()
    {
        if (collected)
        {
            timeDestroyed -= Time.deltaTime;
        }
        if(timeDestroyed <= 0) Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !collected)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.CollectedFruit();
            collected = true;
            animator.SetTrigger("Collected");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && !collected)
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.CollectedFruit();
            collected = true;
            animator.SetTrigger("Collected");
        }
    }
}
