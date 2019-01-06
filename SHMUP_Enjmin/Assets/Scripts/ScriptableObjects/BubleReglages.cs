using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="SHMUP/BulleReglages")]
public class BubleReglages : ScriptableObject {

	[Header("Reglages taille des bulles")]
	[Range(0.1f,1f)]
	public float initialSize=0.5f;
	[Range(0.5f,6f)]
	public float maxSizeBuble=0.5f;

	[Space]

	[Header("Reglages tir de bulles")]
	[Range(0.01f,0.1f)]
	[Tooltip("La vitesse a laquelle la bulle grossit")]
	public float speedGrow=0.01f;

	[Range(5f,100f)]
	[Tooltip("La vitesse a laquelle la bulle est tirée")]
	public float speedBuble=5f;

	[Space]

	[Header("Forces")]
	[Range(0.1f,3f)]
	[Tooltip("La vitesse a laquelle la bulle va s'arrêter")]
	public float velocityDecrease=0.4f;


	[Range(0f,0.8f)]
	[Tooltip("La vitesse a laquelle la bulle va remonter à la surface")]
	public float archimedEffect=0f;

}
