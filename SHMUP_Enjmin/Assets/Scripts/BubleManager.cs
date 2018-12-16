using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubleManager : MonoBehaviour {

	Rigidbody2D rb2D;
	CircleCollider2D colider;

	void Start () 
	{
		rb2D=GetComponent<Rigidbody2D>();	
		colider=GetComponent<CircleCollider2D>();
	}

	public IEnumerator LaunchBubleCoroutine(float velocityDecreaseAmount)
	{
		//quand le joueur tire la bulle
		yield return new WaitForSeconds(0.05f);
		while(rb2D.velocity.x>0)
		{
			rb2D.velocity=new Vector2(rb2D.velocity.x-velocityDecreaseAmount,0f);
			yield return new WaitForSeconds(0.01f);
		}
		rb2D.velocity=new Vector2(0f,0f);
	}

//COLLISIONS_____________________________________________________________________________________________________

	void OnCollisionEnter2D(Collision2D col)
    {
		if(col.gameObject.tag=="DeathBuble")
		{
			Destroy(this.gameObject);
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
		if(col.gameObject.tag=="Player")
		{	
			//quand le joueur tire la bulle, remettre son collider pour les autres bulles
			colider.isTrigger=false;
		}
	}
}
