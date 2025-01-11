using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetFallingPlatform : ResettableObject
{
    // Start is called before the first frame update
    [SerializeField] private float resetTime = 20.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (resetTime > 0)
        {
            resetTime -= Time.deltaTime;
            //if(resetTime <= 0) this.ResetObject();
        }
    }

    public override void ResetObject()
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
        resetTime = 20.0f;
    }
}
