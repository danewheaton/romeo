using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, IDamageable
{
	public float moveSpeed = 2f;
	public int health = 2;
	public AudioClip[] deathClips;

	Transform frontCheck;
    Rigidbody2D myRigidbody;

	bool dead = false;

	
	void Start()
        
	{
        myRigidbody = GetComponent<Rigidbody2D>();
		frontCheck = transform.Find("frontCheck").transform;
	}

	void FixedUpdate ()
	{
        #region pseudo-AI for running around and bumping into stuff
        Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, 1);
        
		foreach(Collider2D c in frontHits)
		{
			if(c.tag != "Player")
			{
				Flip ();
				break;
			}
		}
        
		myRigidbody.velocity = new Vector2(transform.localScale.x * moveSpeed, myRigidbody.velocity.y);
        #endregion

        if (health <= 0 && !dead) Die();
	}
	
	public void TakeDamage(int hurtAmount)
	{
		health -= hurtAmount;

        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ChangeSpriteColorBack", .1f);
    }

    void ChangeSpriteColorBack()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    void Die()
	{
        #region enemy falls through ground - placeholder effect
        Collider2D[] cols = GetComponents<Collider2D>();
		foreach(Collider2D c in cols) c.isTrigger = true;
        #endregion

        int i = Random.Range(0, deathClips.Length);
		AudioSource.PlayClipAtPoint(deathClips[i], transform.position);

        dead = true;
    }


    public void Flip()
	{
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;
	}
}
