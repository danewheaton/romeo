using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField]
    private Cutscene[] cutscenes;
    [SerializeField]
    private string nextScene;

    private int currentCutscene = 0;

	// Use this for initialization
	void Start ()
    {
        PlayNextCutscene();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void PlayNextCutscene()
    {
        if (currentCutscene < cutscenes.Length)
        {
            cutscenes[currentCutscene].DisplayText();
            currentCutscene++;
        }
        else
        {
            LoadNextScene(nextScene);
        }
    }

    private void LoadNextScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
