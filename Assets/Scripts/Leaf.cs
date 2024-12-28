using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] SpriteRenderer spriteRenderer;
    void Start()
    {
        float time = Random.Range(1.0f, 1.5f);
        StartCoroutine(Flicker(spriteRenderer, time));
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
        Destroy(gameObject);
    }
}
