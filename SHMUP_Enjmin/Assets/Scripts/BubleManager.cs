using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubleManager : MonoBehaviour {

	Rigidbody2D rb2D;
	CircleCollider2D colider;

	//pour différencier la bulle dans l'état de création par le player
	bool curIsCreate=false;

	void Awake () 
	{
		rb2D=GetComponent<Rigidbody2D>();	
		colider=GetComponent<CircleCollider2D>();
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
