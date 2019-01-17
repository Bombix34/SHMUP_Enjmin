using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

	[SerializeField]
	//les différents réglages du personnage contenue dans le dossier Assets/Reglages/PlayerReglages
	PlayerReglages reglages;
	
	//pour le poison notamment
	float tempSpeedValue;

	Vector2 controlWithSpeed= new Vector2(0f,0f);

	[SerializeField]
	BubleReglages bullesReglages;

	//controlles a la manette gràce au package InControl ( manette 360, one, ps4, ps3, nvidia... )
	ControllerManager controller;
	KeyboardController keyboard;
	Rigidbody2D rb2D;
	CapsuleCollider2D colider;
	
	Animator animator;

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

	int nbBullesTirées=0;
    
    // rtpc value
    float rtpcValue = 0.0f;

	PlayerParticles particles;

    Vector2 targetAngle = Vector2.right;

    bool firstBubble = false;
    bool firstDash = false;

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
		particles=GetComponent<PlayerParticles>();
		colider=GetComponent<CapsuleCollider2D>();
		controller=GetComponent<ControllerManager>();
		keyboard=GetComponent<KeyboardController>();
		rb2D=GetComponent<Rigidbody2D>();
		animator=GetComponent<Animator>();
		transform.localScale=new Vector2(reglages.sizePlayer,reglages.sizePlayer);

        AkSoundEngine.SetState("Game_State", "ReadyToDestroyBuble");
    }

    void Update () 
	{
		if(isDead)
			return;
		

		SetPlayerSize();

		//Core mechanics
		UpdateTempSpeed();
		MovePlayer();	

		BubleUpdate();
		Dash();
        
        transform.rotation = Quaternion.Lerp(sprite.transform.rotation, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, targetAngle)), Time.deltaTime * 12);

        sprite.flipY = (targetAngle.x < 0.0f);
		
        rtpcValue = transform.position.y;

        AkSoundEngine.SetRTPCValue("Profondeur", rtpcValue, gameObject);

        
	}

