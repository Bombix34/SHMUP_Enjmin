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
	[Range(0f,7f)]
	public float knockback=0;

}
