using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyKeyLoad : MonoBehaviour {

    public Animator animator;

    private int levelToLoad;


    void Update()
    {
        if (Input.anyKeyDown)
        {
            FadeToLevel(3);
        }
    }

    public void FadeToLevel (int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);

    }
}
