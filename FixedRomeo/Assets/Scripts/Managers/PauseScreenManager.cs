using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//All code by CH unless otherwise mentioned
//goes on the PauseScreen canvas
public class PauseScreenManager : MonoBehaviour
{
    [SerializeField]
    private string pauseButton;

    private GameObject PausePanel;

    void Start()
    {
        PausePanel = GameObject.Find("PausePanel");
        AudioListener.pause = false;
        PausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
        Pause();
    }

    private void Pause()
    {
        if (Input.GetButtonDown(pauseButton))
        {
            PausePanel.SetActive(true);
            AudioListener.pause = true;
            Time.timeScale = 0;
        }
    }

    //functions to quit game and continue through the pause screen
    public void Continue()
    {
        PausePanel.SetActive(false);
        AudioListener.pause = false;
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
