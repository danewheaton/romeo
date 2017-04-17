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

	void OnCollisionEnter2D (Collision2D col)
	{
		if(col.gameObject.tag == "Enemy" || col.gameObject.tag == "Boss")
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


	void TakeDamage (Transform enemy)   // TODO: why isn't the health logic here? move it out of UIManager plz
	{
		// create a vector that's from the enemy to the player with an upwards boost
		Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;

		// add a force to the player in the direction of the vector and multiply by the hurtForce
		GetComponent<Rigidbody2D>().AddForce(hurtVector * hurtForce);
        
		int i = Random.Range (0, ouchClips.Length);
		AudioSource.PlayClipAtPoint(ouchClips[i], transform.position);
	}

}
