using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="SHMUP/LevelReglages")]
public class LevelReglages : ScriptableObject 
{

	[Header("Vitesse du scrolling")]
	[Range(0.1f,10f)]
	public float scrollingSpeed=1f;

    [Header("Portions de level design faciles")]
    public List<GameObject> levelPartsEasy;

    [Header("Portions de level design intermédiaires")]
    public List<GameObject> levelPartsNormal;

    [Header("Portions de level design difficiles")]
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

    [Header("Plafonds")]
    public List<GameObject> plafonds;

    [Header("Hauteurs plafonds")]
    public List<float> hauteursPlafonds;
}
