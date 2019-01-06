using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_Trigger : MonoBehaviour {

    public string WiseEvent;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("In trig " + other.gameObject.tag);
        if (other.gameObject.tag == "Player")
        {
            AkSoundEngine.PostEvent(WiseEvent, gameObject);
            Debug.Log("ak send");
        }
    }
}
