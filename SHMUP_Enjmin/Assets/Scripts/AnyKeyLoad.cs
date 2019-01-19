using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyKeyLoad : MonoBehaviour {

    Animator animator;

    private int levelToLoad;

    public GameObject pressAnyKeyText;

    public float chrono;

    bool cmoche = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        pressAnyKeyText.SetActive(false);
    }

    void Update()
    {
        if (chrono > 0)
            chrono -= Time.deltaTime;
        else
        {
            StartCoroutine(FlashText());
            cmoche = true;
            if (Input.anyKeyDown)
            {
                FadeToLevel(3);
            }
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

    IEnumerator FlashText()
    {
        if (!cmoche)
        {
            while (true)
            {
                pressAnyKeyText.SetActive(true);
                yield return new WaitForSeconds(1.3f);
                pressAnyKeyText.SetActive(false);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
