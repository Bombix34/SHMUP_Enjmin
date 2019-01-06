using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    float rightMostSituationBound;

    public LevelReglages reglages;


    void Start ()
    {
        rightMostSituationBound = Camera.main.orthographicSize * Camera.main.aspect;
	}

    void Update ()
    {
        rightMostSituationBound -= reglages.scrollingSpeed * Time.deltaTime;

        while (rightMostSituationBound < Camera.main.orthographicSize * Camera.main.aspect)
        {
            GameObject newSituation = (GameObject)Instantiate(reglages.GetLevelAtRandom(), new Vector3(rightMostSituationBound, 0), transform.rotation);
            newSituation.transform.Translate(new Vector3(Camera.main.orthographicSize * Camera.main.aspect - GetSituationLeftMostBound(newSituation), 0));
            rightMostSituationBound = GetSituationRightMostBound(newSituation);
            for (int i = newSituation.transform.childCount - 1; i >= 0; i--)
            {
                newSituation.transform.GetChild(i).parent = null;
            }
            Destroy(newSituation);
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