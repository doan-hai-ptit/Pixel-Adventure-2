using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPoint : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem particles;
    private bool started = true;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && started)
        {
            GameController.instance.ShakeCamera(3, 0.125f);
            animator.SetTrigger("Move");
            started = false;
            particles.Play();
        }
    }
}
