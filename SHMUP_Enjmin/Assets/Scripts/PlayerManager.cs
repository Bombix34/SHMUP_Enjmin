using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	[SerializeField]
	//les différents réglages du personnage contenue dans le dossier Assets/Reglages/PlayerReglages
	PlayerReglages reglages;

	[SerializeField]
	BubleReglages bullesReglages;

	//controlles a la manette gràce au package InControl ( manette 360, one, ps4, ps3, nvidia... )
	ControllerManager controller;
	KeyboardController keyboard;
	Rigidbody2D rb2D;
	CapsuleCollider2D colider;

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
	bool canMove=true;


    
    // rtpc value
    float rtpcValue;
    int type = 1;

    private void Awake()
    {
        if(reglages == null)
        {
            Debug.LogError("error PlayerManager : PlayerReglages not instanciated through editor");
        }
    }

    void Start () 
	{
		colider=GetComponent<CapsuleCollider2D>();
		controller=GetComponent<ControllerManager>();
		keyboard=GetComponent<KeyboardController>();
		rb2D=GetComponent<Rigidbody2D>();
		transform.localScale=new Vector2(reglages.sizePlayer,reglages.sizePlayer);

        AkSoundEngine.GetRTPCValue("Profondeur", gameObject, 0, out rtpcValue, ref type);
    }
	
	void Update () 
	{
		SetPlayerSize();

		//Core mechanics
		MovePlayer();	
		BubleUpdate();
		Dash();
		
        rtpcValue = transform.position.y;
        AkSoundEngine.SetRTPCValue("Profondeur", rtpcValue, gameObject);

	}


//MOVEMENT________________________________________________________________________________
	public void MovePlayer()
	{
		if((!canMove)||(isDashing))
			return;
		Vector2 controlWithSpeed = controller.getLeftStickDirection()*reglages.speedPlayer;
		if(controlWithSpeed==Vector2.zero)
			controlWithSpeed=keyboard.GetMovement()*reglages.speedPlayer;
       // transform.Translate(new Vector2(controlWithSpeed.x, controlWithSpeed.y));
	   rb2D.velocity=new Vector2(controlWithSpeed.x,controlWithSpeed.y);
		//rb2D.MovePosition(new Vector2(transform.position.x+controlWithSpeed.x,transform.position.y+controlWithSpeed.y));
    }

//DASH__________________________________________________________________________________________

	public void Dash()
	{
		dashChrono-=Time.deltaTime;
		if(dashChrono>0)
			return;
		if(controller.pressButtonB())
		{
			if(curBuble!=null)
				curBuble.GetComponent<BubleManager>().DestroyBuble();
			StartCoroutine(DashAction());
		}
	}

	IEnumerator DashAction()
	{
		isDashing=true;

		if(rb2D.velocity==Vector2.zero)
			rb2D.velocity=new Vector2(reglages.speedPlayer,0f)*reglages.dashPower;
		else	
			rb2D.velocity*=reglages.dashPower;
		
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
				Vector2 forceDirection = new Vector2(col.gameObject.transform.position.x-this.transform.position.x,col.gameObject.transform.position.y-this.transform.position.y);
				forceDirection*=200f;
				col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceDirection.x,forceDirection.y)*bullesReglages.speedBuble);
			}
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "niktemor")
        {
            print("1");
            AkSoundEngine.SetState("Profondeur", "Lvl_01");
        } else if (collision.gameObject.tag == "niktarass")
        {
            print("2");
            AkSoundEngine.SetState("Profondeur", "Lvl_02");
        }
        else if (collision.gameObject.tag == "niktonper")
        {
            print("3");
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

       // AkSoundEngine.PostEvent("Play_Load_Shot", gameObject);
    }

	IEnumerator ModifyBubleSize()
	//permet de faire une petite animation lorsque la bulle grossie
	{
		if(curBuble.transform.localScale.x==bullesReglages.initialSize)
		{	
			curBuble.GetComponent<BubleManager>().IncrementBubleSize();
			curBuble.transform.localScale=new Vector2(bullesReglages.intermediateSize+0.15f,bullesReglages.intermediateSize+0.15f);
			UpdateCurBublePosition();
			yield return new WaitForSeconds(0.05f);
			curBuble.transform.localScale=new Vector2(bullesReglages.intermediateSize,bullesReglages.intermediateSize);
		}
		else if(curBuble.transform.localScale.x==bullesReglages.intermediateSize)
		{
			curBuble.GetComponent<BubleManager>().IncrementBubleSize();
			curBuble.transform.localScale=new Vector2(bullesReglages.maxSizeBuble+0.15f,bullesReglages.maxSizeBuble+0.15f);
			UpdateCurBublePosition();
			yield return new WaitForSeconds(0.05f);
			curBuble.transform.localScale=new Vector2(bullesReglages.maxSizeBuble,bullesReglages.maxSizeBuble);
			ShootBuble();
		}
		chronoIncrementSizeBuble=1f;
	}

	public void UpdateCurBublePosition()
	{
		//quand je laisse appuyer, il faut que la bulle suive le personnage
		if(curBuble==null)
			return;
		Vector2 newPos = new Vector2(transform.position.x + transform.localScale.x + (curBuble.transform.localScale.x*0.9f) , transform.position.y);
		curBuble.transform.position=newPos;
	}

	public void ShootBuble()
	{
		if(curBuble==null)
			return;

		curBuble.GetComponent<BubleManager>().SetIsCreate(false);
		//tir de la bulle

		curBuble.GetComponent<Rigidbody2D>().AddForce(new Vector2(500f,0f)*bullesReglages.speedBuble);

       // AkSoundEngine.PostEvent("Play_Player_Shot", gameObject);
       
        //effet des bulles a remonter vers la surface
        curBuble.GetComponent<Rigidbody2D>().gravityScale=-bullesReglages.archimedEffect;

		Physics2D.IgnoreCollision(colider,curBuble.GetComponent<BubleManager>().GetCollider(),false);

		//knockback du personnage
		StartCoroutine(KnockbackPlayer(new Vector2(-30f,0f)));

		curBuble=null;
	}

	public IEnumerator KnockbackPlayer(Vector2 direction)
	{
		//pour empêcher le joueur de bouger pendant le knockback
		canMove=false;
		for(int i= 0;i<10;i++)
		{
			Vector2 final = new Vector2(direction.x*Time.deltaTime,direction.y*Time.deltaTime)*reglages.knockback;
			//deceleration de la velocité
			final*=(0.1f*(10-i));
			transform.Translate(final);
			yield return new WaitForSeconds(0.001f);
		}
		canMove=true;
	}

//GETTER & SETTER____________________________________________________________________________________________

	public CapsuleCollider2D GetColider()
	{
		return colider;
	}
}
