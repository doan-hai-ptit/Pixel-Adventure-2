using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollision : MonoBehaviour
{
    [SerializeField] private Enemy enemyScript;
    [SerializeField] private float delaySeconds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            StartCoroutine(enemyScript.ChangeDirection(delaySeconds));
        }
    }

 
}
