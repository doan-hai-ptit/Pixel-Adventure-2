using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : Enemy
{
    // Start is called before the first frame update
    public float spikeTime = 2.5f;
    private bool isSpike = true;
    [SerializeField] private GameObject dageZone;
    [SerializeField] private GameObject head;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spikeTime > 0)
        {
            spikeTime -= Time.deltaTime;
            if (spikeTime <= 0)
            {
                isSpike = !isSpike;
                animator.SetBool("Spike", isSpike);
                spikeTime = 2.5f;
            }
        }

        if (isSpike)
        {
            dageZone.SetActive(true);
            head.SetActive(false);
        }
        else
        {
            dageZone.SetActive(false);
            head.SetActive(true);
        }
    }
}
