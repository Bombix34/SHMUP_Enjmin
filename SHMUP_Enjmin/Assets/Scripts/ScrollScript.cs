using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(-LevelManager.instance.GetScrollingSpeed() * Time.deltaTime, 0f, 0f);
    }
}
