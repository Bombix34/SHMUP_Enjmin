using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {

	public Text scoreText;

	public Text allSavedText;

	public List<GameObject> buttonUI;

	ControllerManager controller;
	KeyboardController keyboard;

	Animator animator;

	bool finishAnimation=false;

	float chronoTemp=0;
	float maxValChrono=0.2f;


	int indexAction=0; 
	//0 rejoué
	//1 menu
	//2 quitter

	void Awake()
	{
		controller=GetComponent<ControllerManager>();
		keyboard=GetComponent<KeyboardController>();
		animator=GetComponent<Animator>();
	}

	void Update()
	{
		if(!finishAnimation)
		{	
			if(controller.pressFireBouton()||controller.pressDashBouton()||keyboard.PressFireBouton()||keyboard.PressDashBouton())
				animator.speed=1.6f;
			return;
		}

		if(chronoTemp>0)
			chronoTemp-=Time.deltaTime;

		if(controller.pressFireBouton()||controller.pressDashBouton()||keyboard.PressFireBouton()||keyboard.PressDashBouton())
			DoAction();


		Vector2 control = controller.getLeftStickDirection();
        if(control==Vector2.zero)
            control=keyboard.GetMovement();
		if((control==Vector2.zero)||(chronoTemp>0))
			return;
		if(control.y>0.05)
			DecrementIndex();
		else if(control.y<-0.05)
			IncrementIndex();
	}

	public void FinishAnim()
	{
		finishAnimation=true;
		buttonUI[indexAction].SetActive(true);
	}

	public void DoAction()
	{
		switch(indexAction)
		{
			case 0 :
				Relaunch();
				break;
			case 1 :
				MainMenu();
				break;
			case 2 :
				QuitGame();
				break;
		}
	}


	public void IncrementIndex()
	{
		buttonUI[indexAction].SetActive(false);
		chronoTemp=maxValChrono;
		indexAction++;
		if(indexAction>2)
			indexAction=0;
		buttonUI[indexAction].SetActive(true);
	}

	public void DecrementIndex()
	{
		buttonUI[indexAction].SetActive(false);
		chronoTemp=maxValChrono;
		indexAction--;
		if(indexAction<0)
			indexAction=2;
		buttonUI[indexAction].SetActive(true);
	}

	public void QuitGame()
	{
		GameManager.instance.QuitGame();
	}

	public void MainMenu()
	{
		GameManager.instance.MainMenu();
	}

	public void Relaunch()
	{
		GameManager.instance.RelaunchGame();
	}
}
