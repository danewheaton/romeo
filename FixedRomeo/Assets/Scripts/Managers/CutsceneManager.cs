using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    enum Language { English, Spanish, Polish}

    [SerializeField]
    private Cutscene[] cutscenes;
    [SerializeField]
    private string[] cutsceneStringsEnglish;
    [SerializeField]
    private string[] cutsceneStringsSpanish;
    [SerializeField]
    private string[] cutsceneStringsPolish;
    [SerializeField]
    private string nextScene;
    [SerializeField]
    private Language debugLanguage;

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

    private void LoadNextScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    private void SetCutsceneStringsBasedOnLanguage()
    {
        switch (debugLanguage)
        {
            case Language.English:
                cutscenes[currentCutscene].SetCutsceneString(cutsceneStringsEnglish[currentCutscene]);
                break;
            case Language.Spanish:
                cutscenes[currentCutscene].SetCutsceneString(cutsceneStringsSpanish[currentCutscene]);
                break;
            case Language.Polish:
                cutscenes[currentCutscene].SetCutsceneString(cutsceneStringsPolish[currentCutscene]);
                break;
            default:
                cutscenes[currentCutscene].SetCutsceneString(cutsceneStringsEnglish[currentCutscene]);
                break;
        }
    }

    public void PlayNextCutscene()
    {
        if (currentCutscene < cutscenes.Length)
        {
            SetCutsceneStringsBasedOnLanguage();
            cutscenes[currentCutscene].DisplayText();
            currentCutscene++;
        }
        else
        {
            LoadNextScene(nextScene);
        }
    }

}
