using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_Lvl_02 : MonoBehaviour
{

    public string WiseEvent;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("In trig " + other.gameObject.tag);
        if (other.gameObject.tag == "Music_Lvl")
        {
            AkSoundEngine.SetState("Lvl_Musique", "Lvl_02");
            //    AkSoundEngine.PostEvent(WiseEvent, gameObject);
            Debug.Log("ak send");
        }
    }
}
