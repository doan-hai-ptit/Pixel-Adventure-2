using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bee : Enemy
{
    [SerializeField] private float hoverAmplitude = 5f;  // Biên độ dao động (mức độ lên xuống)
    [SerializeField] private float hoverFrequency = 0.5f; 
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 0.4f;
    private float randomPhaseY;
    private float randomPhaseX;
    // Start is called before the first frame update
    void Start()
    {
        //this.enemyName = this.GetType().Name;
        randomPhaseY = Random.Range(0f, Mathf.PI * 2);
        randomPhaseX = Random.Range(0f, Mathf.PI * 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Idle();
            
        }
        
        if (IsPlayerInRange(radius))
        {
            Attack();
        }
    }

    protected override void Idle()
    {
        float velocityY = Mathf.Sin(Time.time * hoverFrequency + randomPhaseY) * hoverAmplitude;
        float velocityX = Mathf.Cos(Time.time * hoverFrequency + randomPhaseX) * hoverAmplitude;
        rb.velocity = new Vector2(velocityX, velocityY);
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
        yield return new WaitForSeconds(0.4f);
        Instantiate(bulletPrefab, transform.position + new Vector3(0, -0.687f, 0), Quaternion.identity);
        fireRate = 0.4f;
    }
    
}
