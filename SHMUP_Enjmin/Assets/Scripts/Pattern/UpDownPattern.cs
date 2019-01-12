using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownPattern : PatternInterface {

    // direction initiale du pote
    public bool directionUp = true;
    // distance parcouru (entre la position la plus haute et la position la plus basse)
    public float distance = 3.0f;
    // vitesse de déplacement du pote
    public float speed = 1f;

    public float distanceDone = 0f;

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

    void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Vector3 to;
        if(directionUp)
            to = new Vector3(transform.position.x, transform.position.y + (distance - distanceDone), transform.position.z);
        else
            to = new Vector3(transform.position.x, transform.position.y - (distance - distanceDone), transform.position.z);
        Gizmos.DrawLine(transform.position, to);

        Vector3 to2;
        if (directionUp)
            to2 = new Vector3(transform.position.x, transform.position.y - distanceDone, transform.position.z);
        else
            to2 = new Vector3(transform.position.x, transform.position.y + distanceDone, transform.position.z);
        Gizmos.DrawLine(transform.position, to2);

    }
}