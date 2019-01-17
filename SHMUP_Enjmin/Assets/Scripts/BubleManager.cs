using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubleManager : MonoBehaviour {

	protected Rigidbody2D rb2D;
    protected CircleCollider2D colider;

	protected Animator animator;

	protected BubleSize bubleSize=BubleSize.init;

	[SerializeField]
	protected BubleReglages reglages;

	//pour différencier la bulle dans l'état de création par le player
	protected bool curIsCreate=false;

	protected List<GameObject> objectInTheBuble;

   protected  float rtpcValue2 = (float)BubleSize.init;
   

   bool endBubble=false;

    protected void Awake () 
	{
		rb2D = GetComponent<Rigidbody2D>();
        colider = GetComponent<CircleCollider2D>();
		objectInTheBuble = new List<GameObject>();
		colider = GetComponent<CircleCollider2D>();
		animator =GetComponent<Animator>();
	}

    protected void Start()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Wall"))
        {
            Physics2D.IgnoreCollision(GetCollider(), go.GetComponent<BoxCollider2D>());
        }
    }

    protected void Update()
	{
        rtpcValue2 = (float)bubleSize;
        AkSoundEngine.SetRTPCValue("BubbleSize", rtpcValue2, gameObject);
	}

	public virtual void DestroyBuble()
	{
		rb2D.velocity=Vector2.zero;
		StopCoroutine(ShakeBuble());
		List<GameObject>tempPote = objectInTheBuble;
        foreach(GameObject pote in tempPote)
        {
            // on decroche les potes dans les bulles, et on réactive leur scrollable
			if(pote!=null)
			{
				pote.transform.parent = null;
				pote.GetComponent<SavedManager>().SetIsInBuble(false);
				pote.GetComponent<SavedManager>().EnterBuble(false);
				pote.GetComponent<ScrollScript>().enabled = true;
			}
        }
		StartCoroutine(DestroyAnim());
        // enlever les bulles ui éclatent sortis d'écran
	}

	protected IEnumerator DestroyAnim()
	{
		animator.SetTrigger("Destroy");
		yield return new WaitForSeconds(0.3f);
		Destroy(this.gameObject);
	}

	public IEnumerator ShakeBuble()
	{
		Transform sprite = this.transform.Find("sprite");
		if(curIsCreate)
		{
			sprite.transform.position=new Vector2(this.transform.position.x+Random.Range(0f,0.1f),this.transform.position.y+Random.Range(0f,0.1f));
			yield return new WaitForSeconds(0.05f);
		}
		if(sprite!=null)
			sprite.transform.position=this.transform.position;
	}

	IEnumerator WinBuble()
	{
		endBubble=true;
		while((LevelManager.instance.GetGameObjectLowestBound(this.gameObject))<(Camera.main.orthographicSize))
		{
			transform.Translate(0f,Time.deltaTime*2f,0f);
			yield return new WaitForSeconds(0.01f);
		}
		foreach(GameObject pote in objectInTheBuble)
        {
			GameManager.instance.AddScore();
            TentaclesManager.instance.MoveBackward();
            LevelManager.instance.ChangeScore(LevelManager.instance.reglages.bonusAmiSauve);
        }
		for (int i = objectInTheBuble.Count - 1; i >= 0; i--)
        {
			Destroy(objectInTheBuble[i]);
		}
        Destroy(this.gameObject);
	}


//OBJETS DANS LA BULLE_________________________________________________________________________________________

	protected List<GameObject> CheckCharacterInBuble(string tag)
	{
		List<GameObject> finalList=new List<GameObject>();
		if(objectInTheBuble.Count==0)
			return finalList;
		foreach(GameObject obj in objectInTheBuble)
		{
			if(obj.tag==tag)
				finalList.Add(obj);
		}
		return finalList;
	}

	protected virtual IEnumerator SetObjectInTheBuble(GameObject obj)
	{
        // on desactive le scrollable des potes emprisonnés dans la bulle
        obj.GetComponent<ScrollScript>().enabled = false;
		animator.SetTrigger("Shoot");
        if(obj.GetComponent<PatternInterface>() != null)
        {
            obj.GetComponent<PatternInterface>().enabled = false;
        }
        obj.transform.parent=this.transform;

		Vector2 forceDirection = new Vector2(this.transform.position.x-obj.transform.position.x,this.transform.position.y-obj.transform.position.y);
		float distanceFromCenter = GetDistanceFromBubleCenter(obj.transform.position);
		forceDirection.Normalize();
		int frameCount=0;
		float randDist = 0;
		switch(objectInTheBuble.Count)
		{
			case 0:
				randDist = 0.05f;
				break;
			case 1:
				randDist = 0.3f;
				break;
			case 2:
				randDist = 0.4f;
				break;
		}
		while(GetDistanceFromBubleCenter(obj.transform.position) > (distanceFromCenter  * randDist ))
		{
			if(GetDistanceFromBubleCenter(obj.transform.position) < (distanceFromCenter-0.3f ))
			{
				//on attend un certain nombre de frame avant de le faire rentrer très vite 
				if(obj.GetComponent<SavedManager>()!=null)
					obj.GetComponent<SavedManager>().EnterBuble(true);
				obj.transform.Translate(forceDirection.x*Time.deltaTime*5f,forceDirection.y*Time.deltaTime*5f,0f);
			}
			else
				obj.transform.Translate(forceDirection.x*Time.deltaTime*0.6f,forceDirection.y*Time.deltaTime*0.6f,0f);
			frameCount++;
			if(frameCount>40&&objectInTheBuble.Count==1)
			//pour empêcher que les ptits calamars partent dans l'espace des fois
			//je sais pas d'ou provient le bug donc je mets une petite sécurité 
				obj.transform.position = this.transform.position;
			yield return new WaitForSeconds(0.001f);
		}
		if(objectInTheBuble.Count==1)
		{
			obj.GetComponent<SavedManager>().EnterBuble(true);
			obj.transform.position = this.transform.position;
		}
	}

	public float GetDistanceFromBubleCenter(Vector2 pos)
	{
		return Mathf.Sqrt(Mathf.Pow(this.transform.position.x-pos.x,2)+Mathf.Pow(this.transform.position.y-pos.y,2));
	}

