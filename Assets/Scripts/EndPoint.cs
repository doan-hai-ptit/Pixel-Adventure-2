using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem particles;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] Animator animatorPlayer;
    [SerializeField] Renderer rend;
    private bool pressed = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !pressed && GameController.instance.IsEligible())
        {
            GameController.instance.ShakeCamera(3, 0.125f);
            rb = other.GetComponent<Rigidbody2D>();
            animatorPlayer = other.GetComponent<Animator>();
            rend = other.GetComponent<Renderer>();
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * 26f, ForceMode2D.Impulse);
            StartCoroutine(End());
            animator.SetTrigger("Pressed");
            pressed = true;
            particles.Play();
            
        }
    }

    IEnumerator End()
    {
        //yield return new WaitForSeconds(0.1f);
        animatorPlayer.SetBool("Disappearing", true);
        yield return new WaitForSeconds(0.35f);
        rend.enabled = false;
        yield return new WaitForSeconds(0.35f);
        GameController.instance.CompleteLV();
        SaveSystem.Instance.SaveGame();
    }
}