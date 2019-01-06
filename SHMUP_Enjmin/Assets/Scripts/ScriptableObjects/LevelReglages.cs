using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="SHMUP/LevelReglages")]
public class LevelReglages : ScriptableObject 
{

	[Header("Vitesse du scrolling")]
	[Range(0.1f,10f)]
	public float scrollingSpeed=1f;


	[Header("Portion de level design")]
	public List<GameObject> levelParts;


	public GameObject GetLevelAtRandom()
	{
		return levelParts[(int)Random.Range(0f,levelParts.Count)];
	}


}
