using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	[SerializeField]
	//les différents réglages du personnage contenue dans le dossier Assets/Reglages/PlayerReglages
	PlayerReglages reglages;
	
	//pour le poison notamment
	float tempSpeedValue;

	[SerializeField]
	BubleReglages bullesReglages;

	//controlles a la manette gràce au package InControl ( manette 360, one, ps4, ps3, nvidia... )
	ControllerManager controller;
	KeyboardController keyboard;
	Rigidbody2D rb2D;
	CapsuleCollider2D colider;

	[SerializeField]
	SpriteRenderer sprite;

	[SerializeField]
	//prefab pour la création des bulles
	GameObject bublePrefab;

	//bulle que le joueur est en train de créer
	GameObject curBuble;
	//chrono pour GrowBuble(), attendre avant de grossir la bulle
	float chronoIncrementSizeBuble=1;

	bool isDashing=false;

	//cooldown du dash
	float dashChrono=0f;
	bool canDash=true;
	bool canMove=true;

	bool isDead=false;
    
    // rtpc value
    float rtpcValue = 0.0f;

    private void Awake()
    {
        if(reglages == null)
        {
            Debug.LogError("error PlayerManager : PlayerReglages not instanciated through editor");
        }
		tempSpeedValue=reglages.speedPlayer;
    }

    void Start () 
	{
		colider=GetComponent<CapsuleCollider2D>();
		controller=GetComponent<ControllerManager>();
		keyboard=GetComponent<KeyboardController>();
		rb2D=GetComponent<Rigidbody2D>();
		transform.localScale=new Vector2(reglages.sizePlayer,reglages.sizePlayer);

        AkSoundEngine.SetState("Game_State", "ReadyToDestroyBuble");

    }

    void Update () 
	{
		if(isDead)
		{
			if(controller.pressAnyButton()||keyboard.PressFireBouton()||keyboard.PressDashBouton())
				GameManager.instance.RelaunchGame();
			return;
		}

		SetPlayerSize();

		//Core mechanics
		UpdateTempSpeed();
		MovePlayer();	

		BubleUpdate();
		Dash();
		
        rtpcValue = transform.position.y;

        AkSoundEngine.SetRTPCValue("Profondeur", rtpcValue, gameObject);
	}

	public void Die()
	{
		isDead=true;
		if(curBuble!=null)
			curBuble.GetComponent<BubleManager>().DestroyBuble();
	}

	IEnumerator Damaged()
	{
		if(Camera.main.GetComponent<CameraShaker>()!=null)
			Camera.main.GetComponent<CameraShaker>().LaunchShake(0.3f,0.2f);
		sprite.enabled=false;
		yield return new WaitForSeconds(0.1f);
		sprite.enabled=true;
	}


//MOVEMENT________________________________________________________________________________
	public void MovePlayer()
	{
		if((!canMove)||(isDashing))
			return;
		Vector2 controlWithSpeed = controller.getLeftStickDirection()*tempSpeedValue;
		if(controlWithSpeed==Vector2.zero)
			controlWithSpeed=keyboard.GetMovement()*tempSpeedValue;
       // transform.Translate(new Vector2(controlWithSpeed.x, controlWithSpeed.y));
	   rb2D.velocity=new Vector2(controlWithSpeed.x,controlWithSpeed.y);
		//rb2D.MovePosition(new Vector2(transform.position.x+controlWithSpeed.x,transform.position.y+controlWithSpeed.y));
    }

	public void UpdateTempSpeed()
	//pour le poison de l'oursin
	{
		if(tempSpeedValue==reglages.speedPlayer)
			return;

		if(tempSpeedValue<reglages.speedPlayer)
		{
			canDash=false;
			sprite.color=Color.green;
			tempSpeedValue+=Time.deltaTime*reglages.oursinPoisonEffect;

            AkSoundEngine.SetState("Game_State", "Poisonus");
        }
		else
		{
			canDash=true;
			sprite.color=Color.white;
			tempSpeedValue=reglages.speedPlayer;

            AkSoundEngine.SetState("Game_State", "ReadyToDestroyBuble");

        }
    }

