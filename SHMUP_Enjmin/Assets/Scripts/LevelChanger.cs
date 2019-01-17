﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {

    public Animator animator;

    private int levelToLoad;

	

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ObjPlay"))
        {
            FadeToLevel(2);
        }

        else if (other.CompareTag("ObjQuit"))
        {
            FadeToLevel(3);
        }
    }

    public void FadeToLevel (int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
        AkSoundEngine.PostEvent("Play_Play", gameObject);
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
        AkSoundEngine.SetState("MENU", "Out");



    }
}