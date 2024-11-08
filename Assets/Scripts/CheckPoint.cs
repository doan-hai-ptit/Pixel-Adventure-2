using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem particles;
    private bool isChecked = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isChecked)
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            player.RespawnPosition = transform.position;
            animator.SetBool("Checked", true);
            SceneController.instance.ShakeCamera(3, 0.125f);
            particles.Play();
            isChecked = true;
        }
    }
}
