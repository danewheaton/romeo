using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField]
    private Text playerFeedBackText;

    private WaitForSeconds textDelay = new WaitForSeconds(2);
    private AudioSource menuSelectionFailed;

	// Use this for initialization
	void Start ()
    {
        playerFeedBackText.text = "Level Unavailable";
        playerFeedBackText.gameObject.SetActive(false);
        menuSelectionFailed = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void LoadScene(string levelToLoad)
    {
        Debug.Log("Loading " + levelToLoad);
        if (levelToLoad == "empty")
        {
            menuSelectionFailed.Play();
            StartCoroutine(LevelCurrentlyUnavailable());
        }

        else
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }

    private IEnumerator LevelCurrentlyUnavailable()
    {
        playerFeedBackText.gameObject.SetActive(true);
        yield return textDelay;
        playerFeedBackText.gameObject.SetActive(false);
    }
}
