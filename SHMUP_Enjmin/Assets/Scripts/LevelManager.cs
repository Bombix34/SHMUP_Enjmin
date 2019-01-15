using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    float rightmostSituationBound;

    List<GameObject> plafondsBackground;
    List<GameObject> plafondsMiddleground;
    List<GameObject> plafondsForeground;

    float rightmostBackgroundPlafondBound;
    float rightmostMiddlegroundPlafondBound;
    float rightmostForegroundPlafondBound;

    public LevelReglages reglages;

    float surfaceJouable;
    float surfaceJouable2;

    int score;

    void Start ()
    {
        rightmostSituationBound = Camera.main.orthographicSize * Camera.main.aspect;

        plafondsBackground = new List<GameObject>();
        plafondsMiddleground = new List<GameObject>();
        plafondsForeground = new List<GameObject>();
        rightmostBackgroundPlafondBound = Camera.main.orthographicSize * Camera.main.aspect;
        rightmostMiddlegroundPlafondBound = Camera.main.orthographicSize * Camera.main.aspect;
        rightmostForegroundPlafondBound = Camera.main.orthographicSize * Camera.main.aspect;

        surfaceJouable = 100.0f;

        score = 0;
    }

    void Update ()
    {
        rightmostSituationBound -= reglages.scrollingSpeed * Time.deltaTime;
        rightmostBackgroundPlafondBound -= reglages.scrollingSpeed * Time.deltaTime * 0.25f;
        rightmostMiddlegroundPlafondBound -= reglages.scrollingSpeed * Time.deltaTime * 0.5f;
        rightmostForegroundPlafondBound -= reglages.scrollingSpeed * Time.deltaTime * 0.75f;

        if (reglages.levelPartsEasy.Count + reglages.levelPartsNormal.Count + reglages.levelPartsHard.Count > 0)
        {
            if (rightmostSituationBound < Camera.main.orthographicSize * Camera.main.aspect)
            {
                GameObject newSituation;
                if (score < reglages.pallierNormal)
                {
                    newSituation = (GameObject)Instantiate(reglages.GetEasyLevel(), new Vector3(rightmostSituationBound, 0), transform.rotation);
                } else if (score < reglages.pallierDifficile) {
                    newSituation = (GameObject)Instantiate(reglages.GetNormalLevel(), new Vector3(rightmostSituationBound, 0), transform.rotation);
                } else {
                    newSituation = (GameObject)Instantiate(reglages.GetHardLevel(), new Vector3(rightmostSituationBound, 0), transform.rotation);
                }
                
                newSituation.transform.Translate(new Vector3(Camera.main.orthographicSize * Camera.main.aspect - GetGameObjectLeftmostBound(newSituation), 0.0f));
                rightmostSituationBound = GetGameObjectRightmostBound(newSituation) + (Camera.main.orthographicSize * Camera.main.aspect) / 2;
                for (int i = newSituation.transform.childCount - 1; i > 0; i--)
                {
                    surfaceJouable -= GetGameObjectSurface(newSituation.transform.GetChild(i).gameObject) / 2;
                    newSituation.transform.GetChild(i).parent = null;
                }
                Destroy(newSituation);
            }
        }

        if (rightmostBackgroundPlafondBound < Camera.main.orthographicSize * Camera.main.aspect)
        {
            int idPlafond = Random.Range(0, reglages.plafonds.Count);
            GameObject newBasePlafond = (GameObject)Instantiate(reglages.plafonds[idPlafond], new Vector3(rightmostBackgroundPlafondBound, reglages.hauteursPlafonds[idPlafond], 0.3f), transform.rotation);
            newBasePlafond.transform.Translate(new Vector3(Camera.main.orthographicSize * Camera.main.aspect - GetGameObjectLeftmostBound(newBasePlafond), 0));
            Color color = newBasePlafond.GetComponent<SpriteRenderer>().material.color;
            color *= 0.1f;
            color.a = 1.0f;
            newBasePlafond.GetComponent<SpriteRenderer>().material.color = color;
            rightmostBackgroundPlafondBound = GetGameObjectRightmostBound(newBasePlafond) - Random.Range(0.5f, 2.0f);
            plafondsBackground.Add(newBasePlafond);
        }

        if (rightmostMiddlegroundPlafondBound < Camera.main.orthographicSize * Camera.main.aspect)
        {
            int idPlafond = Random.Range(0, reglages.plafonds.Count);
            GameObject newBasePlafond = (GameObject)Instantiate(reglages.plafonds[idPlafond], new Vector3(rightmostMiddlegroundPlafondBound, reglages.hauteursPlafonds[idPlafond], 0.2f), transform.rotation);
            newBasePlafond.transform.Translate(new Vector3(Camera.main.orthographicSize * Camera.main.aspect - GetGameObjectLeftmostBound(newBasePlafond), 0));
            Color color = newBasePlafond.GetComponent<SpriteRenderer>().material.color;
            color *= 0.2f;
            color.a = 1.0f;
            newBasePlafond.GetComponent<SpriteRenderer>().material.color = color;
            rightmostMiddlegroundPlafondBound = GetGameObjectRightmostBound(newBasePlafond) + Random.Range(0.25f, 1.0f);
            plafondsMiddleground.Add(newBasePlafond);
        }

        if (rightmostForegroundPlafondBound < Camera.main.orthographicSize * Camera.main.aspect)
        {
            int idPlafond = Random.Range(0, reglages.plafonds.Count);
            GameObject newBasePlafond = (GameObject)Instantiate(reglages.plafonds[idPlafond], new Vector3(rightmostForegroundPlafondBound, reglages.hauteursPlafonds[idPlafond], 0.1f), transform.rotation);
            newBasePlafond.transform.Translate(new Vector3(Camera.main.orthographicSize * Camera.main.aspect - GetGameObjectLeftmostBound(newBasePlafond), 0));
            Color color = newBasePlafond.GetComponent<SpriteRenderer>().material.color;
            color *= 0.3f;
            color.a = 1.0f;
            newBasePlafond.GetComponent<SpriteRenderer>().material.color = color;
            rightmostForegroundPlafondBound = GetGameObjectRightmostBound(newBasePlafond) + Random.Range(0.5f, 2.0f);
            plafondsForeground.Add(newBasePlafond);
        }

        if (GetGameObjectRightmostBound(plafondsBackground.First()) < -Camera.main.orthographicSize * Camera.main.aspect && plafondsBackground.Count != 0)
        {
            GameObject basePlafondToRemove = plafondsBackground.First();
            plafondsBackground.RemoveAt(0);
            Destroy(basePlafondToRemove);
        }

        if (GetGameObjectRightmostBound(plafondsMiddleground.First()) < -Camera.main.orthographicSize * Camera.main.aspect && plafondsMiddleground.Count != 0)
        {
            GameObject basePlafondToRemove = plafondsMiddleground.First();
            plafondsMiddleground.RemoveAt(0);
            Destroy(basePlafondToRemove);
        }

        if (GetGameObjectRightmostBound(plafondsForeground.First()) < -Camera.main.orthographicSize * Camera.main.aspect && plafondsForeground.Count != 0)
        {
            GameObject basePlafondToRemove = plafondsForeground.First();
            plafondsForeground.RemoveAt(0);
            Destroy(basePlafondToRemove);
        }

        Vector3 scrollingVector = new Vector3(-reglages.scrollingSpeed * Time.deltaTime, 0.0f, 0.0f);

        foreach (GameObject basePlafond in plafondsBackground)
        {
            basePlafond.transform.Translate(scrollingVector * 0.25f);
        }

        foreach (GameObject basePlafond in plafondsMiddleground)
        {
            basePlafond.transform.Translate(scrollingVector * 0.5f);
        }

        foreach (GameObject basePlafond in plafondsForeground)
        {
            basePlafond.transform.Translate(scrollingVector * 0.75f);
        }

        surfaceJouable2 = surfaceJouable;
        AkSoundEngine.SetRTPCValue("Surface_jouable", surfaceJouable2, gameObject);
    }

    public void ChangeScore(int change)
    {
        score += change;
    }

    public void ReleasePlayableSpace(GameObject gameObjectParam)
    {
        surfaceJouable += GetGameObjectSurface(gameObjectParam) / 2;
    }

    float GetGameObjectSurface(GameObject gameObjectParam)
    {
        return GetGameObjectWidth(gameObjectParam) * GetGameObjectHeight(gameObjectParam);
    }

    float GetGameObjectHeight(GameObject gameObjectParam)
    {
        return GetGameObjectHighestBound(gameObjectParam) - GetGameObjectLowestBound(gameObjectParam);
    }

    float GetGameObjectWidth(GameObject gameObjectParam)
    {
        return GetGameObjectRightmostBound(gameObjectParam) - GetGameObjectLeftmostBound(gameObjectParam);
    }

    float GetGameObjectHighestBound(GameObject gameObjectParam)
    {
        float situationHighestBound = gameObjectParam.transform.position.y;

        SpriteRenderer renderer = gameObjectParam.GetComponent<SpriteRenderer>();

        if (gameObjectParam.GetComponent<SpriteRenderer>() != null)
        {
            float rendererHighestBound = renderer.bounds.max.y;

            if (rendererHighestBound > situationHighestBound)
            {
                situationHighestBound = rendererHighestBound;
            }
        }

        SpriteMask mask = gameObjectParam.GetComponent<SpriteMask>();

        if (gameObjectParam.GetComponent<SpriteMask>() != null)
        {
            float maskHighestBound = renderer.bounds.max.y;

            if (maskHighestBound > situationHighestBound)
            {
                situationHighestBound = maskHighestBound;
            }
        }

        foreach (Transform child in gameObjectParam.transform)
        {
            if (child != gameObjectParam.transform)
            {
                float childHighestBound = GetGameObjectHighestBound(child.gameObject);
                if (childHighestBound > situationHighestBound)
                {
                    situationHighestBound = childHighestBound;
                }
            }
        }

        return situationHighestBound;
    }

    float GetGameObjectLowestBound(GameObject gameObjectParam)
    {
        float situationLowestBound = gameObjectParam.transform.position.y;

        SpriteRenderer renderer = gameObjectParam.GetComponent<SpriteRenderer>();

        if (gameObjectParam.GetComponent<SpriteRenderer>() != null)
        {
            float rendererLowestBound = renderer.bounds.min.y;

            if (rendererLowestBound < situationLowestBound)
            {
                situationLowestBound = rendererLowestBound;
            }
        }

        SpriteMask mask = gameObjectParam.GetComponent<SpriteMask>();

        if (gameObjectParam.GetComponent<SpriteMask>() != null)
        {
            float maskLowestBound = renderer.bounds.min.y;

            if (maskLowestBound < situationLowestBound)
            {
                situationLowestBound = maskLowestBound;
            }
        }

        foreach (Transform child in gameObjectParam.transform)
        {
            if (child != gameObjectParam.transform)
            {
                float childLowestBound = GetGameObjectLowestBound(child.gameObject);
                if (childLowestBound < situationLowestBound)
                {
                    situationLowestBound = childLowestBound;
                }
            }
        }

        return situationLowestBound;
    }

    public float GetGameObjectLeftmostBound(GameObject gameObjectParam)
    {
        float situationLeftmostBound = gameObjectParam.transform.position.x;

        Renderer renderer = gameObjectParam.GetComponent<SpriteRenderer>();

        if (gameObjectParam.GetComponent<SpriteRenderer>() != null)
        {
            float rendererLeftmostBound = renderer.bounds.min.x;

            if (rendererLeftmostBound < situationLeftmostBound)
            {
                situationLeftmostBound = rendererLeftmostBound;
            }
        }

        SpriteMask mask = gameObjectParam.GetComponent<SpriteMask>();

        if (gameObjectParam.GetComponent<SpriteMask>() != null)
        {
            float maskLeftmostBound = renderer.bounds.min.x;

            if (maskLeftmostBound < situationLeftmostBound)
            {
                situationLeftmostBound = maskLeftmostBound;
            }
        }

        foreach (Transform child in gameObjectParam.transform)
        {
            if (child != gameObjectParam.transform)
            {
                float childLeftmostBound = GetGameObjectLeftmostBound(child.gameObject);

                if (childLeftmostBound < situationLeftmostBound)
                {
                    situationLeftmostBound = childLeftmostBound;
                }
            }
        }

        return situationLeftmostBound;
    }

    public float GetGameObjectRightmostBound(GameObject gameObjectParam)
    {
        float situationRightmostBound = gameObjectParam.transform.position.x;

        Renderer renderer = gameObjectParam.GetComponentInChildren<SpriteRenderer>();

        if (gameObjectParam.GetComponent<SpriteRenderer>() != null)
        {
            float rendererRightmostBound = renderer.bounds.max.x;

            if (rendererRightmostBound > situationRightmostBound)
            {
                situationRightmostBound = rendererRightmostBound;
            }
        }

        SpriteMask mask = gameObjectParam.GetComponent<SpriteMask>();

        if (gameObjectParam.GetComponent<SpriteMask>() != null)
        {
            float maskRightmostBound = renderer.bounds.max.x;

            if (maskRightmostBound > situationRightmostBound)
            {
                situationRightmostBound = maskRightmostBound;
            }
        }

        foreach (Transform child in gameObjectParam.transform)
        {
            if (child != gameObjectParam.transform)
            {
                float childRightmostBound = GetGameObjectRightmostBound(child.gameObject);
                if (childRightmostBound > situationRightmostBound)
                {
                    situationRightmostBound = childRightmostBound;
                }
            }
        }

        return situationRightmostBound;
    }

    public float GetScrollingSpeed()
    {
        return reglages.scrollingSpeed;
    }

//SINGLETON________________________________________________________________________________________________
	private static LevelManager s_Instance = null;

    // This defines a instance property that attempts to find the manager object in the scene and
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