using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillGame : MonoBehaviour {

    public Animator animator;

    private int levelToLoad;


    
    void Update()
    {
            StartCoroutine("Quit");
    }

    IEnumerator Quit()
    {
        yield return new WaitForSeconds(3);
        QuitGame();
        
    }

    public void QuitGame()
    {
        // save any game data here
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
             Application.Quit();
#endif
    }
}
