using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedManager : MonoBehaviour {

	bool isInBuble=false;

	[SerializeField]
	GameObject auraPoulpe;

	public void SetIsInBuble(bool val)
	{
		isInBuble=val;
		auraPoulpe.SetActive(!isInBuble);
	}

	public bool GetIsInbuble()
	{
		return isInBuble;
	}
}
