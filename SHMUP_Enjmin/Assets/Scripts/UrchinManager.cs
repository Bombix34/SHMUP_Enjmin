using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrchinManager : MonoBehaviour {

    public float retractedTime = 3.0f;

    [HideInInspector]
    public bool isRetracted = false;

    private float retractedCooldown = 0;
    private CircleCollider2D col;

    void Awake()
    {
        col = GetComponent<CircleCollider2D>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (isRetracted)
        {
            retractedCooldown += Time.deltaTime;
            if(retractedCooldown > retractedTime)
            {
                detract();
            }
        }
	}

    public void retract()
    {
        Debug.Log("JE RENTRE");
        retractedCooldown = 0;
        isRetracted = true;
        col.enabled = false;
        // TODO : animation de retractation

    }

    public void detract()
    {
        Debug.Log("JE SORS");
        isRetracted = false;
        col.enabled = true;
        // TODO : animation de detractation
    }
}
