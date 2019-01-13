using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimRand : MonoBehaviour {

	Animator animator;

	string animName="algue";

	void Start () 
	{
		animator=GetComponent<Animator>();
		animator.speed=0f;
		StartCoroutine(StartAnimation());
	}

	IEnumerator StartAnimation()
	{
		float randTime=Random.Range(0f,2f);
		yield return new WaitForSeconds(randTime);
		animator.speed=Random.Range(0.9f,1.1f);
		animator.Play(animName);
	}
}
