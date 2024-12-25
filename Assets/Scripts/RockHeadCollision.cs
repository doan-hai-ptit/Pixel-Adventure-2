using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockHeadCollision : MonoBehaviour
{
    [SerializeField] private string animationName;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController player;
    [SerializeField] private bool isCollidingWithPlayer = false;
    [SerializeField] private RockHeadCollision nextPart;
    [SerializeField] private Renderer rockHead;
    private bool isActive = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive && !other.CompareTag("Player"))
        {
            if (isCollidingWithPlayer)
            {
                player.ChangeHealth(-1);
                player.Dead();
                isCollidingWithPlayer = false;
            }
            //Debug.Log(rockHead.isVisible);
            if(rockHead.isVisible) GameController.instance.ShakeCamera(1.5f, 0.125f);
            isActive = false;
            nextPart.SetActive(true);
            StartCoroutine(Animation());
        }
        else if (isActive)
        {
            if(player == null) player = other.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            isCollidingWithPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            isCollidingWithPlayer = false;
        }
    }
    IEnumerator Animation()
    {
        animator.SetTrigger(animationName);
        yield return new WaitForSeconds(0.2f);
    }

    public void SetNextPart(RockHeadCollision nextPart)
    {
        this.nextPart = nextPart;
    }

    public void SetActive(bool active)
    {
        this.isActive = active;
    }
}
