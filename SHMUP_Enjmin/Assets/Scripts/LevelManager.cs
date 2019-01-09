using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    float rightMostSituationBound;
    float rightMostBasePlafondBound;

    List<GameObject> basesPlafond;
    List<List<GameObject>> stalactiteGroups;

    public LevelReglages reglages;

    void Start ()
    {
        rightMostSituationBound = Camera.main.orthographicSize * Camera.main.aspect;
        rightMostBasePlafondBound = Camera.main.orthographicSize * Camera.main.aspect;
        basesPlafond = new List<GameObject>();
        stalactiteGroups = new List<List<GameObject>>();
    }

    void Update ()
    {
        rightMostSituationBound -= reglages.scrollingSpeed * Time.deltaTime;
        rightMostBasePlafondBound -= reglages.scrollingSpeed * Time.deltaTime;

        while (rightMostSituationBound < Camera.main.orthographicSize * Camera.main.aspect)
        {
            GameObject newSituation = (GameObject)Instantiate(reglages.GetLevelAtRandom(), new Vector3(rightMostSituationBound, 0), transform.rotation);
            newSituation.transform.Translate(new Vector3(Camera.main.orthographicSize * Camera.main.aspect - GetGameObjectChildrenLeftMostBound(newSituation), 0));
            rightMostSituationBound = GetGameObjectChildrenRightMostBound(newSituation) + (Camera.main.orthographicSize * Camera.main.aspect) / 2;
            for (int i = newSituation.transform.childCount - 1; i >= 0; i--)
            {
                newSituation.transform.GetChild(i).parent = null;
            }
            Destroy(newSituation);
        }

        while (rightMostBasePlafondBound < Camera.main.orthographicSize * Camera.main.aspect)
        {
            GameObject newBasePlafond = (GameObject)Instantiate(reglages.GetBaseDePlafondRandom(), new Vector3(rightMostBasePlafondBound, 4.0731f), transform.rotation);
            newBasePlafond.transform.Translate(new Vector3(Camera.main.orthographicSize * Camera.main.aspect - GetGameObjectLeftMostBound(newBasePlafond), 0));
            rightMostBasePlafondBound = GetGameObjectRightMostBound(newBasePlafond) + Random.Range(0.5f, 4.0f);
            basesPlafond.Add(newBasePlafond);
            List<GameObject> stalactitesGroup = new List<GameObject>();
            float stalactitesXPos = GetGameObjectLeftMostBound(newBasePlafond) + Random.Range(0.1f, 1.0f);
            while (stalactitesXPos < GetGameObjectRightMostBound(newBasePlafond))
            {
                GameObject stalactite = (GameObject)Instantiate(reglages.GetRandomStalactite(), new Vector3(stalactitesXPos, 4.0f + Random.Range(-0.5f, 0.5f), 0.1f), transform.rotation);
                stalactite.transform.position += new Vector3(stalactite.GetComponent<Renderer>().bounds.max.x - stalactite.transform.position.x, 0.0f, 0.0f);
                stalactitesGroup.Add(stalactite);
                stalactitesXPos += GetGameObjectSize(stalactite) + Random.Range(0.1f, 1.0f);
            }
            stalactiteGroups.Add(stalactitesGroup);
        }

        if (basesPlafond.Count != 0)
        {
            while (GetGameObjectRightMostBound(basesPlafond.First()) < -Camera.main.orthographicSize * Camera.main.aspect)
            {
                GameObject basePlafondToRemove = basesPlafond.First();
                basesPlafond.RemoveAt(0);
                Destroy(basePlafondToRemove);
                List<GameObject> stalactiteGroupToRemove = stalactiteGroups.First();
                stalactiteGroups.RemoveAt(0);
                foreach (GameObject stalactite in stalactiteGroupToRemove)
                {
                    Destroy(stalactite);
                }

                if (basesPlafond.Count == 0)
                {
                    break;
                }
            }
        }

        Vector3 scrollingVector = new Vector3(-reglages.scrollingSpeed * Time.deltaTime, 0.0f, 0.0f);

        foreach (GameObject basePlafond in basesPlafond)
        {
            basePlafond.transform.Translate(scrollingVector);
        }

        foreach (List<GameObject> stalactitesGroup in stalactiteGroups)
        {
            foreach (GameObject stalactite in stalactitesGroup)
            {
                stalactite.transform.Translate(scrollingVector * 0.8f);
            }
        }
    }

    float GetGameObjectSize(GameObject gameObjectParam)
    {
        return GetGameObjectRightMostBound(gameObjectParam) - GetGameObjectLeftMostBound(gameObjectParam);
    }

    float GetGameObjectLeftMostBound(GameObject gameObjectParam)
    {
        return gameObjectParam.GetComponentInChildren<Renderer>().bounds.min.x;
    }

    float GetGameObjectRightMostBound(GameObject gameObjectParam)
    {
        return gameObjectParam.GetComponentInChildren<Renderer>().bounds.max.x;
    }

    float GetGameObjectChildrenLeftMostBound(GameObject gameObjectParam)
    {
        List<Transform> elements = new List<Transform>();
        foreach (Transform child in gameObjectParam.transform)
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

    float GetGameObjectChildrenRightMostBound(GameObject gameObjectParam)
    {
        List<Transform> elements = new List<Transform>();
        foreach (Transform child in gameObjectParam.transform)
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