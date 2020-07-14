using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResumeBtn() {
        Debug.Log("resume game");
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void QuitBtn() {
        Debug.Log("quit game");
        Application.Quit();
    }
}
