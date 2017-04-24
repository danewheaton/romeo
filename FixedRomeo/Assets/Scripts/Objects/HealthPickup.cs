using UnityEngine;
using System.Collections;

//All code authored by CH unless stated
[RequireComponent(typeof(AudioSource))]
public class HealthPickup : MonoBehaviour
{
    private AudioSource healthPickupSound;
    private SpriteRenderer sprite;
    private BoxCollider2D boxCollider2d;
    private bool isPickedUp = false;
	// Use this for initialization
	void Start ()
    {
        healthPickupSound = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
        boxCollider2d = GetComponent<BoxCollider2D>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            healthPickupSound.Play();

            //if player health is not already at 100
            if (playerHealth.Health <100 && isPickedUp == false)
            {
                if (playerHealth.Health <= 97)
                {
                    playerHealth.TakeDamage(-3);
                }
                if (playerHealth.Health == 98)
                {
                    playerHealth.TakeDamage(-2);
                }
                if (playerHealth.Health == 99)
                {
                    playerHealth.TakeDamage(-1);
                }
            }
            sprite.enabled = false;
            isPickedUp = true;
            boxCollider2d.enabled = false;
        }
    }
}
