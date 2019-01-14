using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedManager : MonoBehaviour {

	bool isInBuble=false;

	[SerializeField]
	GameObject auraPoulpe;

	Animator anim;

	[SerializeField]
	SpriteRenderer sprite;

	void Start()
	{
		//sprite = GetComponentInChildren<SpriteRenderer>();
		SetInitColor();
		anim=GetComponent<Animator>();
	}

	public void SetIsInBuble(bool val)
	{
		isInBuble=val;
		auraPoulpe.SetActive(!isInBuble);
	}

	public bool GetIsInbuble()
	{
		return isInBuble;
	}

	public void SetInitColor()
	{
		sprite.color=new Color32((byte)Random.Range(200f,255f),(byte)Random.Range(200f,255f),(byte)Random.Range(200f,255f),255);
	}

	public void EnterBuble(bool isEntering)
	{
		if(isEntering)
			anim.SetTrigger("enter");
		else
			anim.SetTrigger("exit");
	}
}
