using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//ALL authored by Chris Harper unless otherwise mentioned
public class PreCutscene : MonoBehaviour
{
    [SerializeField]
    private string nextScene;

    private Renderer r;

    private RawImage imageRenderer;

    private MovieTexture movie;

    private AudioSource movieAudio;

    void Awake()
    {
        r = GetComponent<Renderer>();
        movieAudio = GetComponent<AudioSource>();
        movie = (MovieTexture)r.material.mainTexture;
        movieAudio.clip = movie.audioClip;
    }

	// Use this for initialization
	void Start ()
    {
        PlayMovieAudio();
        movie.Play();
    }

    private void PlayMovieAudio()
    {
        movieAudio.Play();
    }

    // Update is called once per frame
    void Update ()
    {
        Debug.Log("Movie is playing: " + movie.isPlaying.ToString());
        if (movie.isPlaying == false)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
	}
}
