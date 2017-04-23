using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class PauseScreenManager : MonoBehaviour
{
    [SerializeField]
    private string pauseButton;

    private bool isPaused = false;
	
	// Update is called once per frame
	void Update ()
    {
        Pause();
	}

    private void Pause()
    {
        if (Input.GetButtonDown(pauseButton))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Debug.Log("Game is paused");
                Time.timeScale = 0;
            }
            if (isPaused)
            {
                Debug.Log("Game unpaused");
                Time.timeScale = 1;
            }
        }
    }
}
