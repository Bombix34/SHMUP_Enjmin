using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubleMainMenu : BubleManager 
{
	protected override void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "ObjMenu")
		//ENTRE DANS LA BULLE
        {
            if (curIsCreate)
				GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().DetachBuble();

			if(col.gameObject.GetComponent<BoxCollider2D>()!=null)
				col.gameObject.GetComponent<BoxCollider2D>().enabled=false;
			else if(col.gameObject.GetComponent<CircleCollider2D>()!=null)
				col.gameObject.GetComponent<CircleCollider2D>().enabled=false;


			//sinon on ajoute le perso dans la bulle
            StartCoroutine(SetObjectInTheBuble(col.gameObject));
            objectInTheBuble.Add(col.gameObject);

            AkSoundEngine.PostEvent("Play_Pnj_Oh", gameObject);
        }
        else if (col.gameObject.tag == "oursin")
        {
            // si la bulle est déjà créée, on retracte l'oursin
            if (!col.gameObject.GetComponent<UrchinManager>().GetIsRetracted())
            {
                col.gameObject.GetComponent<UrchinManager>().retract();
                DestroyBuble();
                LevelManager.instance.ChangeScore(LevelManager.instance.reglages.malusBulleAmiEclatee);
            }
            AkSoundEngine.PostEvent("Play_Bubble_Explode_Os", gameObject);
        }
        else
		//BOUNCE
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

	protected override void OnTriggerEnter2D(Collider2D col)
    {
		if (col.gameObject.tag == "ObjPlay")
		//ENTRE DANS LA BULLE
        {
            if (curIsCreate)
				GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().DetachBuble();

			if(col.gameObject.GetComponent<ObjMainMenu>()!=null)
			{
				if(col.gameObject.GetComponent<ObjMainMenu>().GetIsInBuble())
					return;
				else
					col.gameObject.GetComponent<ObjMainMenu>().SetInsBuble(true);
			}


			//sinon on ajoute le perso dans la bulle
            StartCoroutine(SetObjectInTheBuble(col.gameObject));
            objectInTheBuble.Add(col.gameObject);

            AkSoundEngine.PostEvent("Play_Pnj_Oh", gameObject);

        }
        else if (col.gameObject.tag == "ObjQuit")
        //ENTRE DANS LA BULLE
        {
            if (curIsCreate)
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().DetachBuble();
		
			if(col.gameObject.GetComponent<ObjMainMenu>()!=null)
			{
				if(col.gameObject.GetComponent<ObjMainMenu>().GetIsInBuble())
					return;
				else
					col.gameObject.GetComponent<ObjMainMenu>().SetInsBuble(true);
			}

            //sinon on ajoute le perso dans la bulle
            StartCoroutine(SetObjectInTheBuble(col.gameObject));
            objectInTheBuble.Add(col.gameObject);

            AkSoundEngine.PostEvent("Play_Pnj_Oh", gameObject);

        }
    }

	public override void DestroyBuble()
	{
		StopCoroutine(ShakeBuble());
		StartCoroutine(DestroyAnim());
	}
	protected override IEnumerator SetObjectInTheBuble(GameObject obj)
	{
		// on desactive le scrollable des potes emprisonnés dans la bulle
		if(obj.GetComponent<ScrollScript>()!=null)
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
		float randDist = Random.Range(5f,10f);
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
	}

	protected override void OnTriggerExit2D(Collider2D col)
	{
        if (col.gameObject.tag == "DeathBuble")
        {
            DestroyBuble();
        }
	}
}
