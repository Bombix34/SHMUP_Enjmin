using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentaclesManager : MonoBehaviour {

    public GameObject tentacles;

    private float distanceDone = 0;


    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start () {

        float dist = Camera.main.transform.position.z;

        Vector3 hg = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, -dist));
        Vector3 bg = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -dist));

        Vector3 pos;

        //bordure Gauche
        tentacles.transform.position = hg;
        pos = bg - hg;
        tentacles.transform.position.Scale(new Vector3(0.5F, 0.5F, 0.5F));
        tentacles.transform.Translate(pos / 2);

    }

    // Update is called once per frame
    void Update () {


	}
}