//DAMAGE_________________________________________________________________________________________________________
	public void Die()
	{
		isDead=true;

		//PLAYTEST
		GameManager.instance.AddMetric("Bulles tirées", nbBullesTirées.ToString());

		colider.enabled=false;

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

	IEnumerator FlashPoison()
	{
		while(tempSpeedValue<reglages.speedPlayer)
		{
			sprite.color=Color.white;
			yield return new WaitForSeconds(1-(tempSpeedValue/reglages.speedPlayer));
			sprite.color=new Color32(64,220,0,0);
			yield return new WaitForSeconds(1-(tempSpeedValue/reglages.speedPlayer));
		}
	}

//MOVEMENT________________________________________________________________________________
	public void MovePlayer()
    {
        if((!canMove)||(isDashing))
            return;

        Vector2 inputVector = controller.getLeftStickDirection();
        if (inputVector == Vector2.zero)
            inputVector = keyboard.GetMovement();

        controlWithSpeed = controller.getLeftStickDirection()*tempSpeedValue;
        if(controlWithSpeed==Vector2.zero)
            controlWithSpeed=keyboard.GetMovement()*tempSpeedValue;
       // transform.Translate(new Vector2(controlWithSpeed.x, controlWithSpeed.y));
		particles.LaunchBulleStop(controlWithSpeed==Vector2.zero);
		particles.LaunchLineParticle(controlWithSpeed!=Vector2.zero);

        if (Mathf.Abs(inputVector.x) > 0.5f || Mathf.Abs(inputVector.y) > 0.5f)
        {
            targetAngle = controlWithSpeed;
       	    rb2D.velocity=new Vector2(controlWithSpeed.x,controlWithSpeed.y + Mathf.Sin(Time.frameCount / (30.0f / reglages.frequence)) * reglages.amplitude);
            Color color;
            if (!firstBubble && SceneManager.GetActiveScene().name == "MenuScene")
            {
                color = GameObject.Find("TutoBulle").GetComponent<SpriteRenderer>().color;
                color.a = 0.1f;
                GameObject.Find("TutoBulle").GetComponent<SpriteRenderer>().color = color;
            } else if (!firstDash && firstBubble)
            {
                color = GameObject.Find("TutoDash").GetComponent<SpriteRenderer>().color;
                color.a = 0.1f;
                GameObject.Find("TutoDash").GetComponent<SpriteRenderer>().color = color;
            } else if (SceneManager.GetActiveScene().name == "MenuScene")
            {
                color = GameObject.Find("TutoSauver").GetComponent<SpriteRenderer>().color;
                color.a = 0.1f;
                GameObject.Find("TutoSauver").GetComponent<SpriteRenderer>().color = color;
            }
        } else
        {
            rb2D.velocity = Vector2.zero;
            Color color;
            if (!firstBubble && SceneManager.GetActiveScene().name == "MenuScene")
            {
                color = GameObject.Find("TutoBulle").GetComponent<SpriteRenderer>().color;
                color.a = 1.0f;
                GameObject.Find("TutoBulle").GetComponent<SpriteRenderer>().color = color;
            }
            else if (!firstDash && firstBubble)
            {
                color = GameObject.Find("TutoDash").GetComponent<SpriteRenderer>().color;
                color.a = 1.0f;
                GameObject.Find("TutoDash").GetComponent<SpriteRenderer>().color = color;
            }
            else if (SceneManager.GetActiveScene().name == "MenuScene")
            {
                color = GameObject.Find("TutoSauver").GetComponent<SpriteRenderer>().color;
                color.a = 1.0f;
                GameObject.Find("TutoSauver").GetComponent<SpriteRenderer>().color = color;
            }
        }

	   	UpdateSpeedAnim();
        //rb2D.MovePosition(new Vector2(transform.position.x+controlWithSpeed.x,transform.position.y+controlWithSpeed.y));


        //sprite.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, controlWithSpeed));
    }

	public void UpdateSpeedAnim()
	{
		if(rb2D.velocity.x==0)
			animator.speed=1f;
		else if(rb2D.velocity.x>0)
			animator.speed = 1 + ((rb2D.velocity.x/reglages.speedPlayer)/2);
		else
			animator.speed =1 - ((Mathf.Abs(rb2D.velocity.x)/reglages.speedPlayer)/2);
	}

	public void UpdateTempSpeed()
	//pour le poison de l'oursin
	{
		if(tempSpeedValue==reglages.speedPlayer)
			return;

		if(tempSpeedValue<reglages.speedPlayer)
		{
			canDash=false;
			tempSpeedValue+=Time.deltaTime*reglages.oursinPoisonEffect;

            AkSoundEngine.SetState("Game_State", "Poisonus");
        }
		else
		{
			canDash=true;
			sprite.color=Color.white;
			tempSpeedValue=reglages.speedPlayer;

			particles.ActivatePoisonPartcle(false);

            AkSoundEngine.SetState("Game_State", "ReadyToDestroyBuble");

        }
    }

	public IEnumerator KnockbackPlayer(Vector2 direction)
	{
		//pour empêcher le joueur de bouger pendant le knockback
		canMove=false;
		canDash=false;
		animator.speed=0.2f;
		for(int i= 0;i<10;i++)
		{
			Vector2 final = new Vector2(direction.x*Time.deltaTime,direction.y*Time.deltaTime)*reglages.knockback;
			//deceleration de la velocité
			final*=(0.1f*(10-i));
			transform.Translate(final);
			yield return new WaitForSeconds(0.001f);
		}
		animator.speed=1f;
		canMove=true;
		canDash=true;
	}

//DASH__________________________________________________________________________________________

	public void Dash()
	{
        

		dashChrono-=Time.deltaTime;
		if((dashChrono>0)||(!canDash))
			return;
		if(controller.pressDashBouton()||keyboard.PressDashBouton())
		{
			if(curBuble!=null)
			{	
				BubleManager tempBuble = curBuble.GetComponent<BubleManager>();
				tempBuble.DestroyBuble();
			}
			isDashing=true;
			StartCoroutine(DashAction());
            AkSoundEngine.PostEvent("Play_Player_Dash_os", gameObject);

            if (!firstDash && firstBubble)
            {
                firstDash = true;
                GameObject.Find("TutoDash").GetComponent<SpriteRenderer>().enabled = false;
                GameObject.Find("TutoSauver").GetComponent<SpriteRenderer>().enabled = true;
            }
        }
	}

	IEnumerator DashAction()
	{

		animator.SetTrigger("Dash");

		particles.LaunchLineParticle(false);

		Vector2 tempDirection = controlWithSpeed;
		tempDirection.Normalize();

		if(tempDirection==Vector2.zero)
		{
			rb2D.velocity=new Vector2(reglages.speedPlayer,0f)*reglages.dashPower;
            targetAngle = Vector2.right;
		}
		else
        {
			rb2D.velocity=new Vector2(tempDirection.x*reglages.speedPlayer,tempDirection.y*reglages.speedPlayer)*reglages.dashPower;
            /*float angle = Vector2.SignedAngle(Vector2.right, tempDirection);
            GetComponentInChildren<SpriteRenderer>().transform.Rotate(new Vector3(0.0f, 0.0f, angle));*/
        }
		
		particles.ActiveDashParticles(true);
		
		//attendre la fin du dash
		float dash=reglages.dashDuration;
		while(dash>0)
		{
			dash-=Time.deltaTime;
			yield return new WaitForSeconds(0.001f);
		}
		//_______________________

		particles.ActiveDashParticles(false);

		isDashing=false;
		rb2D.velocity=Vector2.zero;
        //GetComponentInChildren<SpriteRenderer>().transform.rotation = Quaternion.identity;
        dashChrono =reglages.dashCoolDown;
	}

//COLLISIONS____________________________________________________________________________________

    void OnCollisionEnter2D(Collision2D col)
    {
        //rb2D.angularVelocity = 0.0f;
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
			particles.ActivatePoisonPartcle(true);
			StartCoroutine(FlashPoison());
			StartCoroutine(KnockbackPlayer(forceDirection));
			StartCoroutine(Damaged());
			col.gameObject.GetComponent<UrchinManager>().retract();
            LevelManager.instance.ChangeScore(LevelManager.instance.reglages.malusCollisionOursin);
		}
    }

	void OnCollisionStay2D(Collision2D col)
    {
        //rb2D.angularVelocity = 0.0f;
        if (col.gameObject.tag == "Buble")
       	{
			if(isDashing)
			//pousser la bulle super fort
			{
				Vector2 forceDirection = new Vector2(col.gameObject.transform.position.x-this.transform.position.x,col.gameObject.transform.position.y-this.transform.position.y);
				forceDirection*=200f;
				col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceDirection.x,forceDirection.y)*reglages.dashKnockbackBuble);
                AkSoundEngine.PostEvent("Play_Impact_Dash_bubble", gameObject);
			}
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
		if(controller.pressFireBouton()||keyboard.PressFireBouton())
			CreateBuble();
		if(controller.useFireBouton()||keyboard.UseFireBouton())
			GrowBuble();
		if(controller.releaseFireBouton()||keyboard.ReleaseFireBouton())
			ShootBuble();
	}

	public void CreateBuble()
	{
		if((curBuble==null)&&(!isDashing))
		{
			chronoIncrementSizeBuble=1f;
			Vector2 bublePosition= new Vector2(transform.position.x + (GetGameObjectWidth(gameObject) / 2), transform.position.y);
            curBuble = Instantiate(bublePrefab, bublePosition,Quaternion.identity) as GameObject;
			curBuble.GetComponent<BubleManager>().SetIsCreate(true);
			curBuble.transform.localScale=new Vector2(0.01f,0.01f);
			StartCoroutine(InitAnimBuble());
			curBuble.GetComponent<BubleManager>().GetRigidbody().drag=bullesReglages.velocityDecrease;

			Physics2D.IgnoreCollision(colider,curBuble.GetComponent<BubleManager>().GetCollider(),true);
            
            canMove = false;
            rb2D.velocity = Vector2.zero;
		}
	}

	IEnumerator InitAnimBuble()
	{
		GameObject tempBuble=curBuble;
		while(tempBuble.transform.localScale.x<bullesReglages.initialSize)
		{
			tempBuble.transform.localScale=new Vector2(tempBuble.transform.localScale.x+Time.deltaTime*8f,tempBuble.transform.localScale.y+Time.deltaTime*8f);
			tempBuble.GetComponent<CircleCollider2D>().enabled=false;
			yield return new WaitForSeconds(0.001f);
		}
		tempBuble.transform.localScale=new Vector2(bullesReglages.initialSize,bullesReglages.initialSize);
		tempBuble.GetComponent<CircleCollider2D>().enabled=true;
	}

	public void GrowBuble()
	{
		if(curBuble==null)
			return;
		if(curBuble.transform.localScale.x<bullesReglages.initialSize)
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
		Vector2 newPos = new Vector2(transform.position.x + (GetGameObjectWidth(gameObject) / 2) + (GetGameObjectWidth(curBuble) / 2) , transform.position.y);
		curBuble.transform.position=newPos;
	}

	public void ShootBuble()
	{
		if(curBuble==null)
			return;

        if (!firstBubble && SceneManager.GetActiveScene().name == "MenuScene")
        {
            GameObject.Find("TutoBulle").GetComponent<SpriteRenderer>().enabled = false;
            GameObject.Find("TutoDash").GetComponent<SpriteRenderer>().enabled = true;
            firstBubble = true;
        }

        curBuble.GetComponent<BubleManager>().SetIsCreate(false);
		//tir de la bulle

		nbBullesTirées++;

        int bubbleSize = curBuble.GetComponent<BubleManager>().getBubbleSize();
        Vector2 bubbleForce;
        if (bubbleSize == 1)
        {
            bubbleForce = new Vector2(500f, 0f) * bullesReglages.speedBubleInit;
        } else if (bubbleSize == 2)
        {
            bubbleForce = new Vector2(500f, 0f) * bullesReglages.speedBubleIntermediate;
        } else
        {
            bubbleForce = new Vector2(500f, 0f) * bullesReglages.speedBubleMax;
        }

        curBuble.GetComponent<Rigidbody2D>().AddForce(bubbleForce);
        curBuble.GetComponent<BubleManager>().GetBubleAnim().SetTrigger("Shoot");

        AkSoundEngine.PostEvent("Play_Player_Shot", gameObject);
       
        //effet des bulles a remonter vers la surface
        curBuble.GetComponent<Rigidbody2D>().gravityScale=-bullesReglages.archimedEffect;

		Physics2D.IgnoreCollision(colider,curBuble.GetComponent<BubleManager>().GetCollider(),false);

		//knockback du personnage
		StartCoroutine(KnockbackPlayer(new Vector2(-30f,0f)));

		curBuble=null;

        canMove = true;
    }

	public void DetachBuble()
	//pour detacher une bulle sans la tirer 
	{
		if(curBuble==null)
			return;

        if (!firstBubble && SceneManager.GetActiveScene().name == "MenuScene")
        {
            GameObject.Find("TutoBulle").GetComponent<SpriteRenderer>().enabled = false;
            GameObject.Find("TutoDash").GetComponent<SpriteRenderer>().enabled = true;
            firstBubble = true;
        }

		//Hugo : j'ai ajouté ça, je ne savais pas si un event special allait être crée 
        AkSoundEngine.PostEvent("Play_Player_Shot", gameObject);

		curBuble.GetComponent<BubleManager>().SetIsCreate(false);
        //effet des bulles a remonter vers la surface
        curBuble.GetComponent<Rigidbody2D>().gravityScale=-bullesReglages.archimedEffect;

		Physics2D.IgnoreCollision(colider,curBuble.GetComponent<BubleManager>().GetCollider(),false);
		curBuble=null;

        canMove = true;
    }


