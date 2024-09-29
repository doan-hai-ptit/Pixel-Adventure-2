using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DageZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Animator animator = other.GetComponent<Animator>();
            animator.SetTrigger("Hit");
        }
    }
}
