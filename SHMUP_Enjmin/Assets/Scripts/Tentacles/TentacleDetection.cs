﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleDetection : MonoBehaviour 
{

	[SerializeField]
	Transform tentacles;
	[SerializeField]
	Transform initPosition;
	[SerializeField]
	Transform finalPosition;

	List<GameObject> objectNears;

	bool isDetract=false;

	[SerializeField]
	TentaclesReglages reglages;

	void Start () 
	{
		objectNears=new List<GameObject>();
		tentacles.position=initPosition.position;
	}

	void OnTriggerEnter2D(Collider2D col)
    {
		if((col.gameObject.tag=="ToSave")||(col.gameObject.tag=="Player"))
		{
			if(col.transform.position.x>this.transform.position.x)
			//si entre par la droite, ajoute le a la liste des objets proches
			{
				objectNears.Add(col.gameObject);
				RefreshList();
				if(objectNears.Count==1)
					StartCoroutine(MoveTentacles());
			}
		}
    }

	void OnTriggerExit2D(Collider2D col)
    {
		if((col.gameObject.tag=="ToSave")||(col.gameObject.tag=="Player"))
		{
			objectNears.Remove(col.gameObject);
			RefreshList();
			if(objectNears.Count==0)
			{
				StartCoroutine(MoveTentacles());

			}
		}
    }

	public void RefreshList()
	{
		List<GameObject> removeList=new List<GameObject>();
		for(int i=0;i<objectNears.Count;i++)
		{
			if(objectNears[i]==null)
			{
				removeList.Add(objectNears[i]);
			}
		}
		for(int j=0;j<removeList.Count;j++)
		{
			objectNears.Remove(removeList[j]);
		}
		isDetract=objectNears.Count!=0;
	}

	public IEnumerator MoveTentacles()
	{
		if(isDetract)
		{
			ChangeAnimSpeed(reglages.animSpeedUp);
       		AkSoundEngine.PostEvent("Play_Kraken_Near_Rnd", gameObject);
			while(tentacles.position.x<finalPosition.position.x)
			{
				if(!isDetract)
				{
					RefreshList();
					StartCoroutine(MoveTentacles());
				}
				tentacles.Translate(Time.deltaTime*reglages.speedApparition,0f,0f);
				yield return new WaitForSeconds(0.01f);
			}
		}
		else
		{
			ChangeAnimSpeed(1f);
			while(tentacles.position.x>initPosition.position.x)
			{
				tentacles.Translate(-Time.deltaTime*reglages.speedApparition/20f,0f,0f);
				yield return new WaitForSeconds(0.01f);
			}
		}
	}

	public void Retract()
	{
		RefreshList();
       if(objectNears.Count==0)
	   {
			StopCoroutine(MoveTentacles());
        	StartCoroutine(MoveTentacles());
	   }
	}

	public void ChangeAnimSpeed(float val)
	{
		TentacleAnim[] tentaclesAnim=tentacles.GetComponentsInChildren<TentacleAnim>();
		foreach(TentacleAnim anim in tentaclesAnim)
		{
			anim.SpeedAnimation(val);
		}
	}

	public int GetObjectNearsCount()
	{
		return objectNears.Count;
	}

}
