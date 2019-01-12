using System;
using UnityEngine;
using InControl;

public class ControllerManager : MonoBehaviour {
//stick gauche : getLeftStickDirection()
//stick gauche + d-pad = getLeftStickDPad()
//stick droit : getRightStickDirection()
//pressButton+nom du bouton() = quand le bouton est appuye
//releasedButton+nom du bouton() = quand le bouton est relaché
//usingButton+nom du bouton() = detection en continue de l'appuie du bouton

	[SerializeField]
	int playerNum;

	InputDevice inputDevice;

	#pragma warning disable 0414 // private field assigned but not used.
	InputControl control;

	public int PlayerNum{
		get
		{
			return playerNum;
		}
		set
		{
			playerNum=value;
		}
	}

	bool RefreshController(){
		inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices[playerNum] : null;
		if(inputDevice==null)
			return false;
		else{
			control = inputDevice.GetControl( InputControlType.Action1 );
			return true;
		}
	}

	public Vector2 getLeftStickDirection( )
	{
		if(!RefreshController())
			return new Vector2(0f,0f);
		//return new Vector2(inputDevice.Direction.X,inputDevice.Direction.Y);
		return new Vector2(inputDevice.LeftStickX, inputDevice.LeftStickY);
	}

	public Vector2 getRightStickDirection( )
	{
		if(!RefreshController())
			return new Vector2(0f,0f);
		//return new Vector2(inputDevice.Direction.X,inputDevice.Direction.Y);
		return new Vector2(inputDevice.RightStickX, inputDevice.RightStickY);
	}

	public Vector2 getLeftStickDPad()
	{
		//left joystick and D-Pad
		if(!RefreshController())
			return new Vector2(0f,0f);
		return new Vector2(inputDevice.Direction.X,inputDevice.Direction.Y);
	}

//SHMUP_____________________________________________

	public bool pressFireBouton()
	{
		if(pressButtonA()||pressButtonR2()||pressButtonR1())
			return true;
		return false;
	}

	public bool useFireBouton()
	{
		if(usingButtonA()||usingButtonR2()||usingButtonR1())
			return true;
		return false;
	}

	public bool releaseFireBouton()
	{
		if(releasedButtonA()||releasedButtonR2()||releasedButtonR1())
			return true;
		return false;
	}

	public bool pressDashBouton()
	{
		if(pressButtonB()||pressButtonX())
			return true;
		return false;
	}

//DETECTION PRESSION
	public bool pressAnyButton()
	{
		if(!RefreshController())
			return false;
		if(inputDevice.AnyButton.WasPressed)
			return true;
		return false;
	}
	public bool pressButtonA()
	{
		if(!RefreshController())
			return false;
		if (inputDevice.Action1.WasPressed)
			//A
			return true;
		return false;
	}
	
	public bool pressButtonB()
	{
		if(!RefreshController())
			return false;
		if (inputDevice.Action2.WasPressed)
			//B
			return true;
		return false;
	}
	public bool pressButtonX()
	{
		if(!RefreshController())
			return false;
		if (inputDevice.Action3.WasPressed)
		//X
			return true;	
		return false;
	}

	public bool pressButtonY()
	{
		if(!RefreshController())
			return false;
		if (inputDevice.Action4.WasPressed)
			//Y
			return true;
		return false;
	}
	public bool pressButtonR1()
	{
		if(!RefreshController())
			return false;;
		if(inputDevice.RightBumper.WasPressed)
			//R1
			return true;
		return false;	
	}
	public bool pressButtonR2()
	{
		if(!RefreshController())
			return false;;
		if(inputDevice.RightTrigger.WasPressed)
			//R2
			return true;
		return false;	
	}
	public bool pressButtonL1()
	{
		if(!RefreshController())
			return false;;
		if(inputDevice.LeftBumper.WasPressed)
			//L1
			return true;
		return false;	
	}
	public bool pressButtonL2()
	{
		if(!RefreshController())
			return false;;
		if(inputDevice.LeftTrigger.WasPressed)
			//L2
			return true;
		return false;	
	}
//DETECTION RELEASE
	public bool releasedAnyButton()
	{
		if(inputDevice.AnyButton.WasReleased)
			return true;
		return false;
	}
	public bool releasedButtonA()
	{
		if(!RefreshController())
			return false;
		if (inputDevice.Action1.WasReleased)
			//A
			return true;
		return false;
	}
	public bool releasedButtonB()
	{
		if(!RefreshController())
			return false;
		if (inputDevice.Action2.WasReleased)
			//B
			return true;
		return false;
	}
	public bool releasedButtonX()
	{
		if(!RefreshController())
			return false;
		if (inputDevice.Action3.WasReleased)
		//X
			return true;	
		return false;
	}

	public bool releasedButtonY()
	{
		if(!RefreshController())
			return false;
		if (inputDevice.Action4.WasReleased)
			//Y
			return true;
		return false;
	}
	public bool releasedButtonR1()
	{
		if(!RefreshController())
			return false;;
		if(inputDevice.RightBumper.WasReleased)
			//R1
			return true;
		return false;	
	}
	public bool releasedButtonR2()
	{
		if(!RefreshController())
			return false;;
		if(inputDevice.RightTrigger.WasReleased)
			//R2
			return true;
		return false;	
	}
	public bool releasedButtonL1()
	{
		if(!RefreshController())
			return false;;
		if(inputDevice.LeftBumper.WasReleased)
			//L1
			return true;
		return false;	
	}
	public bool releasedButtonL2()
	{
		if(!RefreshController())
			return false;;
		if(inputDevice.LeftTrigger.WasReleased)
			//L2
			return true;
		return false;	
	}

//DETECTION CONTINU
		public bool usingButtonA()
		{
			if(!RefreshController())
				return false;
			if (inputDevice.Action1)
				//A
				return true;
			return false;
		}
		public bool usingButtonB(){
			if(!RefreshController())
				return false;
			if (inputDevice.Action2)
				//B
				return true;
			else	
				return false;
		}
		public bool usingButtonX(){
			if(!RefreshController())
				return false;
			if (inputDevice.Action3)
				//X
				return true;	
			return false;
		}
		public bool usingButtonY(){
			if(!RefreshController())
				return false;
			if (inputDevice.Action4)
				//Y
				return true;
			return false;
		}
		public bool usingButtonR1(){
			if(!RefreshController())
				return false;;
			if(inputDevice.RightBumper)
				//R1
				return true;
			return false;	
		}
		public bool usingButtonR2(){
			if(!RefreshController())
				return false;;
			if(inputDevice.RightTrigger)
				//R2
				return true;
			return false;	
		}
		public bool usingButtonL1(){
			if(!RefreshController())
				return false;;
			if(inputDevice.LeftBumper)
				//L1
				return true;
			return false;	
		}
		public bool usingButtonL2(){
			if(!RefreshController())
				return false;;
			if(inputDevice.LeftTrigger)
				//L2
				return true;
			return false;	
		}
}
