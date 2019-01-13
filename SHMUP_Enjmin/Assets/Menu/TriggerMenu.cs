using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMenu : MonoBehaviour {

    [Range(1f,20f)]

    public float transitionSpeed = 1f;
    
    public bool isInMainMenu = true;

    [SerializeField]
    //caracterise le trigger gauche ou droite 
    bool toMainMenuTrigger = false;

    [SerializeField]
    Vector2 cavernPosition=new Vector2(18f,0f);

    public GameObject uiScore;

   
	

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(isInMainMenu&&!toMainMenuTrigger)
            {
                //Transition vers la caverne des crédits
                StartCoroutine(StopPlayer(col.gameObject));
                uiScore.SetActive(false);
                StartCoroutine(MoveCamera(false,col.gameObject));
                SwitchIsInMainMenu(false);
            }
            else if(!isInMainMenu&&toMainMenuTrigger)
            {
                //Transition vers la caverne Main Menu 
                StartCoroutine(StopPlayer(col.gameObject));
                uiScore.SetActive(true);
                StartCoroutine(MoveCamera(true,col.gameObject));
                SwitchIsInMainMenu(true);
            }
        }
    }

    public void SwitchIsInMainMenu(bool val)
    {
        TriggerMenu[] triggs = transform.parent.GetComponentsInChildren<TriggerMenu>();
        foreach(TriggerMenu trig in triggs)
        {
            trig.isInMainMenu = val;
        }
    }

    IEnumerator StopPlayer(GameObject player)
    {

        player.GetComponent<PlayerManager>().enabled = false;
        yield return new WaitForSeconds(0.3f);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    IEnumerator MoveCamera(bool toMainMenu,GameObject player)
    {
        if (toMainMenu)
        {
            Debug.Log("transition vers menu");
            while (Camera.main.transform.position.x>0)
            {
                Camera.main.transform.Translate(new Vector2(-Time.deltaTime * transitionSpeed, 0f));
                yield return new WaitForSeconds(0.01f);
            }
            Camera.main.transform.position = new Vector3(0f, 0f,-10f);
        }
        else
        {
            while (Camera.main.transform.position.x < cavernPosition.x)
            {
                Camera.main.transform.Translate(new Vector2(Time.deltaTime * transitionSpeed, 0f));
                yield return new WaitForSeconds(0.01f);
            }
            Camera.main.transform.position = new Vector3(cavernPosition.x, cavernPosition.y, -10f);
        }
        player.GetComponent<PlayerManager>().enabled = true;
    }
}
