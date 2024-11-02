using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformMachine : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Vector2 endPosition;
    [SerializeField] private GameObject brownPlatformPrefab;
    [SerializeField] private GameObject greyPlatformPrefab;
    [SerializeField] private GameObject chainPrefab;
    [SerializeField] private bool horizontal = false;
    [SerializeField] private float delta = 0.5f;
    [SerializeField] private float speedMove = 4f;
    private Platform platform;
    void Start()
    {
        CreatePlatform();
        CreateChain();
        if (horizontal)
        {
            platform.velocity = new Vector2(speedMove, 0);
        }
        else
        {
            platform.velocity = new Vector2(0, speedMove);
        }
        platform.startPosition = startPosition;
        platform.endPosition = endPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreatePlatform()
    {
        if (horizontal)
        {
             platform = Instantiate(greyPlatformPrefab, startPosition, Quaternion.identity, this.transform).gameObject.GetComponent<Platform>();
            
        }
        else
        {
            platform = Instantiate(brownPlatformPrefab, startPosition, Quaternion.identity, this.transform).gameObject.GetComponent<Platform>();
        }
    }
    
    private void CreateChain()
    {
        Vector2 startPos = new Vector2(startPosition.x, startPosition.y);
        Vector2 endPos =new Vector2(endPosition.x, endPosition.y);
        Vector2 point = startPos;
        Vector2 direction = (endPos - startPos).normalized;
        while ((startPos - endPos).magnitude > (point - startPos).magnitude)
        {
            Instantiate(chainPrefab, point, Quaternion.identity, this.transform);
            point += direction * delta;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(startPosition, 0.3f);
        Gizmos.DrawSphere(endPosition, 0.3f);
    }
}
