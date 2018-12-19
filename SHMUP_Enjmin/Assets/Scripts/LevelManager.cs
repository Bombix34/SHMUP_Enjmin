using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    float rightMostSituationBound;
    List<GameObject> situations;

    public float scrollingSpeed;

    public GameObject iterableSituation1;
    public GameObject iterableSituation2;
    public GameObject iterableSituation3;
    public GameObject iterableSituation4;

    // Use this for initialization
    void Start ()
    {
        rightMostSituationBound = Camera.main.orthographicSize * Camera.main.aspect;
        situations = new List<GameObject>();
	}

    // Update is called once per frame
    void Update ()
    {
        foreach (GameObject situation in situations)
        {
            situation.transform.position -= new Vector3 (scrollingSpeed, 0.0f, 0.0f);
        }
        rightMostSituationBound -= scrollingSpeed;
        
        if (situations.Count != 0)
        {
            while (GetSituationRightMostBound(situations.First()) < -1 * Camera.main.orthographicSize * Camera.main.aspect)
            {
                GameObject situationToDestroy = situations.First();
                situations.RemoveAt(0);
                Destroy(situationToDestroy);
            }
        }

        FillScreenWithSituations();
	}

    void FillScreenWithSituations()
    {
        while (rightMostSituationBound < Camera.main.orthographicSize * Camera.main.aspect)
        {
            int situationId = Random.Range(0, 4);
            if (situationId == 0)
            {
                situations.Add((GameObject)Instantiate(iterableSituation1, new Vector3(rightMostSituationBound, 0), transform.rotation));
            }
            else if (situationId == 1)
            {
                situations.Add((GameObject)Instantiate(iterableSituation2, new Vector3(rightMostSituationBound, 0), transform.rotation));
            }
            else if (situationId == 2)
            {
                situations.Add((GameObject)Instantiate(iterableSituation3, new Vector3(rightMostSituationBound, 0), transform.rotation));
            }
            else if (situationId == 3)
            {
                situations.Add((GameObject)Instantiate(iterableSituation4, new Vector3(rightMostSituationBound, 0), transform.rotation));
            }
            situations.Last().transform.Translate(new Vector3 (Camera.main.orthographicSize * Camera.main.aspect - GetSituationLeftMostBound(situations.Last()), 0));
            rightMostSituationBound = GetSituationRightMostBound(situations.Last());
        }
    }

    float GetSituationLeftMostBound(GameObject situation)
    {
        List<Transform> elements = new List<Transform>();
        foreach (Transform child in situation.transform)
        {
            elements.Add(child);
        }
        if (elements.Count != 0)
        {
            float situationLeftMostBound = elements.Min(element => element.gameObject.GetComponent<Renderer>().bounds.min.x);
            for (int i = 0; i < elements.Count; i++)
            {
                print(elements[i].gameObject.GetComponent<Renderer>().bounds.min.x);
            }
            return situationLeftMostBound;
        }
        else
        {
            return 0.0f;
        }
    }

    float GetSituationRightMostBound(GameObject situation)
    {
        List<Transform> elements = new List<Transform>();
        foreach (Transform child in situation.transform)
        {
            elements.Add(child);
        }
        if (elements.Count != 0)
        {
            float situationRightMostBound = elements.Max(element => element.gameObject.GetComponent<Renderer>().bounds.max.x);
            return situationRightMostBound;
        }
        else
        {
            return 0.0f;
        }
    }
}