using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour 
{
	public bool PressFireBouton()
	{
		return Input.GetButtonDown("Bubble");
	}
	public bool UseFireBouton()
	{
		return Input.GetButton("Bubble");
	}
	public bool ReleaseFireBouton()
	{
		return Input.GetButtonUp("Bubble");
	}
	public Vector2 GetMovement()
	{
		Vector2 keyboardAxis = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
		return keyboardAxis;
	}

	public bool PressDashBouton()
	{
		return Input.GetButtonDown("Dash");
	}
}
