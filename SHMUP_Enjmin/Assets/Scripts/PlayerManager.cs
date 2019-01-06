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
    
    // rtpc value
    float rtpcValue;
    int type = 1;

	bool isDashing=false;
	bool canMove=true;

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
		//SetPlayerSize();

		//Core mechanics
		MovePlayer();	
		BubleUpdate();

        rtpcValue = transform.position.y;
        AkSoundEngine.SetRTPCValue("Profondeur", rtpcValue, gameObject);
	}

	public void MovePlayer()
	{
		if(!canMove)
			return;
		Vector2 controlWithSpeed = controller.getLeftStickDirection()*reglages.speedPlayer;
		if(controlWithSpeed==Vector2.zero)
			controlWithSpeed=keyboard.GetMovement()*reglages.speedPlayer;
       // transform.Translate(new Vector2(controlWithSpeed.x, controlWithSpeed.y));
		rb2D.MovePosition(new Vector2(transform.position.x+controlWithSpeed.x,transform.position.y+controlWithSpeed.y));
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "ToSave")
        {
            Physics2D.IgnoreCollision(colider, col.collider);
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
		if((curBuble==null))
		{
			Vector2 bublePosition= new Vector2(transform.position.x + transform.localScale.x +(bullesReglages.initialSize), transform.position.y);
			curBuble = Instantiate(bublePrefab, bublePosition,Quaternion.identity) as GameObject;
			curBuble.GetComponent<BubleManager>().SetIsCreate(true);
			curBuble.transform.localScale=new Vector2(bullesReglages.initialSize,bullesReglages.initialSize);
			curBuble.GetComponent<BubleManager>().GetRigidbody().drag=bullesReglages.velocityDecrease;
		}
	}

	public void GrowBuble()
	{
		if(curBuble==null)
			return;
		if(curBuble.transform.localScale.x>=bullesReglages.maxSizeBuble)
			return;
		curBuble.transform.localScale=new Vector2(curBuble.transform.localScale.x+bullesReglages.speedGrow,curBuble.transform.localScale.y+bullesReglages.speedGrow);

		Physics2D.IgnoreCollision(colider,curBuble.GetComponent<BubleManager>().GetCollider(),true);

       // AkSoundEngine.PostEvent("Play_Load_Shot", gameObject);
    }

	public void UpdateCurBublePosition()
	{
		//quand je laisse appuyer, il faut que la bulle suive le personnage
		if(curBuble==null)
			return;
		Vector2 newPos = new Vector2(transform.position.x+curBuble.transform.localScale.x + 1.3f , transform.position.y);
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
		StopCoroutine(KnockbackPlayer(direction));
	}

//GETTER & SETTER____________________________________________________________________________________________

	public CapsuleCollider2D GetColider()
	{
		return colider;
	}
}
