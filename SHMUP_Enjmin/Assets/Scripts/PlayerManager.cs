using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	[SerializeField]
	//les différents réglages du personnage contenue dans le dossier Assets/Reglages/PlayerReglages
	PlayerReglages reglages;

	//controlles a la manette gràce au package InControl ( manette 360, one, ps4, ps3, nvidia... )
	ControllerManager controller;
	Rigidbody2D rb2D;
	CircleCollider2D colider;

	[SerializeField]
	//prefab pour la création des bulles
	GameObject bublePrefab;

	//bulle que le joueur est en train de créer
	GameObject curBuble;

	void Start () 
	{
		colider=GetComponent<CircleCollider2D>();
		controller=GetComponent<ControllerManager>();
		rb2D=GetComponent<Rigidbody2D>();
		transform.localScale=new Vector2(reglages.sizePlayer,reglages.sizePlayer);
	}
	
	void Update () 
	{
		SetPlayerSize();

		//Core mechanics
		MovePlayer();	
		BubleUpdate();
	}

	public void MovePlayer()
	{
		Vector2 controlWithSpeed = controller.getLeftStickDirection()*reglages.speedPlayer;
		rb2D.MovePosition(new Vector2(transform.position.x+controlWithSpeed.x,transform.position.y+controlWithSpeed.y));
	}

//POUR LES TESTS____________________________________________________________________________________

	public void SetPlayerSize()
	{
		//dans le update pour pouvoir tester sans devoir relancer le jeu
		//a supprimer lorsque la taille est définitivement choisie
		transform.localScale=new Vector2(reglages.sizePlayer,reglages.sizePlayer);
	}

//BUBLES SHOOT______________________________________________________________________________________
	
	public void BubleUpdate()
	{
		UpdateCurBublePosition();

		//controlles a la manette
		if(controller.pressButtonA())
			CreateBuble();
		if(controller.usingButtonA())
			GrowBuble();
		if(controller.releasedButtonA())
			ShootBuble();
	}

	public void CreateBuble()
	{
		if((curBuble==null))
		{
			Vector2 bublePosition= new Vector2(transform.position.x+(reglages.initialSize*2f), transform.position.y);
			curBuble = Instantiate(bublePrefab, bublePosition,Quaternion.identity) as GameObject;
			curBuble.transform.localScale=new Vector2(reglages.initialSize,reglages.initialSize);
			curBuble.GetComponent<BubleManager>().GetRigidbody().drag=reglages.velocityDecrease;
			curBuble.GetComponent<BubleManager>().SetIsCreate(true);
		}
	}

	public void GrowBuble()
	{
		if(curBuble==null)
			return;
		if(curBuble.transform.localScale.x>=reglages.maxSizeBuble)
			return;
		curBuble.transform.localScale=new Vector2(curBuble.transform.localScale.x+reglages.speedGrow,curBuble.transform.localScale.y+reglages.speedGrow);
	}

	public void UpdateCurBublePosition()
	{
		//quand je laisse appuyer, il faut que la bulle suive le personnage
		if(curBuble==null)
			return;
		Vector2 newPos = new Vector2(transform.position.x+curBuble.transform.localScale.x+0.5f, transform.position.y);
		curBuble.transform.position=newPos;
	}

	public void ShootBuble()
	{
		if(curBuble==null)
			return;
		curBuble.GetComponent<BubleManager>().SetIsCreate(false);
		//tir de la bulle
		curBuble.GetComponent<Rigidbody2D>().AddForce(new Vector2(100f,0f)*reglages.speedBuble);
		//effet des bulles a remonter vers la surface
		curBuble.GetComponent<Rigidbody2D>().gravityScale=-reglages.archimedEffect;
		//knockback du personnage
		rb2D.AddForce(new Vector2(-100f,0f)*reglages.knockback);
		curBuble=null;
	}

//GETTER & SETTER____________________________________________________________________________________________

	public CircleCollider2D GetColider()
	{
		return colider;
	}
}
