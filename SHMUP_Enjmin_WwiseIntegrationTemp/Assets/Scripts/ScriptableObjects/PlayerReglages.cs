using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName="SHMUP/PlayerReglages")]
public class PlayerReglages : ScriptableObject 
{

	[Header("Taille du personnage")]
	[Range(0.4f,2f)]
	public float sizePlayer=0.4f;

	[Space]

	[Header("Vitesse du personnage")]
	[Range(0.1f,0.4f)]
	public float speedPlayer=0.1f;

	[Space]

	[Header("Knockback du tir de bulle")]
	[Range(0f,20f)]
	public float knockback=0f;

	[Space]

	[Header("Reglages tir de bulles")]
	[Range(0.1f,1f)]
	public float initialSize=0.5f;
	[Range(0.5f,6f)]
	public float maxSizeBuble=0.5f;

	[Range(0.01f,0.1f)]
	[Tooltip("La vitesse a laquelle la bulle grossit")]
	public float speedGrow=0.01f;

	
	[Range(5f,100f)]
	[Tooltip("La vitesse a laquelle la bulle est tirée")]
	public float speedBuble=5f;

	[Range(0.1f,3f)]
	[Tooltip("La vitesse a laquelle la bulle va s'arrêter")]
	public float velocityDecrease=0.4f;

	[Space]

	[Header("Force qui ramène les bulles vers la surface")]
	[Range(0f,0.8f)]
	public float archimedEffect=0f;


}
