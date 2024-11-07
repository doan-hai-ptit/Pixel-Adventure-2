using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using Random = UnityEngine.Random;

public class Plant : Enemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 0.4f;
    [SerializeField] private Vector2 force = new Vector2(15, 0);
    // Start is called before the first frame update
    void Start()
    {
        direction = (int)transform.localScale.x * -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Idle();
            
        }
        
        if (IsPlayerInRange(width, height))
        {
            Attack();
        }
    }

    protected override void Idle()
    {
        
    }
    public override void Attack()
    {
        if (fireRate > 0)
        {
            fireRate -= Time.deltaTime;
            if (fireRate <= 0)
            {
                StartCoroutine(Launch());
            }
        }
    }

    IEnumerator Launch()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.3f);
        GameObject bullet =  Instantiate(bulletPrefab, transform.position + new Vector3(0.8f * direction, 0.3f, 0), Quaternion.identity) as GameObject;
        Rigidbody2D rigidbody2D = bullet.GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = force * direction;
        Debug.Log(force);
        fireRate = 0.4f;
    }
    
}