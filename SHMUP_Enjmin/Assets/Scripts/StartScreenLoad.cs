using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenLoad : MonoBehaviour {

    public Animator animator;

    private int levelToLoad;

    float chrono = 3f;


    void Update()
    {
        chrono-=Time.deltaTime;
        if ((Input.anyKeyDown)||(chrono<=0))
        {
            FadeToLevel(1);
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
