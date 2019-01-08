using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedManager : MonoBehaviour {

	bool isInBuble=false;

	public void SetIsInBuble(bool val)
	{
		isInBuble=val;
	}

	public bool GetIsInbuble()
	{
		return isInBuble;
	}
}