//DASH__________________________________________________________________________________________

	public void Dash()
	{
		dashChrono-=Time.deltaTime;
		if((dashChrono>0)||(!canDash))
			return;
		if(controller.pressButtonB()||keyboard.PressDashBouton())
		{
			if(curBuble!=null)
			{	
				BubleManager tempBuble = curBuble.GetComponent<BubleManager>();
				tempBuble.DestroyBuble();
			}
			StartCoroutine(DashAction());
            AkSoundEngine.PostEvent("Play_Player_Dash_os", gameObject);
        }
	}

	IEnumerator DashAction()
	{
		isDashing=true;

		Vector2 tempDirection = rb2D.velocity;
		tempDirection.Normalize();

		if(tempDirection==Vector2.zero)
			rb2D.velocity=new Vector2(reglages.speedPlayer,0f)*reglages.dashPower;
		else	
			rb2D.velocity=new Vector2(tempDirection.x*reglages.speedPlayer,tempDirection.y*reglages.speedPlayer)*reglages.dashPower;
		
		//attendre la fin du dash
		float dash=reglages.dashDuration;
		while(dash>0)
		{
			dash-=Time.deltaTime;
			yield return new WaitForSeconds(0.001f);
		}
		//_______________________
		isDashing=false;
		rb2D.velocity=Vector2.zero;
		dashChrono=reglages.dashCoolDown;
	}

