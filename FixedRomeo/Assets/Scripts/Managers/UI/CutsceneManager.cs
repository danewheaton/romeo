﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

// all code by CH unless otherwise noted

public enum Language { English, Spanish, Polish }

[RequireComponent(typeof(AudioSource))]
public class CutsceneManager : MonoBehaviour
{
    [SerializeField]
    private Cutscene[] cutscenes;
    [SerializeField]
    private string[] cutsceneStringsEnglish;
    [SerializeField]
    private AudioClip[] cutsceneVOEnglish;
    [SerializeField]
    private string[] cutsceneStringsSpanish;
    [SerializeField]
    private AudioClip[] cutsceneVOSpanish;
    [SerializeField]
    private string[] cutsceneStringsPolish;
    [SerializeField]
    private AudioClip[] cutsceneVOPolish;
    [SerializeField]
    private string nextScene;
    [SerializeField]
    private Language language;
    [SerializeField]
    private GameObject languageSelectorScreen;

    private int currentCutscene = 0;
    private bool languageSelected = false;
    private AudioSource VoiceOverSource;
    private AudioClip[] VoiceOverClips;


	// Use this for initialization
	void Start ()
    {
        VoiceOverSource = GetComponent<AudioSource>();
        //PlayNextCutscene();
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
        switch (language)
        {
            case Language.English:
                cutscenes[currentCutscene].SetCutsceneString(cutsceneStringsEnglish[currentCutscene]);
                VoiceOverClips = cutsceneVOEnglish;
                break;
            case Language.Spanish:
                cutscenes[currentCutscene].SetCutsceneString(cutsceneStringsSpanish[currentCutscene]);
                VoiceOverClips = cutsceneVOSpanish;
                break;
            case Language.Polish:
                cutscenes[currentCutscene].SetCutsceneString(cutsceneStringsPolish[currentCutscene]);
                VoiceOverClips = cutsceneVOPolish;
                break;
            default:
                cutscenes[currentCutscene].SetCutsceneString(cutsceneStringsEnglish[currentCutscene]);
                break;
        }
    }

    public void PlayNextCutscene()
    {
        if (languageSelected == true)
        {
            if (currentCutscene < cutscenes.Length)
            {
                SetCutsceneStringsBasedOnLanguage();
                cutscenes[currentCutscene].DisplayText();
                VoiceOverSource.clip = VoiceOverClips[currentCutscene];
                VoiceOverSource.Play();
                currentCutscene++;
            }
            else
            {
                LoadNextScene(nextScene);
            }
        }
    }

    public void ChangeLanguage(string setLanguage)
    {
        switch (setLanguage)
        {
            case "english":
                language = Language.English;
                break;
            case "spanish":
                language = Language.Spanish;
                break;
            case "polish":
                language = Language.Polish;
                break;
            default:
                language = Language.English;
                break;
        }
        languageSelected = true;
        languageSelectorScreen.SetActive(false);
        PlayNextCutscene();
    }
}
