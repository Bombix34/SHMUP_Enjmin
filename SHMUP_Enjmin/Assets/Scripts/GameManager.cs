using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	int score=0;

	[SerializeField]
	GameObject gameOverUI;

	[SerializeField]
	GameObject whiteMask;

	GameObject player;

	Score highScore;

	float chrono=0f;

	bool gameOver=false;

	void Start () 
	{
		player=GameObject.FindGameObjectWithTag("Player");
		highScore=GetComponent<Score>();
	}

	void Update()
	{
		chrono+=Time.deltaTime;
	}

	public void AddScore()
	{
		score++;
	}

	public int GetScore()
	{
		return score;
	}

	public void GameOver()
	{
		gameOver=true;

		AddMetric("Time",Mathf.Floor((chrono/60)).ToString() + ":" + Mathf.RoundToInt(chrono%60).ToString());
		AddMetric("Score",score.ToString());
		
		GetComponent<Playtest>().Save();

		GameObject gameover = Instantiate(gameOverUI, transform.position,Quaternion.identity) as GameObject;
		gameover.GetComponent<GameOverUI>().scoreText.text=score.ToString();
		gameover.GetComponent<GameOverUI>().allSavedText.text=highScore.GetOnlineScoreToString();
		player.GetComponent<PlayerManager>().Die();
		highScore.AddNewHighscore("world",score);
		gameover.SetActive(true);

        AkSoundEngine.PostEvent("Stop_All", gameObject);
        AkSoundEngine.PostEvent("Play_Music_GameOver", gameObject);

    }

    public void RelaunchGame()
	{
		AddMetric("Rejoué",true.ToString());

		GetComponent<Playtest>().Save();

		SceneManager.LoadScene("MainScene");
	}

	public void MainMenu()
	{
		AddMetric("Rejoué",false.ToString());

		GetComponent<Playtest>().Save();

		//A CHANGER AVEC LA SCENE DU MENU PRINCIPAL
		SceneManager.LoadScene("MenuScene");
	}

	public void LaunchFlash()
	{
		whiteMask.GetComponent<Animator>().SetTrigger("flash");
	}

	public void QuitGame()
	{
		GetComponent<Playtest>().Save();
		SceneManager.LoadScene("QuitScene");
	}

	public void AddMetric(string name, string val)
	{
		if(GetComponent<Playtest>()==null)
			return;
		GetComponent<Playtest>().AddMetric(name, val);
	}

	public bool IsGameOver()
	{
		return gameOver;
	}

//SINGLETON________________________________________________________________________________________________

	private static GameManager s_Instance = null;

    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static GameManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
                Debug.Log("error");
                GameObject obj = new GameObject("Error");
                s_Instance = obj.AddComponent(typeof(GameManager)) as GameManager;
            }

            return s_Instance;
        }
    }
}
