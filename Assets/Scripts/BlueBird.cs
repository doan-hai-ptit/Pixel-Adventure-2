using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBird : Enemy
{
    // Start is called before the first frame update
    [SerializeField] private GameObject wallCollison;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private float time = 0.45f;
    // Start is called before the first frame update
    void Start()
    {
        float duration = particleSystem.main.duration;

        // Thời gian sống tối đa của một hạt
        float maxLifetime = particleSystem.main.startLifetime.constantMax;

        Debug.Log($"Particle System Duration: {duration} seconds");
        Debug.Log($"Particle Max Lifetime: {maxLifetime} seconds");

        // Tổng thời gian hệ thống tồn tại (Duration + Max Lifetime)
        float totalTime = duration + maxLifetime;
        Debug.Log($"Total Particle System Time: {totalTime} seconds");
        health = 1;
    }
    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                particleSystem.Play();
                time = 0.45f;
            }
        }
        Move();
    }

    public override void Move()
    {
        if (!isDead)
        {
            if(isChangingDirection) Idle();
            else
            {
                transform.localScale = new Vector3(direction * -1, 1, 1);
                rb.velocity = new Vector2(speedMove * direction, 0);
            }
        }
    }

    protected override void Idle()
    {
        rb.velocity = Vector2.zero;
    }
    public override IEnumerator ChangeDirection(float secondsDuration)
    {

        isChangingDirection = true;
        yield return new WaitForSeconds(secondsDuration);
        direction *= -1;
        isChangingDirection = false;
    }
}