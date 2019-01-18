using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStart : MonoBehaviour {

        Animator anim;

    float chrono;

    private void Start()
    {
        anim = GetComponent<Animator>();
        
        chrono = Random.Range(3f, 8f);
    }

    void Update()
    {
        if (chrono > 0)
            chrono -= Time.deltaTime;
        else
        {
            anim.SetTrigger("launch");
            chrono = Random.Range(3f, 8f);
        }
    }

}
