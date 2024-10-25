using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private int health = 1;
    [SerializeField] GameObject[] debrises = new GameObject[4];
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            StartCoroutine(Break());
            health = 10;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetTrigger("Hit");
            this.health--;
        }
    }

    IEnumerator Break()
    {
        SceneController.instance.ShakeCamera(3, 0.125f);
        yield return new WaitForSeconds(0.15f);
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        boxCollider.isTrigger = true;
        for (int i = 0; i < 4; i++)
        { 
            debrises[i].SetActive(true);
            Disappear(debrises[i]);
        }
        yield return new WaitForSeconds(3f);
        //Destroy(gameObject);
    }

    private void Disappear(GameObject debris)
    {
        float time = Random.Range(1.5f, 2);
        StartCoroutine(flicker(debris.GetComponent<SpriteRenderer>(), time));
    }

    IEnumerator flicker(SpriteRenderer spriteRenderer, float time)
    {
        float delayTime = Random.Range(1, 1.2f);
        yield return new WaitForSeconds(delayTime);
        while (time > 0)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            time -= 0.2f;
            yield return new WaitForSeconds(0.2f);
        }
        spriteRenderer.enabled = false;
    }
}
