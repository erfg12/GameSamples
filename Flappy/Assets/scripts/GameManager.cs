using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {
	public delegate void GameDelegate ();
	public static event GameDelegate OnGameStarted;//Plane actions when the countdown is over
	public static event GameDelegate OnGameOverConfirmed;//Clicking the replay button, the operations to be done on the plane and the status of Parallax objects
	public static GameManager Instance;//GameManagera reference to access another script from outside (Singleton)

	public GameObject startPage;
	public GameObject gameOverPage;
	public GameObject countdownPage;
	public GameObject scoreText;
	public GameObject playerObj;

	enum PageState{
		None,
		Start,
		GameOver,
		Countdown
	}

	int score = 0;
	bool gameOver = true;

	public bool GameOver { get { return gameOver; } }

	void Awake(){
		Instance = this;
	}

	void OnEnable(){
		CountdownText.OnCountdownFinished += OnCountdownFinished;
		TapController.OnPlayerDied += OnPlayerDied;
		TapController.OnPlayerScored += OnPlayerScored;
	}

	void OnDisable(){
		CountdownText.OnCountdownFinished -= OnCountdownFinished;
		TapController.OnPlayerDied -= OnPlayerDied;
		TapController.OnPlayerScored -= OnPlayerScored;
	}

	void OnCountdownFinished(){
		SetPageState (PageState.None);
		scoreText.SetActive(true);
		OnGameStarted ();//event TapController ways
		score = 0;
		gameOver = false;
	}

	void OnPlayerDied(){
		gameOver = true;
		int savedScore = PlayerPrefs.GetInt ("highscore");
		if (score > savedScore) {
			PlayerPrefs.SetInt ("highscore", score);
		}
		SetPageState (PageState.GameOver);
	}

	void OnPlayerScored(){
		score++;
		scoreText.GetComponent<Text>().text = score.ToString();
	}

	void SetPageState(PageState state){
		switch (state)
		{
			case PageState.None:
				startPage.SetActive(false);
				gameOverPage.SetActive(false);
				countdownPage.SetActive(false);
				break;
			case PageState.Start:
				scoreText.SetActive(false);
				playerObj.SetActive(false);
				startPage.SetActive(true);
				gameOverPage.SetActive(false);
				countdownPage.SetActive(false);
				break;
			case PageState.GameOver:
				startPage.SetActive(false);
				gameOverPage.SetActive(true);
				countdownPage.SetActive(false);
				break;
			case PageState.Countdown:
				playerObj.SetActive(true);
				startPage.SetActive(false);
				gameOverPage.SetActive(false);
				countdownPage.SetActive(true);
				break;
		}
	}

	public void ConfirmedGameOver(){
		//It is activated when the replay button is clicked.
		OnGameOverConfirmed();//event TapController ways
		scoreText.GetComponent<Text>().text = "0";
		SetPageState (PageState.Start);
	}
	public void StartGame()
	{
		//It becomes active when the play button is clicked.
		SetPageState(PageState.Countdown);
	}
}
