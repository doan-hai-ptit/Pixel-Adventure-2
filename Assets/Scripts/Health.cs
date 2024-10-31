using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Health : MonoBehaviour
{
    [SerializeField] GameObject heartPrefab;
    [SerializeField] GameObject player;
    [SerializeField] Vector3[] positions = new Vector3[3];
    PlayerController playerController;
    private GameObject[] hearts = new GameObject[3];
    // Start is called before the first frame update
    public Vector2 originalSize;
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        UpdateHearts();
    }

    public void UpdateHearts()
    {
        foreach (GameObject heart in hearts)
        {
            if (heart != null)
            {
                Destroy(heart);
            }
        }
        for (int i = 0; i < playerController.currentHealth; i++)
        {
            hearts[i] = Instantiate(heartPrefab, transform.position + positions[i], Quaternion.identity, this.transform);
        }
    }
}
