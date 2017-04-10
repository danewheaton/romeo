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
	// Use this for initialization
	void Start ()
    {
        mainAudioSource = GetComponent<AudioSource>();
        mainAudioSource.clip = stageTheme;
        mainAudioSource.Play();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
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
