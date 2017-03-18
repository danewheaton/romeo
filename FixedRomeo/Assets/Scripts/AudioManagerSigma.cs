using UnityEngine;
using System.Collections;

//So that way any script this Audio Manager is attached to will have an AudioSource with it
[RequireComponent(typeof(AudioSource))]
public class AudioManagerSigma : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] audioClips = new AudioClip[7];

    private AudioSource audioSource;

	// Use this for initialization
	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    //Each sound has a different function, in case anything wants to be done with things like the audio mixer, reverb, etc
    private void PlayJump()
    {
        //Jump is the first element in the audioclips array
        audioSource.clip = audioClips[0];
        audioSource.Play();
    }

    private void PlayShoot()
    {
        //Shoot is the second element in the audioclips array
        //TODO: Find good way to sort through "shoot" clip variants
        audioSource.clip = audioClips[1];
        audioSource.Play();
    }

    private void PlayCharge()
    {
        //TODO: Play "windup" charge sound effect, then proceed to loop through a second sound effect of "being charged"
    }

    private void PlaySteps()
    {
        //TODO: Make sure each step is properly slightly varried for each "step"
    }

    private void PlayWallClimb()
    {
        //WallClimb is the fifth element in the audioclips array
        audioSource.clip = audioClips[4];
        audioSource.Play();
    }

    private void PlayDamage()
    {
        //Damage is the sixth element in the audioclips array
        audioSource.clip = audioClips[5];
        audioSource.Play();
    }

    private void PlayDie()
    {
        //Die is the seventh element in the audioclips array
        audioSource.clip = audioClips[7];
        audioSource.Play();
    }

    //function can be called with an argument of which desired sound must be played
    public void PlaySound(string soundToPlay)
    {
        switch (soundToPlay)
        {
            case "jump":
                PlayJump();
                break;
            case "shoot":
                PlayShoot();
                break;
            case "charge":
                PlayCharge();
                break;
            case "step":
                PlaySteps();
                break;
            case "wallclimb":
                PlayWallClimb();
                break;
            case "damage":
                PlayDamage();
                break;
            case "die":
                PlayDie();
                break;
            default:
                break;
        }
    }
}
