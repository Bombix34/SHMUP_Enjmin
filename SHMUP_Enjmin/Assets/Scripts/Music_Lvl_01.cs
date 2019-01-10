using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_Lvl_01 : MonoBehaviour
{

    public string WiseEvent;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("In trig " + other.gameObject.tag);
        if (other.gameObject.tag == "Music_Lvl")
        {
            AkSoundEngine.SetState("Lvl_Musique", "Lvl_01");
            //    AkSoundEngine.PostEvent(WiseEvent, gameObject);
            Debug.Log("ak send");
        }
    }
}
