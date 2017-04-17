using UnityEngine;
using System.Collections;

public class DaneTemp_Cutscene : MonoBehaviour
{
    [SerializeField]
    AudioSource adam;

    [SerializeField]
    AudioSource[] texts;

    [SerializeField]
    AudioClip[] clips;

    int clicks;
    bool polish;

    void Update()
    {
        if (Input.GetButtonDown("Polish") && !polish)
        {
            foreach (AudioSource a in texts) a.mute = true;
            if (clicks <= 3) adam.clip = clips[clicks];
            adam.Play();
            clicks++;
            polish = true;
        }
    }

    public void PlayAdamLines()
    {
        adam.clip = clips[clicks];
        adam.Play();
        clicks++;
    }
}
