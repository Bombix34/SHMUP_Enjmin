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

    [Space]

    [Header("Portions de level design")]
    public List<GameObject> levelPartsEasy;
    public List<GameObject> levelPartsNormal;
    public List<GameObject> levelPartsHard;

    public GameObject GetEasyLevel()
    {
        return levelPartsEasy[(int)Random.Range(0f, levelPartsEasy.Count)];
    }

    public GameObject GetNormalLevel()
    {
        return levelPartsNormal[(int)Random.Range(0f, levelPartsNormal.Count)];
    }

    public GameObject GetHardLevel()
    {
        return levelPartsHard[(int)Random.Range(0f, levelPartsHard.Count)];
    }

    [Space]

    [Header("Plafonds")]
    public List<GameObject> plafonds;
    public List<float> hauteursPlafonds;
}