//GETTER & SETTER____________________________________________________________________________________________

	public CapsuleCollider2D GetColider()
	{
		return colider;
	}

    public float GetGameObjectWidth(GameObject gameObjectParam)
    {
        return GetGameObjectRightmostBound(gameObjectParam) - GetGameObjectLeftmostBound(gameObjectParam);
    }

    public float GetGameObjectLeftmostBound(GameObject gameObjectParam)
    {
        float situationLeftmostBound = gameObjectParam.transform.position.x;

        Renderer renderer = gameObjectParam.GetComponent<SpriteRenderer>();

        if (gameObjectParam.GetComponent<SpriteRenderer>() != null)
        {
            float rendererLeftmostBound = renderer.bounds.min.x;

            if (rendererLeftmostBound < situationLeftmostBound)
            {
                situationLeftmostBound = rendererLeftmostBound;
            }
        }

        SpriteMask mask = gameObjectParam.GetComponent<SpriteMask>();

        if (gameObjectParam.GetComponent<SpriteMask>() != null)
        {
            float maskLeftmostBound = mask.bounds.min.x;

            if (maskLeftmostBound < situationLeftmostBound)
            {
                situationLeftmostBound = maskLeftmostBound;
            }
        }

        foreach (Transform child in gameObjectParam.transform)
        {
            if (child != gameObjectParam.transform)
            {
                float childLeftmostBound = GetGameObjectLeftmostBound(child.gameObject);

                if (childLeftmostBound < situationLeftmostBound)
                {
                    situationLeftmostBound = childLeftmostBound;
                }
            }
        }

        return situationLeftmostBound;
    }

    public float GetGameObjectRightmostBound(GameObject gameObjectParam)
    {
        float situationRightmostBound = gameObjectParam.transform.position.x;

        Renderer renderer = gameObjectParam.GetComponentInChildren<SpriteRenderer>();

        if (gameObjectParam.GetComponent<SpriteRenderer>() != null)
        {
            float rendererRightmostBound = renderer.bounds.max.x;

            if (rendererRightmostBound > situationRightmostBound)
            {
                situationRightmostBound = rendererRightmostBound;
            }
        }

        SpriteMask mask = gameObjectParam.GetComponent<SpriteMask>();

        if (gameObjectParam.GetComponent<SpriteMask>() != null)
        {
            float maskRightmostBound = mask.bounds.max.x;

            if (maskRightmostBound > situationRightmostBound)
            {
                situationRightmostBound = maskRightmostBound;
            }
        }

        foreach (Transform child in gameObjectParam.transform)
        {
            if (child != gameObjectParam.transform)
            {
                float childRightmostBound = GetGameObjectRightmostBound(child.gameObject);
                if (childRightmostBound > situationRightmostBound)
                {
                    situationRightmostBound = childRightmostBound;
                }
            }
        }

        return situationRightmostBound;
    }
}
