using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResettableObject : MonoBehaviour
{
    public Vector2 resetPosition;
    public Rigidbody2D rigidBody2d;
    public GameObject effect;
    public Animator animator;
    void Awake()
    {
        resetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void ResetObject()
    {
        Debug.Log("Reset Object");
    }
}
