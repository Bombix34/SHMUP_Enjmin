using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownPattern : PatternInterface {

    // direction initiale du pote
    public bool directionUp = true;
    // distance parcouru (entre la position la plus haute et la position la plus basse)
    public float distance = 3.0f;
    // vitesse de déplacement du pote
    public float speed = 0.1f;

    private float distanceDone = 0f;

	// Use this for initialization
	void Start () {
		
	}

    private void Update()
    {

        distanceDone += Time.deltaTime * speed;
        if (distanceDone > distance)
        {
            directionUp = !directionUp;
            distanceDone = 0;
        }
        if (directionUp)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * speed, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * speed, transform.position.z);
        }
    }
}