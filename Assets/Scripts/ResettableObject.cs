using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResettableObject : MonoBehaviour
{
    private Vector2 resetPosition;
    [SerializeField] private Rigidbody2D rigidBody2d;
    [SerializeField] private GameObject effect;
    [SerializeField] private Animator animator;
    void Awake()
    {
        resetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetObject()
    {
        rigidBody2d.bodyType = RigidbodyType2D.Kinematic;
        FallingPlatform fp = this.GetComponent<FallingPlatform>();
        fp.isOn = true;
        fp.isFalling = false;
        fp.randomPhaseY = Random.Range(0f, Mathf.PI * 2);
        fp.randomPhaseX = Random.Range(0f, Mathf.PI * 2);
        fp.timeToFall = 1.2f;
        this.transform.position = resetPosition;
        this.gameObject.SetActive(true);
        effect.SetActive(true);
        Debug.Log("Reset Object");
    }
}
