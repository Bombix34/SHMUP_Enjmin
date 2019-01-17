using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScript : MonoBehaviour {

	void Start ()
    {
		
	}
	
	void Update () {
        transform.Translate(-LevelManager.instance.GetScrollingSpeed() * Time.deltaTime, 0f, 0f, Space.World);

        if (LevelManager.instance.GetGameObjectRightmostBound(this.gameObject) < -1 * Camera.main.orthographicSize * Camera.main.aspect)
        {
            print(this.gameObject.name);
            Destroy(this.gameObject);
        }
    }
}