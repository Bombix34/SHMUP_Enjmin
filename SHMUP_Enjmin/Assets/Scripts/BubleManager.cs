using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubleManager : MonoBehaviour {

	Rigidbody2D rb2D;
	CircleCollider2D colider;

	//pour différencier la bulle dans l'état de création par le player
	bool curIsCreate=false;

	List<GameObject> objectInTheBuble;

	void Awake () 
	{
		rb2D=GetComponent<Rigidbody2D>();	
		colider=GetComponent<CircleCollider2D>();
		objectInTheBuble=new List<GameObject>();
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
		if(col.gameObject.tag=="DeathBuble")
		{
			Destroy(this.gameObject);
		}
		else if(col.gameObject.tag=="Buble")
		{
			if(curIsCreate)
			{
				//détruit la bulle que le personnage est en train de créer 
				Destroy(this.gameObject);
			}
		}
    }

	void OnTriggerEnter2D(Collider2D col)
    {
		if(col.gameObject.tag=="DeathBuble")
		{
			Destroy(this.gameObject);
		}
	}
	void OnTriggerStay2D(Collider2D col)
    {
		if(col.gameObject.tag=="ToSave")
		{
			StartCoroutine(SetObjectInTheBuble(col.gameObject));
			objectInTheBuble.Add(col.gameObject);
		}
	}
	void OnTriggerExit2D(Collider2D col)
	{
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
}
