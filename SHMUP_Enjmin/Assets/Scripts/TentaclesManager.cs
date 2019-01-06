using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentaclesManager : MonoBehaviour {

    public static TentaclesManager instance;

    public GameObject[] tentacles;

    public float distanceAtEachCapture = 2f;
    public float moveSpeedForward = 0.1f;

    public float distanceAtEachSave = 2f;
    public float moveSpeedBackward = 0.1f;

    private Coroutine deplacement;

    public float distanceDone = 0;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start () {

        float dist = Camera.main.transform.position.z;

        Vector3 hg = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, -dist));
        Vector3 bg = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -dist));

        Vector3 pos;

        //bordure Gauche
        transform.position = hg;
        pos = bg - hg;
        transform.position.Scale(new Vector3(0.5F, 0.5F, 0.5F));
        transform.Translate(pos / 2);

    }

    void Update () {


	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ToSave")
        {
            MoveForward();
        }
        else if(collision.gameObject.tag=="Player")
        {
            GameManager.instance.GameOver();
        }
    }

    public void MoveForward()
    {
        // TODO : animation de capture

        // deplacement des tentacules
        distanceDone += distanceAtEachCapture;
        StartCoroutine(MoveForwardCoroutine());
    }

    public void MoveBackward()
    {
        // deplacement des tentacules
        distanceDone -= distanceAtEachSave;
        StartCoroutine(MoveBackwardCoroutine());
    }

    IEnumerator MoveForwardCoroutine()
    {
        float dist = 0;
        while(distanceAtEachCapture > dist)
        {
            transform.position = new Vector3(transform.position.x + moveSpeedForward, transform.position.y, transform.position.z);
            dist += moveSpeedForward;
            yield return null;
        }
    }

    IEnumerator MoveBackwardCoroutine()
    {
        float dist = 0;
        while (distanceAtEachSave > dist && distanceDone >= 0)
        {
            transform.position = new Vector3(transform.position.x - moveSpeedBackward, transform.position.y, transform.position.z);
            dist += moveSpeedBackward;
            yield return null;
        }
    }
}
