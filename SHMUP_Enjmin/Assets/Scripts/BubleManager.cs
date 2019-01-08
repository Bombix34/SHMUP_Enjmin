using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubleManager : MonoBehaviour {

	Rigidbody2D rb2D;
    CircleCollider2D colider;

	BubleSize bubleSize=BubleSize.init;

	//pour différencier la bulle dans l'état de création par le player
	bool curIsCreate=false;

	List<GameObject> objectInTheBuble;

    float rtpcValue2 = (float)BubleSize.init;

    void Awake () 
	{
		rb2D = GetComponent<Rigidbody2D>();
        colider = GetComponent<CircleCollider2D>();
		objectInTheBuble = new List<GameObject>();
		colider = GetComponent<CircleCollider2D>();
	}

	void Update()
	{
        rtpcValue2 = (float)bubleSize;
        AkSoundEngine.SetRTPCValue("BubbleSize", rtpcValue2, gameObject);

        if (curIsCreate)
			return;

        //transform.localScale = new Vector3(transform.localScale.x * 1.1f, transform.localScale.y * 1.1f, transform.localScale.z);
	}


	public void DestroyBuble()
	{
        foreach(GameObject pote in objectInTheBuble)
        {
            // on decroche les potes dans les bulles, et on réactive leur scrollable
            pote.transform.parent = null;
            pote.GetComponent<ScrollScript>().enabled = true;
        }
		Destroy(this.gameObject);
        // enlever les bulles ui éclatent sortis d'écran
	}

	public IEnumerator ShakeBuble()
	{
		Transform sprite = this.transform.Find("sprite");
		if(curIsCreate)
		{
			sprite.transform.position=new Vector2(this.transform.position.x+Random.Range(0f,0.1f),this.transform.position.y+Random.Range(0f,0.1f));
			yield return new WaitForSeconds(0.05f);
		}
		sprite.transform.position=this.transform.position;
	}


//OBJETS DANS LA BULLE_________________________________________________________________________________________

	List<GameObject> CheckCharacterInBuble(string tag)
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

	IEnumerator SetObjectInTheBuble(GameObject obj)
	{
        // on desactive le scrollable des potes emprisonnés dans la bulle
        obj.GetComponent<ScrollScript>().enabled = false;
        if(obj.GetComponent<PatternInterface>() != null)
        {
            obj.GetComponent<PatternInterface>().enabled = false;
        }
        obj.transform.parent=this.transform;

		Vector2 forceDirection = new Vector2(this.transform.position.x-obj.transform.position.x,this.transform.position.y-obj.transform.position.y);
		float distanceFromCenter = GetDistanceFromBubleCenter(obj.transform.position);
		forceDirection.Normalize();
		int frameCount=0;
		float randDist = Random.Range(3f,10f);
		while(GetDistanceFromBubleCenter(obj.transform.position) > (distanceFromCenter * 1/randDist ))
		{
			if(frameCount>45)
				//on attend un certain nombre de frame avant de le faire rentrer très vite 
				obj.transform.Translate(forceDirection.x*Time.deltaTime*5f,forceDirection.y*Time.deltaTime*5f,0f);
			else
				obj.transform.Translate(forceDirection.x*Time.deltaTime*0.6f,forceDirection.y*Time.deltaTime*0.6f,0f);
			frameCount++;
			yield return new WaitForSeconds(0.001f);
		}
		//obj.transform.position= new Vector2(obj.transform.position.x+forceDirection.x,)
	}

	public float GetDistanceFromBubleCenter(Vector2 pos)
	{
		return Mathf.Sqrt(Mathf.Pow(this.transform.position.x-pos.x,2)+Mathf.Pow(this.transform.position.y-pos.y,2));
	}

//COLLISIONS_____________________________________________________________________________________________________

	void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag=="Buble")
		{
			if(curIsCreate)
			{
				//détruit la bulle que le personnage est en train de créer 
				DestroyBuble();
                AkSoundEngine.PostEvent("Play_Bubble_Explode_Os", gameObject);

            }
        }
        else if (col.gameObject.tag == "Player")
        {
            if (curIsCreate)
            {
                //Physics2D.IgnoreCollision(colider, col.collider);
            }
        }
    }

	void OnTriggerEnter2D(Collider2D col)
    {
		if(col.gameObject.tag == "oursin")
		{
			// si la bulle est déjà créée, on retracte l'oursin
			if(!curIsCreate)
				col.gameObject.GetComponent<UrchinManager>().retract();	
			DestroyBuble();
            AkSoundEngine.PostEvent("Play_Bubble_Explode_Os", gameObject);

        }
        else if(col.gameObject.tag=="Player")
		{
		}
        else if (col.gameObject.tag == "ToSave")
        {
            if (curIsCreate)
            {
                //détruit la bulle que le personnage est en train de créer 
                DestroyBuble();
                AkSoundEngine.PostEvent("Play_Bubble_Explode_Os", gameObject);

            }
            else
            {
				//on vérifie si on peut ajouter un personnage dans cette bulle en fonction de sa taille
				if((objectInTheBuble.Count==(int)bubleSize)||(objectInTheBuble.Contains(col.gameObject)))
					return;
                StartCoroutine(SetObjectInTheBuble(col.gameObject));
                objectInTheBuble.Add(col.gameObject);

                AkSoundEngine.PostEvent("Play_Pnj_Oh", gameObject);
            }
        }
    }
	
	void OnTriggerExit2D(Collider2D col)
	{
        if (col.gameObject.tag == "DeathBuble")
        {
            DestroyBuble();
        }

        if(col.gameObject.tag == "upBuble")
        {
            foreach(GameObject pote in objectInTheBuble)
            {
				GameManager.instance.AddScore();
                Destroy(pote);
            }
            TentaclesManager.instance.MoveBackward();
            Destroy(this.gameObject);
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

	enum BubleSize
	{
		init=1,
		intermediate=2,
		final=3
	}
}
