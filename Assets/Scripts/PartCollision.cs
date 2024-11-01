using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartCollision : MonoBehaviour
{
    [SerializeField] private string partName;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject box;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && enemy !=null)
        {
            if (partName == "Head")
            {
                Enemy enemyScript = enemy.gameObject.GetComponent<Enemy>();
                enemyScript.Hitted();
            }
            else if (partName == "Body")
            {
                Enemy enemyScript = enemy.gameObject.GetComponent<Enemy>();
                enemyScript.Hit();
            }
        }
        else if (other.CompareTag("Player"))
        {
            if (partName == "Head")
            {
                Box boxScript = box.gameObject.GetComponent<Box>();
                boxScript.UpHitted();
            }
            else if (partName == "Body")
            {
                Box boxScript = box.gameObject.GetComponent<Box>();
                boxScript.DownHitted();
            }
        }
    }
}
