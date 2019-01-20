using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="SHMUP/LevelReglages")]
public class LevelReglages : ScriptableObject 
{

	[Header("Vitesse du scrolling")]
	[Range(0.1f,10f)]
	public float scrollingSpeed=1f;

    [Space]

    [Header("DDA - Bonus / Malus")]
    [Range(0, 5)]
    public int bonusAmiMisEnBulle;

    [Range(0, 5)]
    public int bonusAmiSauve;

    [Range(-5, 0)]
    public int malusBulleAmiEclatee;

    [Range(-5, 0)]
    public int malusCollisionOursin;

    [Range(-5, 0)]
    public int malusAmiMangeParKraken;

    [Header("DDA - Palliers de score")]
    [Range(0, 30)]
    public int pallierNormal;

    [Range(0, 30)]
    public int pallierDifficile;

    [Range(0, 30)]
    public int maximumPoints;

    [Space]

    [Header("Portions de level design")]
    public List<GameObject> levelPartsEasy;
    public List<GameObject> levelPartsNormal;
    public List<GameObject> levelPartsHard;

    int lastEasyProvided = -1;
    public GameObject GetEasyLevel()
    {
        if (lastEasyProvided != -1)
        {
            int levelToProvide = lastEasyProvided;
            while (levelToProvide == lastEasyProvided)
            {
                levelToProvide = (int)Random.Range(0f, levelPartsEasy.Count);
            }
            return levelPartsEasy[levelToProvide];
        }
        else
        {
            return levelPartsEasy[(int)Random.Range(0f, levelPartsEasy.Count)];
        }
    }

    int lastNormalProvided = -1;
    public GameObject GetNormalLevel()
    {
        if (lastNormalProvided != -1)
        {
            int levelToProvide = lastNormalProvided;
            while (levelToProvide == lastNormalProvided)
            {
                levelToProvide = (int)Random.Range(0f, levelPartsNormal.Count);
            }
            return levelPartsNormal[levelToProvide];
        }
        else
        {
            return levelPartsNormal[(int)Random.Range(0f, levelPartsNormal.Count)];
        }
    }

    int lastHardProvided = -1;
    public GameObject GetHardLevel()
    {
        if (lastHardProvided != -1)
        {
            int levelToProvide = lastHardProvided;
            while (levelToProvide == lastHardProvided)
            {
                levelToProvide = (int)Random.Range(0f, levelPartsHard.Count);
            }
            return levelPartsHard[levelToProvide];
        }
        else
        {
            return levelPartsHard[(int)Random.Range(0f, levelPartsHard.Count)];
        }
    }

    [Space]

    [Header("Plafonds")]
    public List<GameObject> plafonds;
    public List<float> hauteursPlafonds;
}
