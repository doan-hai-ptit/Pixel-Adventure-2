using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : DageZone
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] bulletPieces = new GameObject[2];
    [SerializeField] private float timeExist = 5;
    private bool isBroken = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeExist > 0)
        {
            timeExist -= Time.deltaTime;
            if (timeExist <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        if (OnGround() && !isBroken)
        {
            //Destroy(this.gameObject);
            StartCoroutine(Break());
            isBroken = true;
        }
    }

    bool OnGround()
    {
            //return Physics2D.OverlapBox(rigidbody2d.position + Vector2.down, new Vector2(0.9f, 0.1f), 0.0f, LayerMask.GetMask("Ground")) || Physics2D.OverlapBox(rigidbody2d.position + Vector2.down, new Vector2(1.0f, 0.1f), 0.0f, LayerMask.GetMask("Platform")) || Physics2D.OverlapBox(rigidbody2d.position + Vector2.down, new Vector2(1.0f, 0.1f), 0.0f, LayerMask.GetMask("DeadZone"));
            return Physics2D.OverlapBox(transform.position + new Vector3(0, -0.35f, 0), new Vector2(0.2f, 0.1f), 0.0f, LayerMask.GetMask("Ground"));
    }
    
    private void Disappear(GameObject debris)
    {
        float time = Random.Range(1.0f, 1.5f);
        StartCoroutine(Flicker(debris.GetComponent<SpriteRenderer>(), time));
    }
    
    
    IEnumerator Flicker(SpriteRenderer spriteRenderer, float time)
    {
        float delayTime = Random.Range(1f, 1.5f);
        yield return new WaitForSeconds(delayTime);
        while (time > 0)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            time -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        spriteRenderer.enabled = false;
    }
    
    private void Launch(GameObject debris, Vector2 force)
    {
        Rigidbody2D rb = debris.GetComponent<Rigidbody2D>();
        rb.AddForce(force, ForceMode2D.Impulse);
    }
    
    IEnumerator Break()
    {
        //yield return new WaitForSeconds(0.15f);
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        for (int i = 0; i < 2; i++)
        { 
            bulletPieces[i].SetActive(true);
            Disappear(bulletPieces[i]);
        }
        Launch(bulletPieces[0], new Vector2(Random.Range(-3.0f, -1.0f), Random.Range(5f, 20f)));
        Launch(bulletPieces[1], new Vector2(Random.Range(1.0f, 3.0f), Random.Range(5f, 20f)));
        yield return new WaitForSeconds(3.5f);
        Destroy(gameObject);
    }
}
