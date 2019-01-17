using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjMainMenu : MonoBehaviour {

	bool isInBuble=false;

	public bool GetIsInBuble()
	{
		return isInBuble;
	}

	public void SetInsBuble(bool val)
	{
		isInBuble=val;

		//pour enlever l'onde autour du petit poulpe sur le menu 
		if(GetComponent<SavedManager>()!=null)
			GetComponent<SavedManager>().SetIsInBuble(true);
	}
}
