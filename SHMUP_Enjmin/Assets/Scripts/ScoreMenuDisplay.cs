using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMenuDisplay : MonoBehaviour {

    Score score;

    Text scoreText;

	// Use this for initialization
	void Start () {
        score = GetComponent<Score>();
        scoreText = GetComponent<Text>();
        scoreText.text = "loading";
        score.DownloadHighscores();
        StartCoroutine(ShowScreen());
	}


    IEnumerator ShowScreen()
    {
        yield return new WaitForSeconds(1f);
        scoreText.text = score.GetOnlineScoreToString();
    }
}
