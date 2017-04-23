using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            //if player health is not already at 100
            if (true)
            {
                playerHealth.TakeDamage(-3);
            }
            Destroy(gameObject);
        }
    }
}
