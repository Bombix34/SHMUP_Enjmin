﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePattern : MonoBehaviour {

    // mettre une valeur négative pour tourner dans le sens horaire
    public float speed = 100.0f;
    
    //Position autour duquel le pote va tourner
    public Transform pivot;

	// Use this for initialization
	void Start () {
		if(pivot == null)
        {
            Debug.LogError("Error on CirclePattern : no pivot declared.");
        }
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(pivot.position, Vector3.forward, speed * Time.deltaTime);
        // empecher le pote de tourner, et donc de provoquer une erreur dans le pattern à cause du scrolling
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

}