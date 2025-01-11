using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetArrow : ResettableObject
{
    // Start is called before the first frame update
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] public float resetTime = 0.0f;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (resetTime > 0)
        {
            resetTime -= Time.deltaTime;
            if (resetTime <= 0)
            {
                this.ResetObject();
            }
        }
    }
    // Update is called once per frame

    public override void ResetObject()
    {
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
        this.gameObject.SetActive(true);
    }
}
