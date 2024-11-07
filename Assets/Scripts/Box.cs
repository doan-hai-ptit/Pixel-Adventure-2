using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] public int health = 1;
    [SerializeField] GameObject[] debrises = new GameObject[4];
    [SerializeField] GameObject[] items = new GameObject[9];
    [SerializeField] LayerMask layerMask;
    [SerializeField] float timer = 0;
    [SerializeField] int amountItem = 0;
    [SerializeField] GameObject headPart;
    [SerializeField] GameObject bodyPart;
    [SerializeField] Animator animator;
    private bool isHitted = false;
    private GameObject player;
    private Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb2d = player.GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    isHitted = false;
                }
            }
        }
        else
        {
            if(health == 0) StartCoroutine(Break());
            health = -1;
        }
    }

    public void UpHitted()
    {
        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(new Vector2(0f, 26f), ForceMode2D.Impulse);
        SceneController.instance.ShakeCamera(3, 0.125f);
        animator.SetTrigger("Hit");
        timer = 0.15f;
        this.health--;
    }

    public void DownHitted()
    {
        SceneController.instance.ShakeCamera(3, 0.125f);
        animator.SetTrigger("Hit");
        timer = 0.15f;
        this.health--;
        isHitted = true;

    }

    IEnumerator Break()
    {
        yield return new WaitForSeconds(0.15f);
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        BoxCollider2D boxCollider1 = headPart. GetComponent<BoxCollider2D>();
        BoxCollider2D boxCollider2 = bodyPart.GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        boxCollider1.enabled = false;
        boxCollider2.enabled = false;
        for (int i = 0; i < 4; i++)
        { 
            debrises[i].SetActive(true);
            Disappear(debrises[i]);
        }
        Launch(debrises[0], new Vector2(Random.Range(-3.0f, -1.0f), Random.Range(1.0f, 3.0f)));
        Launch(debrises[1], new Vector2(Random.Range(1.0f, 3.0f), Random.Range(1.0f, 3.0f)));
        Launch(debrises[2], new Vector2(Random.Range(-3.0f, -1.0f), 0f));
        Launch(debrises[3], new Vector2(Random.Range(1.0f, 3.0f), 0f));
        for (int i = 0; i < amountItem; i++)
        {
            int item = Random.Range(-1, 17);
            DropItem(items[item/2], new Vector2(Random.Range(-3.0f, 3.0f), 0f));
        }
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
