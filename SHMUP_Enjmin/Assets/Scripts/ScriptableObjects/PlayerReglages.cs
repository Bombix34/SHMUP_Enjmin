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

	[Range(0f,100f)]
	public float dashKnockbackBuble=0f;




	[Space]

	[Header("Knockback du tir de bulle")]
	[Range(0f,7f)]
	public float knockback=0;

	[Space]

	[Header("Effets divers")]
	[Range(0.1f,10f)]
	[Tooltip("plus elle est proche de 0 et plus le poison est lent a partir")]
	public float oursinPoisonEffect=0.1f;

}
