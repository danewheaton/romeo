using UnityEngine;
using System.Collections;

// all code by CK unless otherwise noted

public class EnterBossFight : MonoBehaviour
{
    [SerializeField]
    GameObject Slider;

    [SerializeField]
    AudioSource music;

    [SerializeField]
    AudioClip bossTheme;

    void Awake ()
    {
        Slider.SetActive(false);
    }

    void OnTriggerEnter2D (Collider2D other)
    { 
        if (other.gameObject.tag == "Player")
        Slider.SetActive(true);
        music.clip = bossTheme;
        music.Play();
    }
}