//COLLISIONS_____________________________________________________________________________________________________

	protected virtual void OnCollisionEnter2D(Collision2D col)
    {
       	if (col.gameObject.tag == "Player")
        {
        }
		else if(col.gameObject.tag == "oursin")
		{
			// si la bulle est déjà créée, on retracte l'oursin
			if(!col.gameObject.GetComponent<UrchinManager>().GetIsRetracted())
			{
				col.gameObject.GetComponent<UrchinManager>().retract();	
				DestroyBuble();
                LevelManager.instance.ChangeScore(LevelManager.instance.reglages.malusBulleAmiEclatee);
            }
            AkSoundEngine.PostEvent("Play_Bubble_Explode_Os", gameObject);
		}
		else
		{
			if (curIsCreate)
				GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().DetachBuble();
			else
			{
				//les bulles bounce un peu partout
				Vector2 contactPoint=col.GetContact(0).point;
				Vector2 forceDirection = new Vector2(this.transform.position.x-contactPoint.x,this.transform.position.y-contactPoint.y);
				forceDirection.Normalize();
				forceDirection*=200f;
				rb2D.AddForce(new Vector2(forceDirection.x,forceDirection.y)*reglages.bounceEffect);
			}
		}
    }

	protected virtual void OnTriggerEnter2D(Collider2D col)
    {
		if(col.gameObject.tag=="Player")
		{
		}
        else if (col.gameObject.tag == "ToSave")
        {
			if(col.gameObject.GetComponent<SavedManager>().GetIsInbuble())
				return;
            if (curIsCreate)
				GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().DetachBuble();
            
			//on vérifie si on peut ajouter un personnage dans cette bulle en fonction de sa taille
			if((objectInTheBuble.Count==(int)bubleSize)||(objectInTheBuble.Contains(col.gameObject)))
			{
				//si on ne peut pas, on repousse la bulle
				Vector2 forceDirection = new Vector2(this.transform.position.x-col.transform.position.x,this.transform.position.y-col.transform.position.y);
				forceDirection.Normalize();
				forceDirection*=200f;
				rb2D.AddForce(new Vector2(forceDirection.x,forceDirection.y)*20f);
				return;
			}
			//sinon on ajoute le perso dans la bulle
			col.gameObject.GetComponent<SavedManager>().SetIsInBuble(true);
            StartCoroutine(SetObjectInTheBuble(col.gameObject));
            objectInTheBuble.Add(col.gameObject);
            
            LevelManager.instance.ChangeScore(LevelManager.instance.reglages.bonusAmiMisEnBulle);

            AkSoundEngine.PostEvent("Play_Pnj_Oh", gameObject);
        }
		else if(col.gameObject.tag == "upBuble")
        {
			if(!endBubble)
            	StartCoroutine(WinBuble());
        }	
    }
	
	protected virtual void OnTriggerExit2D(Collider2D col)
	{
        if (col.gameObject.tag == "DeathBuble")
        {
            DestroyBuble();
            AkSoundEngine.PostEvent("Play_Bubble_Explode_Os", gameObject);

        }
    }

//GETTER & SETTER____________________________________________________________________________________________

	public Rigidbody2D GetRigidbody()
	{
		if(rb2D==null)
			rb2D=GetComponent<Rigidbody2D>();
		return rb2D;
	}

	public void SetIsCreate(bool val)
	{
		curIsCreate=val;
	}

	public bool GetIsCreate()
	{
		return curIsCreate;
	}

	public CircleCollider2D GetCollider()
	{
		return colider;
	}

	public void IncrementBubleSize()
	{
		if(bubleSize==BubleSize.final)
			return;
		bubleSize++;
        AkSoundEngine.PostEvent("Play_Bulles_Grown", gameObject);
	}

	public Animator GetBubleAnim()
	{
		return animator;
	}

	protected enum BubleSize
	{
		init=1,
		intermediate=2,
		final=3
	}

    public int getBubbleSize()
    {
        return (int)bubleSize;
    }
}
