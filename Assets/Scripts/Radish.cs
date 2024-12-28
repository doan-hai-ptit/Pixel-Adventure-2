using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radish : Enemy
{
    // Start is called before the first frame update
    [SerializeField] private GameObject wallCollison;
    [SerializeField] private float hoverAmplitude = 5f;  // Biên độ dao động (mức độ lên xuống)
    [SerializeField] private float hoverFrequency = 0.5f; 
    [SerializeField] private GameObject leaf1;
    [SerializeField] private GameObject leaf2;
    private float randomPhaseY;
    private float randomPhaseX;
    // Start is called before the first frame update
    void Start()
    {
        health = 2;
        //this.enemyName = this.GetType().Name;
        randomPhaseY = Random.Range(0f, Mathf.PI * 2);
        randomPhaseX = Random.Range(0f, Mathf.PI * 2);
    }
    // Update is called once per frame
    void Update()
    {
        if (isChangingDirection)
        {
            Idle();
        }
        else if(!isChangingDirection || !isDead)
        {
            Move();
        }
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }

    public override void Move()
    {
        if (isOtherForm)
        {
            if(isDead) rb.velocity = new Vector2(0, rb.velocity.y);
            else
            {
                rb.velocity = new Vector2(speedMove * direction, rb.velocity.y);
            }
        }
        else
        {
            float velocityY = Mathf.Sin(Time.time * hoverFrequency + randomPhaseY) * hoverAmplitude;
            float velocityX = Mathf.Cos(Time.time * hoverFrequency + randomPhaseX) * hoverAmplitude;
            if(velocityX > 0) direction = 1;
            else direction = -1;
            rb.velocity = new Vector2(velocityX, velocityY);
        }
        transform.localScale = new Vector3(direction * -1, 1, 1);
    }

    protected override void Idle()
    {
        rb.velocity = Vector2.zero;
    }
    
    protected override void ChangeForm()
    {
        StartCoroutine(Falling());
    }

    private IEnumerator Falling()
    {
        leaf1.SetActive(true);
        leaf2.SetActive(true);
        rb.velocity = Vector2.zero;
        animator.SetTrigger("HitOnAir");
        yield return new WaitForSeconds(0.25f);
        isOtherForm = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 10f;
        speedMove = 6;
    }
    public override IEnumerator ChangeDirection(float secondsDuration)
    {

        isChangingDirection = true;
        yield return new WaitForSeconds(secondsDuration);
        direction *= -1;
        isChangingDirection = false;
    }
}