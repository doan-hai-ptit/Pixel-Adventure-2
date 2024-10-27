using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private int health = 1;
    [SerializeField] GameObject[] debrises = new GameObject[4];
    [SerializeField] GameObject[] items = new GameObject[9];
    [SerializeField] LayerMask layerMask;
    //[SerializeField] Vector2 froce;
    Animator animator;
    private bool isHitted = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Hitted() && !isHitted)
        {
            animator.SetTrigger("Hit");
            this.health--;
            isHitted = true;
        }
        if (health <= 0)
        {
            StartCoroutine(Break());
            health = 10;
        }
    }

    private bool Hitted()
    {
        return Physics2D.OverlapBox(transform.position + new Vector3(0,0.6f, 0), new Vector2(0.8f, 0.2f), 0.5f, layerMask);
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
     //       animator.SetTrigger("Hit");
     //       this.health--;
     //   }
    //}

    IEnumerator Break()
    {
        SceneController.instance.ShakeCamera(3, 0.125f);
        yield return new WaitForSeconds(0.15f);
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        for (int i = 0; i < 4; i++)
        { 
            debrises[i].SetActive(true);
            Disappear(debrises[i]);
        }
        Launch(debrises[0], new Vector2(Random.Range(-3.0f, -1.0f), Random.Range(1.0f, 3.0f)));
        Launch(debrises[1], new Vector2(Random.Range(1.0f, 3.0f), Random.Range(1.0f, 3.0f)));
        Launch(debrises[2], new Vector2(Random.Range(-3.0f, -1.0f), 0f));
        Launch(debrises[3], new Vector2(Random.Range(1.0f, 3.0f), 0f));
        int item1 = Random.Range(0, 9);
        int item2 = Random.Range(0, 9);
        int item3 = Random.Range(0, 9);
        DropItem(items[item1], new Vector2(Random.Range(-3.0f, 3.0f), 0f));
        DropItem(items[item2], new Vector2(Random.Range(-3.0f, 3.0f), 0f));
        DropItem(items[item3], new Vector2(Random.Range(-3.0f, 3.0f), 0f));
        //Launch(debrises[2], froce);
        yield return new WaitForSeconds(3.5f);
        Destroy(gameObject);
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

    private void DropItem(GameObject item, Vector2 force)
    {
        GameObject itemDrop = Instantiate(item, this.transform.position, Quaternion.identity);
        Rigidbody2D rb = itemDrop.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 5f;
        rb.AddForce(force, ForceMode2D.Impulse);
        BoxCollider2D boxCollider = itemDrop.GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = false;
    }
}
