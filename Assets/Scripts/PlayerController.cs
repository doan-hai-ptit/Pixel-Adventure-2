using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerController : MonoBehaviour
{
    // Variables related to player character movement
    public InputAction moveAction;
    Rigidbody2D rigidbody2d;
    float move;
    float moveSpeed = 8.0f;
    // Start is called before the first frame update
    void Start()
    {
        moveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        move = moveAction.ReadValue<float>();
        Debug.Log(move);
    }

    private void FixedUpdate()
    {
       // Vector2 position = (Vector2)rigidbody2d.position + move * moveSpeed * Time.deltaTime;
        //rigidbody2d.MovePosition(position);
    }
}
