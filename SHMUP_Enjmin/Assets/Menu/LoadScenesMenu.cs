﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenesMenu : MonoBehaviour {

    [SerializeField] private string loadScene;
    [SerializeField] private string loadQuit;


    void OnTriggerEnter2D (Collider2D other)
    {
         if(other.CompareTag("ObjPlay"))
        {
            SceneManager.LoadScene(loadScene);
        }

         else if(other.CompareTag("ObjQuit"))
        {
            SceneManager.LoadScene(loadQuit);
        }
    }

}
