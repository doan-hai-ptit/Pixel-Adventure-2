using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//using Random = System.Random;

public class CollectibleItem : MonoBehaviour
{
    Animator animator;
    bool collected = false;
    float timeDestroyed = 0.25f;
    float startFrame = 0f;
    private void Start()
    {
        animator = GetComponent<Animator>();
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
        if (other.CompareTag("Player"))
        {
            collected = true;
            animator.SetTrigger("Collected");
        }
    }
}
