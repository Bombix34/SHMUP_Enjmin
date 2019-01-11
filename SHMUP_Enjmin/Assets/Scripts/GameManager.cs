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

	float chrono=0f;

	void Start () 
	{
		player=GameObject.FindGameObjectWithTag("Player");

      //  AkSoundEngine.SetState("Game_State", "inGame");
        AkSoundEngine.SetState("Lvl_Musique", "Lvl_01");
        AkSoundEngine.PostEvent("Innit_Amb", gameObject);


    }

    void Update () 
	{
		chrono+=Time.deltaTime;
	}

	public void AddScore()
	{
		score++;
	}

	public void GameOver()
	{
		AddMetric("Time",Mathf.Floor((chrono/60)).ToString() + ":" + Mathf.RoundToInt(chrono%60).ToString());
		AddMetric("Score",score.ToString());
		
		GetComponent<Playtest>().Save();

		GameObject gameover = Instantiate(gameOverUI, transform.position,Quaternion.identity) as GameObject;
		gameover.GetComponent<GameOverUI>().scoreText.text=score.ToString();
		player.GetComponent<PlayerManager>().Die();
		gameover.SetActive(true);
     //   AkSoundEngine.SetState("Game_State", "gameOver");
        AkSoundEngine.PostEvent("Stop_All", gameObject);
        AkSoundEngine.PostEvent("Play_Music_GameOver", gameObject);

    }

    public void RelaunchGame()
	{
        Debug.Log("aaa");
        SceneManager.LoadScene("MainScene");
        AkSoundEngine.PostEvent("Stop_Music_GameOver", gameObject);


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
