using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour 
{
	public bool PressFireBouton()
	{
		return Input.GetButtonDown("Fire1");
	}
	public bool UseFireBouton()
	{
		return Input.GetButton("Fire1");
	}
	public bool ReleaseFireBouton()
	{
		return Input.GetButtonUp("Fire1");
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
