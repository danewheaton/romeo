using UnityEngine;
using System.Collections;

//This manager is attached on the trigger used to switch the music!
[RequireComponent(typeof(AudioSource))]

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip bossTheme;
    [SerializeField]
    private AudioClip stageTheme;

    private AudioSource mainAudioSource;

    private void OnEnable()
    {
        SigmaTriggers.OnEnterArena += PlayBossMusic;
    }
    private void OnDisable()
    {
        SigmaTriggers.OnEnterArena -= PlayBossMusic;
    }
    
    void Start ()
    {
        mainAudioSource = GetComponent<AudioSource>();
        mainAudioSource.clip = stageTheme;
        mainAudioSource.Play();
	}

    void PlayBossMusic()
    {
        mainAudioSource.Stop();
        mainAudioSource.clip = bossTheme;
        mainAudioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            mainAudioSource.Stop();
            mainAudioSource.clip = bossTheme;
            mainAudioSource.Play();
        }
    }
}
