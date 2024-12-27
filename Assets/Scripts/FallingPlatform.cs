using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float hoverAmplitude = 0.3f;  // Biên độ dao động (mức độ lên xuống)
    [SerializeField] private float hoverFrequency = 2f;  
    [SerializeField] private float hoverAmplitudeWhenFall = 2f;  // Biên độ dao động (mức độ lên xuống)
    [SerializeField] private float hoverFrequencyWhenFall = 8f;  
    [SerializeField] private GameObject airEffect;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb2d;
    public float timeToFall = 1.2f;
    public bool isOn = true;
    public bool isFalling = false;
    public float randomPhaseY;
    public float randomPhaseX;
    // Start is called before the first frame update
    void Start()
    {
        randomPhaseY = Random.Range(0f, Mathf.PI * 2);
        randomPhaseX = Random.Range(0f, Mathf.PI * 2);
        //rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("On", isOn);
        if (isFalling)
        {
            if (timeToFall > 0)
            {
                PrepareToFall();
                timeToFall -= Time.deltaTime;
                if (timeToFall <= 0)
                {
                    
                    StartCoroutine(Falling());
                }
            }
        }
        else
        {
            Swing();
        }
    }
    IEnumerator Falling()
    {
        GameController.instance.CollectObj(this.gameObject);
        airEffect.SetActive(false);
        isOn = false;
        yield return new WaitForSeconds(0.25f);
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        rb2d.gravityScale = 10;
        yield return new WaitForSeconds(1);
        this.gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            //if (Mathf.Abs(rb.velocity.y - rb2d.velocity.y) < 0.1)
            if(rb.position.y - rb2d.position.y >= 1.05f && Mathf.Abs(rb2d.position.x - rb.position.x) <= 1.5f && Mathf.Abs(rb.velocity.y - rb2d.velocity.y) <= 0.1f)
            {
                isFalling = true;
            }
        }
    }

    private void Swing()
    {
        float velocityY = Mathf.Sin(Time.time * hoverFrequency + randomPhaseY) * hoverAmplitude;
        float velocityX = Mathf.Cos(Time.time * hoverFrequency + randomPhaseX) * hoverAmplitude;
        rb2d.velocity = new Vector2(velocityX, velocityY);
    }

    private void PrepareToFall()
    {
        float velocityY = Mathf.Sin(Time.time * hoverFrequencyWhenFall) * hoverAmplitudeWhenFall;
        rb2d.velocity = new Vector2(0, velocityY);
    }

}
