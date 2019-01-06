using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    float rightMostSituationBound;
    List<GameObject> situations;


    public LevelReglages reglages;


    void Start ()
    {
        rightMostSituationBound = Camera.main.orthographicSize * Camera.main.aspect;
        situations = new List<GameObject>();
	}

    void Update ()
    {
        ScrollLevels();
        
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

    void ScrollLevels()
    {
        foreach (GameObject situation in situations)
        {
            situation.transform.Translate(-reglages.scrollingSpeed*Time.deltaTime,0f,0f);
        }
        rightMostSituationBound -= reglages.scrollingSpeed*Time.deltaTime;
    }

    void FillScreenWithSituations()
    {
        while (rightMostSituationBound < Camera.main.orthographicSize * Camera.main.aspect)
        {
            GameObject nextLevel = reglages.GetLevelAtRandom();
            situations.Add((GameObject)Instantiate(nextLevel, new Vector3(rightMostSituationBound, 0), transform.rotation));
            situations.Last().transform.Translate(new Vector3 (Camera.main.orthographicSize * Camera.main.aspect - GetSituationLeftMostBound(situations.Last()), 0));
            rightMostSituationBound = GetSituationRightMostBound(situations.Last());
            foreach (Transform child in situations.Last().transform)
            {
                child.parent = null;
            }
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
            float situationLeftMostBound = elements.Min(element => element.gameObject.GetComponentInChildren<Renderer>().bounds.min.x);
            for (int i = 0; i < elements.Count; i++)
            {
                //print(elements[i].gameObject.GetComponent<Renderer>().bounds.min.x);
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
            float situationRightMostBound = elements.Max(element => element.gameObject.GetComponentInChildren<Renderer>().bounds.max.x);
            return situationRightMostBound;
        }
        else
        {
            return 0.0f;
        }
    }

    public float GetScrollingSpeed()
    {
        return reglages.scrollingSpeed;
    }

//SINGLETON________________________________________________________________________________________________
	private static LevelManager s_Instance = null;

    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static LevelManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(LevelManager)) as LevelManager;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
                Debug.Log("error");
                GameObject obj = new GameObject("Error");
                s_Instance = obj.AddComponent(typeof(LevelManager)) as LevelManager;
            }

            return s_Instance;
        }
    }

}