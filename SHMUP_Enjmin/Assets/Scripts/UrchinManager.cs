using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrchinManager : MonoBehaviour {

    public float retractedTime = 3.0f;

    [HideInInspector]
    bool isRetracted = false;

    private float retractedCooldown = 0;
    private CircleCollider2D col;

    Animator animator;

    void Awake()
    {
        col = GetComponent<CircleCollider2D>();
        animator= GetComponent<Animator>();
    }

	void Update () {

        if (isRetracted)
        {
            retractedCooldown += Time.deltaTime;
            if(retractedCooldown > retractedTime)
                detract();
        }
	}

    public void retract()
    {
        retractedCooldown = 0;
        isRetracted = true;
        col.enabled=false;
        animator.SetTrigger("Retract");
    }

    public void detract()
    {
        isRetracted = false;
        col.enabled=true;
        animator.SetTrigger("Detract");
    }

    public bool GetIsRetracted()
    {
        return isRetracted;
    }
}
