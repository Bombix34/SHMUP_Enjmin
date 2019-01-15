using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePattern : PatternInterface
{

    // mettre une valeur négative pour tourner dans le sens horaire
    public float speed;

    float distance = 120.0f;

    float distanceDone = 0f;

    public float diametre = 0.0f;

    //Position autour duquel le pote va tourner
    public Transform pivot;

    // Use this for initialization
    void Start()
    {
        if (pivot == null)
        {
            Debug.LogError("Error on CirclePattern : no pivot set.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        distanceDone += speed * Time.deltaTime * (distance - distanceDone + 5);
        if (Mathf.Abs(distanceDone) > distance)
        {
            distanceDone = 0;
        }

        if (pivot != null)
            transform.RotateAround(pivot.position, Vector3.forward, speed * Time.deltaTime * (distance - distanceDone + 5));

        // empecher le pote de tourner, et donc de provoquer une erreur dans le pattern à cause du scrolling
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    void OnDrawGizmos()
    {
        if (pivot != null)
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(pivot.position, diametre);
        }
    }
}
