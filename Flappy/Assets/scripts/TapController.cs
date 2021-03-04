using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour {

	public delegate void PlayerDelegate ();
	public static event PlayerDelegate OnPlayerDied;//for the gameover page in the gamemanager to be active when the player dies
	public static event PlayerDelegate OnPlayerScored;//to change the scoreText in gamemanager when the score is made

	public float tapForce=10;
	public float tiltSmooth=5;
	public Vector3 startPos;

	public AudioSource tapAudio;
	public AudioSource scoreAudio;
	public AudioSource dieAudio;

	Rigidbody2D rb2;
	Quaternion downrotation;
	Quaternion forwardrotation;
	GameManager game;

	void Start(){
		rb2 = GetComponent<Rigidbody2D> ();
		downrotation = Quaternion.Euler (0, 0, -90);
		forwardrotation = Quaternion.Euler (0, 0, 35);
		game = GameManager.Instance;
		rb2.simulated = false;
	}

	void OnEnable(){
		GameManager.OnGameStarted += OnGameStarted;
		GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
	}

	void OnDisable(){
		GameManager.OnGameStarted -= OnGameStarted;
		GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
	}

	void OnGameStarted(){
		//Called when the countdown is over, resetting velocity (not necessary) and simulated (false on death).
		rb2.velocity = Vector3.zero;
		rb2.simulated = true;
	}

	void OnGameOverConfirmed(){
		//Position and rotation are initialized when the replay button is clicked.
		transform.localPosition = startPos;
		transform.rotation = Quaternion.identity;
	}

	void Update(){
		if (game.GameOver) {
			rb2.simulated = false;
			return;
		}
		if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) // spacebar is mostly for debugging
		{
			tapAudio.Play();
			transform.rotation = forwardrotation;
			rb2.velocity = Vector3.zero;
			rb2.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
		}
		transform.rotation = Quaternion.Lerp (transform.rotation, downrotation, tiltSmooth * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "ScoreZone")
		{

			OnPlayerScored();//The event GameManager is sent.
			scoreAudio.Play();
		}
		if (col.gameObject.tag == "DeadZone")
		{
			rb2.simulated = false;
			OnPlayerDied();//The event GameManager is sent.
			dieAudio.Play();
		}
	}

}
