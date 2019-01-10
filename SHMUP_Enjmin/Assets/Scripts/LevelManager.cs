using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    float rightMostSituationBound;

    List<GameObject> plafondsBackground;
    List<GameObject> plafondsMiddleground;
    List<GameObject> plafondsForeground;

    float rightMostBackgroundPlafondBound;
    float rightMostMiddlegroundPlafondBound;
    float rightMostForegroundPlafondBound;

    public LevelReglages reglages;

    void Start ()
    {
        rightMostSituationBound = Camera.main.orthographicSize * Camera.main.aspect;
        plafondsBackground = new List<GameObject>();
        plafondsMiddleground = new List<GameObject>();
        plafondsForeground = new List<GameObject>();
        rightMostBackgroundPlafondBound = Camera.main.orthographicSize * Camera.main.aspect;
        rightMostMiddlegroundPlafondBound = Camera.main.orthographicSize * Camera.main.aspect;
        rightMostForegroundPlafondBound = Camera.main.orthographicSize * Camera.main.aspect;
    }

    void Update ()
    {
        rightMostSituationBound -= reglages.scrollingSpeed * Time.deltaTime;
        rightMostBackgroundPlafondBound -= reglages.scrollingSpeed * Time.deltaTime * 0.33f;
        rightMostMiddlegroundPlafondBound -= reglages.scrollingSpeed * Time.deltaTime * 0.66f;
        rightMostForegroundPlafondBound -= reglages.scrollingSpeed * Time.deltaTime;

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

        while (rightMostBackgroundPlafondBound < Camera.main.orthographicSize * Camera.main.aspect)
        {
            int idPlafond = Random.Range(0, reglages.plafonds.Count);
            GameObject newBasePlafond = (GameObject)Instantiate(reglages.plafonds[idPlafond], new Vector3(rightMostBackgroundPlafondBound, reglages.hauteursPlafonds[idPlafond], 0.3f), transform.rotation);
            newBasePlafond.transform.Translate(new Vector3(Camera.main.orthographicSize * Camera.main.aspect - GetGameObjectLeftMostBound(newBasePlafond), 0));
            Color color = newBasePlafond.GetComponent<SpriteRenderer>().material.color;
            color *= 0.1f;
            color.a = 1.0f;
            newBasePlafond.GetComponent<SpriteRenderer>().material.color = color;
            rightMostBackgroundPlafondBound = GetGameObjectRightMostBound(newBasePlafond) - Random.Range(0.5f, 2.0f);
            plafondsBackground.Add(newBasePlafond);
        }

        while (rightMostMiddlegroundPlafondBound < Camera.main.orthographicSize * Camera.main.aspect)
        {
            int idPlafond = Random.Range(0, reglages.plafonds.Count);
            GameObject newBasePlafond = (GameObject)Instantiate(reglages.plafonds[idPlafond], new Vector3(rightMostMiddlegroundPlafondBound, reglages.hauteursPlafonds[idPlafond], 0.2f), transform.rotation);
            newBasePlafond.transform.Translate(new Vector3(Camera.main.orthographicSize * Camera.main.aspect - GetGameObjectLeftMostBound(newBasePlafond), 0));
            Color color = newBasePlafond.GetComponent<SpriteRenderer>().material.color;
            color *= 0.45f;
            color.a = 1.0f;
            newBasePlafond.GetComponent<SpriteRenderer>().material.color = color;
            rightMostMiddlegroundPlafondBound = GetGameObjectRightMostBound(newBasePlafond) + Random.Range(0.25f, 1.0f);
            plafondsMiddleground.Add(newBasePlafond);
        }

        while (rightMostForegroundPlafondBound < Camera.main.orthographicSize * Camera.main.aspect)
        {
            int idPlafond = Random.Range(0, reglages.plafonds.Count);
            GameObject newBasePlafond = (GameObject)Instantiate(reglages.plafonds[idPlafond], new Vector3(rightMostForegroundPlafondBound, reglages.hauteursPlafonds[idPlafond], 0.1f), transform.rotation);
            newBasePlafond.transform.Translate(new Vector3(Camera.main.orthographicSize * Camera.main.aspect - GetGameObjectLeftMostBound(newBasePlafond), 0));
            rightMostForegroundPlafondBound = GetGameObjectRightMostBound(newBasePlafond) + Random.Range(0.5f, 2.0f);
            plafondsForeground.Add(newBasePlafond);
        }

        if (plafondsBackground.Count != 0)
        {
            while (GetGameObjectRightMostBound(plafondsBackground.First()) < -Camera.main.orthographicSize * Camera.main.aspect)
            {
                GameObject basePlafondToRemove = plafondsBackground.First();
                plafondsBackground.RemoveAt(0);
                Destroy(basePlafondToRemove);

                if (plafondsBackground.Count == 0)
                {
                    break;
                }
            }
        }

        if (plafondsMiddleground.Count != 0)
        {
            while (GetGameObjectRightMostBound(plafondsMiddleground.First()) < -Camera.main.orthographicSize * Camera.main.aspect)
            {
                GameObject basePlafondToRemove = plafondsMiddleground.First();
                plafondsMiddleground.RemoveAt(0);
                Destroy(basePlafondToRemove);

                if (plafondsMiddleground.Count == 0)
                {
                    break;
                }
            }
        }

        if (plafondsForeground.Count != 0)
        {
            while (GetGameObjectRightMostBound(plafondsForeground.First()) < -Camera.main.orthographicSize * Camera.main.aspect)
            {
                GameObject basePlafondToRemove = plafondsForeground.First();
                plafondsForeground.RemoveAt(0);
                Destroy(basePlafondToRemove);

                if (plafondsForeground.Count == 0)
                {
                    break;
                }
            }
        }

        Vector3 scrollingVector = new Vector3(-reglages.scrollingSpeed * Time.deltaTime, 0.0f, 0.0f);

        foreach (GameObject basePlafond in plafondsBackground)
        {
            basePlafond.transform.Translate(scrollingVector * 0.33f);
        }

        foreach (GameObject basePlafond in plafondsMiddleground)
        {
            basePlafond.transform.Translate(scrollingVector * 0.66f);
        }

        foreach (GameObject basePlafond in plafondsForeground)
        {
            basePlafond.transform.Translate(scrollingVector);
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