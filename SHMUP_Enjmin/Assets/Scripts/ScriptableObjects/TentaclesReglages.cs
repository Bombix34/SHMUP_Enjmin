using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="SHMUP/TentaclesReglages")]
public class TentaclesReglages : ScriptableObject {

	[Range(1f,10f)]
	public float speedApparition=1f;

	[Range(1f,10f)]
	public float animSpeedUp=1f;
}
