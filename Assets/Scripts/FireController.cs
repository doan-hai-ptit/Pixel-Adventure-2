using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{

    [SerializeField] private Vector2[] firePoints;
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private float delta = 0.5f;

    [SerializeField] private float rotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        CreateFire();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateFire()
    {
        for (int i = 0; i < firePoints.Length; i++)
        {
                if (firePoints.Length > i + 1)
                {
                    Vector2 startPos = new Vector2(firePoints[i].x, firePoints[i].y);
                    Vector2 endPos =new Vector2(firePoints[i + 1].x, firePoints[i + 1].y);
                    Vector2 point = startPos;
                    Vector2 direction = (endPos - startPos).normalized;
                    while ((startPos - endPos).magnitude > (point - startPos).magnitude)
                    {
                        Quaternion q = Quaternion.Euler(0f, 0f, rotation);
                        Instantiate(firePrefab, point, q, this.transform);
                        point += direction * delta;
                    }
                }

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < firePoints.Length; i++)
        {
            Vector2 pos = new Vector2(firePoints[i].x,firePoints[i].y);
            Gizmos.DrawSphere(pos,0.3f);
        }
    }
}
