using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_Trigger : MonoBehaviour {

    public string WiseEvent;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("In trig " + other.gameObject.tag);
        if (other.gameObject.tag == "Music_Lvl")
        {
            AkSoundEngine.SetState("Lvl_Musique", "Lvl_01");
        //    AkSoundEngine.PostEvent(WiseEvent, gameObject);
        }
    }
}
