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
	[Range(1f,20f)]
	public float speedPlayer=1f;

	[Space]

	[Header("Dash")]
	[Range(1f,20f)]
	public float dashPower=1f;

	[Range(0.01f,2f)]
	public float dashDuration=0.01f;

	[Range(0f,10f)]
	public float dashCoolDown=0f;


	[Space]

	[Header("Knockback du tir de bulle")]
	[Range(0f,7f)]
	public float knockback=0;

}
