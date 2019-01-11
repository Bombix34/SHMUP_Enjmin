using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleAnim : MonoBehaviour 
{
	Animator anim;

	public string animName;

	void Start () 
	{
		anim= GetComponent<Animator>();
		anim.Play(animName, 0, Random.Range(0f,1f));
	}

	public void SpeedAnimation(float val)
	{
		anim.speed=val;	
		Debug.Log(anim.speed);
	}

}
