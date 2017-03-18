using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{	
	public float health = 100f, repeatDamagePeriod = 2f, hurtForce = 10f, damageAmount = 10f;
	public AudioClip[] ouchClips;

	SpriteRenderer healthBar;
	Vector3 healthScale;
	Animator anim;

    float lastHitTime;

    void Start ()
	{
		healthBar = GameObject.Find("HealthBar").GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
        
		healthScale = healthBar.transform.localScale;
	}


	void OnCollisionEnter2D (Collision2D col)
	{
		if(col.gameObject.tag == "Enemy")
		{
			if (Time.time > lastHitTime + repeatDamagePeriod) // if past cool-down time
			{
				if(health > 0f)
				{
					TakeDamage(col.transform); 
					lastHitTime = Time.time; 
				}
				else
				{
                    #region this makes Sigma fall through the map - placeholder effect
                    Collider2D[] cols = GetComponents<Collider2D>();
					foreach(Collider2D c in cols) c.isTrigger = true;
                    
					SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
					foreach(SpriteRenderer s in spr) s.sortingLayerName = "UI";
                    #endregion

                    GetComponent<PlayerControl>().enabled = false;
					GetComponentInChildren<Gun>().enabled = false;
					anim.SetTrigger("Die");
				}
			}
		}
	}


	void TakeDamage (Transform enemy)
	{
		// create a vector that's from the enemy to the player with an upwards boost
		Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;

		// add a force to the player in the direction of the vector and multiply by the hurtForce
		GetComponent<Rigidbody2D>().AddForce(hurtVector * hurtForce);
        
		health -= damageAmount;
		UpdateHealthBar();
        
		int i = Random.Range (0, ouchClips.Length);
		AudioSource.PlayClipAtPoint(ouchClips[i], transform.position);
	}


	public void UpdateHealthBar ()  // TODO: change this to behave like a megaman-style health bar
	{
		healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);
		healthBar.transform.localScale = new Vector3(healthScale.x * health * 0.01f, 1, 1);
	}
}
