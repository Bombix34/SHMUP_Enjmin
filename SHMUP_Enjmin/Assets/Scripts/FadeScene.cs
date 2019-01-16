using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeScene : MonoBehaviour {

    public string LevelToLoad;
    Animator animator;
	

	void Awake () {
        animator = GetComponent<Animator>();
	}
	
    public void FadeOut()
    {
        animator.SetTrigger("FadeOut");

    }

	
}
