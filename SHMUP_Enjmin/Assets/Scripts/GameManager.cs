using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	int score=0;

	[SerializeField]
	GameObject gameOverUI;

	GameObject player;

	Score highScore;

	void Start () 
	{
		player=GameObject.FindGameObjectWithTag("Player");
		highScore=GetComponent<Score>();
	}

	public void AddScore()
	{
		score++;
	}

	public void GameOver()
	{
		GameObject gameover = Instantiate(gameOverUI, transform.position,Quaternion.identity) as GameObject;
		gameover.GetComponent<GameOverUI>().scoreText.text=score.ToString();
		player.GetComponent<PlayerManager>().Die();
		highScore.AddNewHighscore("world",score);
		gameover.SetActive(true);
	}

	public void RelaunchGame()
	{
		SceneManager.LoadScene("MainScene");
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
