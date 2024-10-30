using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DageZone : MonoBehaviour
{
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if(!player.isDead) player.Dead();
        }
    }

    protected void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if(!player.isDead) player.Dead();
        }
    }
}
