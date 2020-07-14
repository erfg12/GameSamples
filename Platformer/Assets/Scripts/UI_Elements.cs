using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Elements : MonoBehaviour
{
    public Text CoinsLabel;
    public Text LivesLabel;

    public Player thePlayer;
    public Text SpeechText;
    public GameObject SpeechBox;
    public Text LavaTimer;
    private bool playedvideo = false;
    private float update = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        CoinsLabel.text = "x00";
        LivesLabel.text = "x0";
        if (SceneManager.GetActiveScene().name == "Level 3")
            LavaTimer.text = "30";
    }

    // Update is called once per frame
    void Update()
    {
        update += Time.deltaTime;

        if (thePlayer == null)
            return;

        CoinsLabel.text = "x" + PlayerStats.coins.ToString();
        LivesLabel.text = "x" + PlayerStats.lives.ToString();

        if (SceneManager.GetActiveScene().name == "Level 3") {
            if (Mathf.Round(30 - update) > 0)
                LavaTimer.text = (Mathf.Round(30 - update)).ToString();
            else
                thePlayer.PlayerHit();
        }

        if (SpeechBox != null && SpeechText != null) {
            // level 1 speech
            if (SceneManager.GetActiveScene().name == "Level 1") {
                // cipher speech
                if (thePlayer.transform.position.x > 13 && thePlayer.transform.position.x < 14) {
                    if (playedvideo == false) {
                        update = 11.0f;
                        playedvideo = true;
                        SpeechText.text = "Cipher: I'm not letting you beat this game!";
                        SpeechBox.SetActive(true);
                    }
                    if (update > 17.0f && update < 18.0f) {
                        SpeechText.text = "";
                        SpeechBox.SetActive(false);
                    }
                }
                // beginning speech
                else {
                    if (update > 0 && update < 3) {
                        SpeechBox.SetActive(true);
                        SpeechText.text = "Oram: Let's a go!";
                    } else if (update > 3 && update < 6) {
                        SpeechText.text = "Oram: Ugh... I don't feel so good...";
                    } else if (update > 6 && update < 10) {
                        SpeechText.text = "Xae: Ahhh, a new game to hack. Let's get started!";
                    } else if (update > 10 && update < 11) {
                        SpeechText.text = "";
                        SpeechBox.SetActive(false);
                    }
                }
            }
        }
    }
}
