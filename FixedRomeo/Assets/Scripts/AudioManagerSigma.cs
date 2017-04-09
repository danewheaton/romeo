using UnityEngine;
using System.Collections;

//So that way any script this Audio Manager is attached to will have an AudioSource with it
[RequireComponent(typeof(AudioSource))]
public class AudioManagerSigma : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] audioClips = new AudioClip[7];

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioSource shootingAudioSource;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Jump"))
        {
            PlayJump();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            PlayCharge();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            shootingAudioSource.Stop();
        }
	}

    //Each sound has a different function, in case anything wants to be done with things like the audio mixer, reverb, etc
    private void PlayJump()
    {
        //Jump is the first element in the audioclips array
        audioSource.clip = audioClips[0];
        audioSource.Play();
    }

    private void PlayCharge()
    {
        //TODO: Play "windup" charge sound effect, then proceed to loop through a second sound effect of "being charged"
        shootingAudioSource.clip = audioClips[1];
        shootingAudioSource.loop = true;
        shootingAudioSource.Play();
    }

    private void PlayWallClimb()
    {
        //WallClimb is the third element in the audioclips array
        audioSource.clip = audioClips[2];
        audioSource.Play();
    }

    private void PlayDamage()
    {
        //Damage is the fourth element in the audioclips array
        audioSource.clip = audioClips[3];
        audioSource.Play();
    }

    private void PlayDie()
    {
        //Die is the fifth element in the audioclips array
        audioSource.clip = audioClips[4];
        audioSource.Play();
    }

    //function can be called with an argument of which desired sound must be played
    //public void PlaySound(string soundToPlay)
    //{
    //    switch (soundToPlay)
    //    {
    //        case "jump":
    //            PlayJump();
    //            break;
    //        case "shoot":
    //            PlayShoot();
    //            break;
    //        case "charge":
    //            PlayCharge();
    //            break;
    //        case "step":
    //            PlaySteps();
    //            break;
    //        case "wallclimb":
    //            PlayWallClimb();
    //            break;
    //        case "damage":
    //            PlayDamage();
    //            break;
    //        case "die":
    //            PlayDie();
    //            break;
    //        default:
    //            break;
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            PlayDamage();
        }
    }
}
