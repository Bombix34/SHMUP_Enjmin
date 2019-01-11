using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScript : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
        transform.Translate(-LevelManager.instance.GetScrollingSpeed() * Time.deltaTime, 0f, 0f, Space.World);

        if (transform.position.x + transform.localScale.x < -1 * Camera.main.orthographicSize * Camera.main.aspect)
        {
            Destroy(this.gameObject);
        }
    }
}