//COLLISIONS____________________________________________________________________________________

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Buble")
        {
			if(isDashing)
			//pousser la bulle super fort
			{
				Vector2 forceDirection = new Vector2(col.transform.position.x-this.transform.position.x,col.transform.position.y-this.transform.position.y);
				forceDirection.Normalize();
				forceDirection*=200f;
				col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceDirection.x,forceDirection.y)*reglages.dashKnockbackBuble);
			}
        }
		else if(col.gameObject.tag=="oursin")
		{
			tempSpeedValue=0f;
			canDash=false;
			Vector2 forceDirection = new Vector2(this.transform.position.x-col.transform.position.x,this.transform.position.y-col.transform.position.y);
			forceDirection.Normalize();
			forceDirection*=30f;
			StartCoroutine(KnockbackPlayer(forceDirection));
			StartCoroutine(Damaged());
			col.gameObject.GetComponent<UrchinManager>().retract();
		}
    }

	void OnCollisionStay2D(Collision2D col)
    {
		if (col.gameObject.tag == "Buble")
       	{
			if(isDashing)
			//pousser la bulle super fort
			{
				Vector2 forceDirection = new Vector2(col.gameObject.transform.position.x-this.transform.position.x,col.gameObject.transform.position.y-this.transform.position.y);
				forceDirection*=200f;
				col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceDirection.x,forceDirection.y)*reglages.dashKnockbackBuble);
                print("dashbuble");
                AkSoundEngine.PostEvent("Play_Impact_Dash_bubble", gameObject);
			}
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "niktemor")
        {
            AkSoundEngine.SetState("Profondeur", "Lvl_01");
        } else if (collision.gameObject.tag == "niktarass")
        {
            AkSoundEngine.SetState("Profondeur", "Lvl_02");
        }
        else if (collision.gameObject.tag == "niktonper")
        {
            AkSoundEngine.SetState("Profondeur", "Lvl_03");
        }
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
		if(controller.pressButtonA()||keyboard.PressFireBouton())
			CreateBuble();
		if(controller.usingButtonA()||keyboard.UseFireBouton())
			GrowBuble();
		if(controller.releasedButtonA()||keyboard.ReleaseFireBouton())
			ShootBuble();
	}

	public void CreateBuble()
	{
		if((curBuble==null)&&(!isDashing))
		{
			chronoIncrementSizeBuble=1f;
			Vector2 bublePosition= new Vector2(transform.position.x + transform.localScale.x +(bullesReglages.initialSize), transform.position.y);
			curBuble = Instantiate(bublePrefab, bublePosition,Quaternion.identity) as GameObject;
			curBuble.GetComponent<BubleManager>().SetIsCreate(true);
			curBuble.transform.localScale=new Vector2(bullesReglages.initialSize,bullesReglages.initialSize);
			curBuble.GetComponent<BubleManager>().GetRigidbody().drag=bullesReglages.velocityDecrease;

			Physics2D.IgnoreCollision(colider,curBuble.GetComponent<BubleManager>().GetCollider(),true);
		}
	}

	public void GrowBuble()
	{
		if(curBuble==null)
			return;
		chronoIncrementSizeBuble-=bullesReglages.speedGrow;
		if(chronoIncrementSizeBuble<=0)
		{
			StartCoroutine(ModifyBubleSize());
		}
		else //shake de la bulle pendant la création
			StartCoroutine(curBuble.GetComponent<BubleManager>().ShakeBuble());
		//OLD VERSION GROW : 
		//curBuble.transform.localScale=new Vector2(curBuble.transform.localScale.x+bullesReglages.speedGrow,curBuble.transform.localScale.y+bullesReglages.speedGrow);

        AkSoundEngine.PostEvent("Play_Load_Shot", gameObject);
    }

	IEnumerator ModifyBubleSize()
	//permet de faire une petite animation lorsque la bulle grossie
	{
		BubleManager tempBuble = curBuble.GetComponent<BubleManager>();
		if(tempBuble.transform.localScale.x==bullesReglages.initialSize)
		{	
			tempBuble.IncrementBubleSize();
			tempBuble.transform.localScale=new Vector2(bullesReglages.intermediateSize+0.15f,bullesReglages.intermediateSize+0.15f);
			UpdateCurBublePosition();
			yield return new WaitForSeconds(0.05f);
			//if(curBuble!=null)
			tempBuble.transform.localScale=new Vector2(bullesReglages.intermediateSize,bullesReglages.intermediateSize);
		}
		else if(tempBuble.transform.localScale.x==bullesReglages.intermediateSize)
		{
			tempBuble.GetComponent<BubleManager>().IncrementBubleSize();
			tempBuble.transform.localScale=new Vector2(bullesReglages.maxSizeBuble+0.15f,bullesReglages.maxSizeBuble+0.15f);
			UpdateCurBublePosition();
			yield return new WaitForSeconds(0.05f);
			//if(curBuble!=null)
			tempBuble.transform.localScale=new Vector2(bullesReglages.maxSizeBuble,bullesReglages.maxSizeBuble);
			ShootBuble();
		}
		chronoIncrementSizeBuble=1f;
	}

	public void UpdateCurBublePosition()
	{
		//quand je laisse appuyer, il faut que la bulle suive le personnage
		if(curBuble==null)
			return;
		Vector2 newPos = new Vector2(transform.position.x + transform.localScale.x + (curBuble.transform.localScale.x*0.8f) , transform.position.y);
		curBuble.transform.position=newPos;
	}

	public void ShootBuble()
	{
		if(curBuble==null)
			return;

		curBuble.GetComponent<BubleManager>().SetIsCreate(false);
		//tir de la bulle

		curBuble.GetComponent<Rigidbody2D>().AddForce(new Vector2(500f,0f)*bullesReglages.speedBuble);
		curBuble.GetComponent<BubleManager>().GetBubleAnim().SetTrigger("Shoot");

        AkSoundEngine.PostEvent("Play_Player_Shot", gameObject);
       
        //effet des bulles a remonter vers la surface
        curBuble.GetComponent<Rigidbody2D>().gravityScale=-bullesReglages.archimedEffect;

		Physics2D.IgnoreCollision(colider,curBuble.GetComponent<BubleManager>().GetCollider(),false);

		//knockback du personnage
		StartCoroutine(KnockbackPlayer(new Vector2(-30f,0f)));

		curBuble=null;
	}

	public void DetachBuble()
	//pour detacher une bulle sans la tirer 
	{
		if(curBuble==null)
			return;

		//Hugo : j'ai ajouté ça, je ne savais pas si un event special allait être crée 
        AkSoundEngine.PostEvent("Play_Player_Shot", gameObject);

		curBuble.GetComponent<BubleManager>().SetIsCreate(false);
        //effet des bulles a remonter vers la surface
        curBuble.GetComponent<Rigidbody2D>().gravityScale=-bullesReglages.archimedEffect;

		Physics2D.IgnoreCollision(colider,curBuble.GetComponent<BubleManager>().GetCollider(),false);
		curBuble=null;
	}

	public IEnumerator KnockbackPlayer(Vector2 direction)
	{
		//pour empêcher le joueur de bouger pendant le knockback
		canMove=false;
		canDash=false;
		for(int i= 0;i<10;i++)
		{
			Vector2 final = new Vector2(direction.x*Time.deltaTime,direction.y*Time.deltaTime)*reglages.knockback;
			//deceleration de la velocité
			final*=(0.1f*(10-i));
			transform.Translate(final);
			yield return new WaitForSeconds(0.001f);
		}
		canMove=true;
		canDash=true;
	}

//GETTER & SETTER____________________________________________________________________________________________

	public CapsuleCollider2D GetColider()
	{
		return colider;
	}
}
