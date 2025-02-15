using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RockHeadController : MonoBehaviour
{
    [SerializeField] private bool closedLoop;
    [SerializeField] private Vector2[] movePoints;
    [SerializeField] private GameObject rockHeadPrefab;
    [SerializeField] private float moveSpeed;
    [SerializeField] private RockHeadCollision topRockHeadCollision;
    [SerializeField] private RockHeadCollision bottomRockHeadCollision;
    [SerializeField] private RockHeadCollision rightRockHeadCollision;
    [SerializeField] private RockHeadCollision leftRockHeadCollision;
    [SerializeField] private bool horizontal = true;
    private GameObject rockHead;
    private new Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        CreateSaw();
        Move();
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
    }
    

    void CreateSaw()
    {
        Vector2 startPos = new Vector2(movePoints[0].x, movePoints[0].y);
        rockHead = Instantiate(rockHeadPrefab, startPos, Quaternion.identity, this.transform) as GameObject;
        rigidbody2D = rockHead.GetComponent<Rigidbody2D>();
    }

    void SetUp()
    {
        topRockHeadCollision = rockHead.transform.GetChild(0).GetComponent<RockHeadCollision>();
        bottomRockHeadCollision = rockHead.transform.GetChild(1).GetComponent<RockHeadCollision>();
        rightRockHeadCollision = rockHead.transform.GetChild(2).GetComponent<RockHeadCollision>();
        leftRockHeadCollision = rockHead.transform.GetChild(3).GetComponent<RockHeadCollision>();
        if (movePoints.Length >= 4)
        {
            topRockHeadCollision.SetNextPart(rightRockHeadCollision);
            leftRockHeadCollision.SetNextPart(topRockHeadCollision);
            bottomRockHeadCollision.SetNextPart(leftRockHeadCollision);
            rightRockHeadCollision.SetNextPart(bottomRockHeadCollision);
        }
        else if (movePoints.Length >= 2 && horizontal)
        {
            topRockHeadCollision.SetNextPart(rightRockHeadCollision);
            leftRockHeadCollision.SetNextPart(rightRockHeadCollision);
            bottomRockHeadCollision.SetNextPart(leftRockHeadCollision);
            rightRockHeadCollision.SetNextPart(leftRockHeadCollision);
        }
        else
        {
            topRockHeadCollision.SetNextPart(bottomRockHeadCollision);
            leftRockHeadCollision.SetNextPart(topRockHeadCollision);
            bottomRockHeadCollision.SetNextPart(topRockHeadCollision);
            rightRockHeadCollision.SetNextPart(bottomRockHeadCollision);
        }
    }
    void Move()
    {
        Sequence s = DOTween.Sequence();
        if (closedLoop)
        {
            Vector2[] newArray = new Vector2[movePoints.Length + 1];
            for (int i = 0; i < movePoints.Length; i++)
            {
                newArray[i] = movePoints[i];
            }
            float distance = Vector2.Distance(movePoints[0], movePoints[movePoints.Length - 1]);
            newArray[movePoints.Length] = movePoints[0];
            int len = movePoints.Length + 1;
            for (int i = 1; i < len; i++)
            {
                s.Append(rigidbody2D.DOMove(newArray[i], moveSpeed * (newArray[i] - newArray[i-1]).magnitude / distance).SetEase(Ease.Linear)).AppendInterval(0.5f);
            }
            //.Append(rigidbody2D.DOPath(newArray, moveSpeed, PathType.Linear).SetEase(Ease.Linear)).AppendInterval(0.5f);
            s.SetLoops(-1, LoopType.Restart);
        }
        else
        {
            int len = movePoints.Length;
            for (int i = 0; i < len; i++)
            {
                s.Append(rigidbody2D.DOMove(movePoints[i], moveSpeed).SetEase(Ease.Linear)).AppendInterval(0.5f);
            }
            //s.Append(rigidbody2D.DOPath(movePoints, moveSpeed, PathType.Linear).SetEase(Ease.Linear)).AppendInterval(0.5f);
            s.SetLoops(-1, LoopType.Yoyo);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < movePoints.Length; i++)
        {
            Vector2 pos = new Vector2(movePoints[i].x, movePoints[i].y);
            Gizmos.DrawSphere(pos,0.3f);
        }
    }

    public void OnDestroy()
    {
        DOTween.KillAll();
    }
}
