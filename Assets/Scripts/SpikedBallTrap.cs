using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class SpikedBallTrap : MonoBehaviour
{
    
    [SerializeField] private bool closedLoop;
    [SerializeField] private bool clockWise;
    [SerializeField] private GameObject chainPointPrefab;
    [SerializeField] private GameObject spikedBallPrefab;
    [SerializeField] private float radius;
    [SerializeField] private float delta;
    [SerializeField, Range(0f, 360f)] private float angleRange;
    [SerializeField, Range(0f, 360f)] private float startAngle;
    [SerializeField] private float rotationSpeed;
    private int segments = 36;
    private Vector2[] points = new Vector2[36];
    private GameObject spikedBall;
    private Rigidbody2D spikedBallRB;
    // Start is called before the first frame update
    void Start()
    {
        Init();
        CreateChainPoint();
        CreateSpikedBall();
        MoveSpikedBall();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void Init()
    {
        Vector3 rightPoint = this.transform.position + new Vector3(this.radius * Mathf.Sin(angleRange/2 * Mathf.Deg2Rad),-this.radius * Mathf.Cos(angleRange/2 * Mathf.Deg2Rad), 0);
        Vector3 leftPoint = this.transform.position - new Vector3(this.radius * Mathf.Sin(angleRange/2 * Mathf.Deg2Rad), this.radius * Mathf.Cos(angleRange/2 * Mathf.Deg2Rad) , 0);
        points[0] = rightPoint;
        points[35] = leftPoint;// 18
        float angle = angleRange * Mathf.Deg2Rad / (segments - 1);
        for (int i = 1; i <= this.segments - 2; i++)// co tat ca 19 diem
        {
            points[i] = this.transform.position + new Vector3(this.radius * Mathf.Sin(angleRange/2 * Mathf.Deg2Rad - angle * i),-this.radius * Mathf.Cos(angleRange/2 * Mathf.Deg2Rad - angle * i), 0);
        }
    }

    private void CreateChainPoint()
    {
        Vector2 startPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 endPos = new Vector2(this.transform.position.x, this.transform.position.y) + new Vector2(this.radius * Mathf.Sin(startAngle * Mathf.Deg2Rad),-this.radius * Mathf.Cos(startAngle * Mathf.Deg2Rad));
        Vector2 point = startPos;
        Vector2 direction = (endPos - startPos).normalized;
        while ((endPos - startPos).magnitude > (point - startPos).magnitude)
        {
            Instantiate(chainPointPrefab, point, Quaternion.identity, this.transform);
            point += direction * delta;
        }
    }

    private void CreateSpikedBall()
    {
        Vector2 point = new Vector2(this.transform.position.x, this.transform.position.y) + new Vector2(this.radius * Mathf.Sin(startAngle * Mathf.Deg2Rad),-this.radius * Mathf.Cos(startAngle * Mathf.Deg2Rad));
        spikedBall = Instantiate(spikedBallPrefab, point, Quaternion.identity, this.transform) as GameObject;
        spikedBallRB = spikedBall.GetComponent<Rigidbody2D>();
    }

    private void MoveSpikedBall()
    {
        Sequence s = DOTween.Sequence();
        if (closedLoop)
        {
            Vector2[] newArray = new Vector2[points.Length];
            for (int i = 0; i < this.segments; i++)
            {
                newArray[i] = points[i];
            }
//            newArray[this.segments] = points[0];
            s.Append(spikedBallRB.DOPath(newArray, rotationSpeed, PathType.Linear).SetEase(Ease.Linear));
            s.SetLoops(-1, LoopType.Restart);
        }
        else
        {
           // s.Append(spikedBallRB.DOPath(points[0], rotationSpeed, PathType.Linear).SetEase(Ease.Linear));
        }
    }
    
    private void OnDrawGizmos()
    {
        //Draw radius
        Gizmos.color = Color.blue;
        Vector3 rightPoint = this.transform.position + new Vector3(this.radius * Mathf.Sin(angleRange/2 * Mathf.Deg2Rad),-this.radius * Mathf.Cos(angleRange/2 * Mathf.Deg2Rad), 0);
        Vector3 leftPoint = this.transform.position - new Vector3(this.radius * Mathf.Sin(angleRange/2 * Mathf.Deg2Rad), this.radius * Mathf.Cos(angleRange/2 * Mathf.Deg2Rad) , 0);
        Vector3[] points = new Vector3[segments + 2];
        points[0] = rightPoint;
        points[segments + 1] = leftPoint;
        float angle = angleRange * Mathf.Deg2Rad / (segments - 1);
        for (int i = 1; i <= segments - 1; i+=2)
        {
            points[i] = this.transform.position + new Vector3(this.radius * Mathf.Sin(angleRange/2 * Mathf.Deg2Rad - angle * i),-this.radius * Mathf.Cos(angleRange/2 * Mathf.Deg2Rad - angle * i), 0);
            points[i + 1] = this.transform.position + new Vector3(this.radius * Mathf.Sin(angleRange/2 * Mathf.Deg2Rad - angle * i),-this.radius * Mathf.Cos(angleRange/2 * Mathf.Deg2Rad - angle * i), 0); 
        }
        Gizmos.DrawLine(this.transform.position, leftPoint);
        Gizmos.DrawLine(this.transform.position, rightPoint);
        Gizmos.DrawLineList(points);
        
        //Draw start posision
        Vector3 startPos = this.transform.position + new Vector3(this.radius * Mathf.Sin(startAngle * Mathf.Deg2Rad),-this.radius * Mathf.Cos(startAngle * Mathf.Deg2Rad), 0);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(this.transform.position, startPos);
        
    }
}
