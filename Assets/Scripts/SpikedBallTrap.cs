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
    private Vector2[] points = new Vector2[36];// goc la 36
    private Vector2[] reversedPoints = new Vector2[36];// goc la 36
    private float[] angleList = new float[37];
    private Rigidbody2D spikedBallRB;
    private GameObject spikedBall;
    private List<Rigidbody2D> chainPointRB = new List<Rigidbody2D>();
    private List<Vector2> movePoints = new List <Vector2>();
    private List<Vector2[]> moveChainsPoints = new List <Vector2[]>();// di chuyen luc dau
    private List<Vector2[]> chainsPoints = new List <Vector2[]>(); // di chuyen
    private List<Vector2[]> reversedChainPoints = new List<Vector2[]>();// di chuyen nguoc lai
    
    private int startPoint = 0;
    // Start is called before the first frame update
    void Start()
    {
        Init();
        CreateSpikedBall();
        MoveSpikedBall();
        CreateChainPoint();
        MoveChainPoint();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void Init()
    {
        //Init Spike Ball and something relate 
        Vector3 rightPoint = this.transform.position + new Vector3(this.radius * Mathf.Sin(angleRange/2 * Mathf.Deg2Rad),-this.radius * Mathf.Cos(angleRange/2 * Mathf.Deg2Rad), 0);
        Vector3 leftPoint = this.transform.position - new Vector3(this.radius * Mathf.Sin(angleRange/2 * Mathf.Deg2Rad), this.radius * Mathf.Cos(angleRange/2 * Mathf.Deg2Rad) , 0);
        this.points[0] = rightPoint;
        this.reversedPoints[35] = rightPoint;
        this.angleList[0] = this.angleRange/2 * Mathf.Deg2Rad;
        this.points[35] = leftPoint;// 18
        this.reversedPoints[0] = leftPoint;
        this.angleList[35] = -this.angleRange/2 * Mathf.Deg2Rad;
        float angle = angleRange * Mathf.Deg2Rad / (segments - 1);
        for (int i = 1; i <= this.segments - 2; i++)// co tat ca 19 diem
        {
            this.points[i] = this.transform.position + new Vector3(this.radius * Mathf.Sin(angleRange/2 * Mathf.Deg2Rad - angle * i),-this.radius * Mathf.Cos(angleRange/2 * Mathf.Deg2Rad - angle * i), 0);
            this.reversedPoints[35 - i] = points[i];
            this.angleList[i] = angleRange/2 * Mathf.Deg2Rad - angle * i;
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
            GameObject chain = Instantiate(chainPointPrefab, point, Quaternion.identity, this.transform) as GameObject;
            chainPointRB.Add(chain.GetComponent<Rigidbody2D>());
            Vector3 rightPoint = this.transform.position + new Vector3((point - startPos).magnitude * Mathf.Sin(angleRange/2 * Mathf.Deg2Rad),-(point - startPos).magnitude * Mathf.Cos(angleRange/2 * Mathf.Deg2Rad), 0);
            Vector3 leftPoint = this.transform.position - new Vector3((point - startPos).magnitude * Mathf.Sin(angleRange/2 * Mathf.Deg2Rad), (point - startPos).magnitude * Mathf.Cos(angleRange/2 * Mathf.Deg2Rad) , 0);
            Vector2[] temp = new Vector2[36];
            temp[0] = rightPoint; temp[35] = leftPoint;
            Vector2[] reversedTemp = new Vector2[36];
            reversedTemp[35] = rightPoint; reversedTemp[0] = leftPoint;
            Vector2[] firstMove = new Vector2[movePoints.Count - 1];
            for (int i = 1; i < this.segments-1; i++)
            {
                temp[i] = this.transform.position + new Vector3((point - startPos).magnitude * Mathf.Sin(this.angleList[i]),-(point - startPos).magnitude * Mathf.Cos(this.angleList[i]), 0);
                reversedTemp[35-i] = temp[i];
                
            }
            for (int i = startPoint; i < this.segments; i++)
            {
                firstMove[i - startPoint] = temp[i];
                //Debug.Log(firstMove[i - startPoint]);
            }
            reversedChainPoints.Add(reversedTemp);
            chainsPoints.Add(temp);
            moveChainsPoints.Add(firstMove);
            point += direction * delta;
        }
    }

    private void CreateSpikedBall()
    {
        Vector2 point = new Vector2(this.transform.position.x, this.transform.position.y) + new Vector2(this.radius * Mathf.Sin(startAngle * Mathf.Deg2Rad),-this.radius * Mathf.Cos(startAngle * Mathf.Deg2Rad));
        spikedBall = Instantiate(spikedBallPrefab, point, Quaternion.identity, this.transform) as GameObject;
        spikedBallRB = spikedBall.GetComponent<Rigidbody2D>();
        float radianAngle = startAngle * Mathf.Deg2Rad;
        if(radianAngle > Mathf.PI) radianAngle = -(2 * Mathf.PI - radianAngle);
        for (int i = 0; i < this.segments; i++)
        {
            if (angleList[i] < radianAngle)
            {
                this.startPoint = i;
                Debug.Log(startPoint);
                break;
            }   
        }
        movePoints.Add(point);
        for (int i = startPoint; i < this.segments; i++)
        {
            movePoints.Add(points[i]);
        }
        
    }

    private void MoveSpikedBall()
    {
        Sequence s1 = DOTween.Sequence();
        Vector2[] newArray = new Vector2[movePoints.Count];
        for (int i = 0; i < movePoints.Count; i++) {
                newArray[i] = movePoints[i];
        }
        s1.Append(spikedBallRB.DOPath(newArray, rotationSpeed * newArray.Length/36, PathType.Linear).SetEase(Ease.Linear)).OnComplete(() =>
        {
            s1.Kill();
            s1 = null;
            Sequence s2 = DOTween.Sequence();
            if (closedLoop)
            {
                s2.Append(spikedBallRB.DOPath(points, rotationSpeed, PathType.Linear).SetEase(Ease.Linear)).SetLoops(-1, LoopType.Yoyo);
                s2.SetLoops(-1, LoopType.Restart);
            }
            else
            {
                s2.Append(spikedBallRB.DOPath(reversedPoints, rotationSpeed, PathType.Linear).SetEase(Ease.Linear)).SetLoops(-1, LoopType.Yoyo);
            }
        });
    }

    private void MoveChainPoint()
    {
        for (int i = chainPointRB.Count -1; i >=0; i--)
        {
            Sequence s1 = DOTween.Sequence();
            var i1 = i;
            s1.Append(chainPointRB[i]
                .DOPath(moveChainsPoints[i], rotationSpeed * moveChainsPoints[i].Length / 36, PathType.Linear)
                .SetEase(Ease.Linear)).OnComplete(() =>
            {
                //s1.Kill();
                Sequence s2 = DOTween.Sequence();
                if (closedLoop)
                {
                    s2.Append(chainPointRB[i].DOPath(chainsPoints[i], rotationSpeed, PathType.Linear).SetEase(Ease.Linear));
                    s2.SetLoops(-1, LoopType.Restart);
                }
                else
                {
                    s2.Append(chainPointRB[i].DOPath(reversedChainPoints[i], rotationSpeed, PathType.Linear).SetEase(Ease.Linear));
                    s2.SetLoops(-1, LoopType.Yoyo);
                }
            });
        }
    }
    
    
    private void OnDrawGizmos()
    {
        //Draw radius
        Gizmos.color = Color.blue;
        Vector3 rightPoint = this.transform.position + new Vector3(this.radius * Mathf.Sin(angleRange/2 * Mathf.Deg2Rad),-this.radius * Mathf.Cos(angleRange/2 * Mathf.Deg2Rad), 0);
        Vector3 leftPoint = this.transform.position - new Vector3(this.radius * Mathf.Sin(angleRange/2 * Mathf.Deg2Rad), this.radius * Mathf.Cos(angleRange/2 * Mathf.Deg2Rad) , 0);
        Vector3[] points = new Vector3[segments];
        points[0] = rightPoint;
        points[35] = leftPoint;
        float angle = angleRange * Mathf.Deg2Rad / (segments - 1);
        for (int i = 1; i <= segments - 2; i+=2)
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
