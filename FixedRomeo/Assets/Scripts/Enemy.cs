﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public float moveSpeed = 2f;
	public int health = 2;
	public AudioClip[] deathClips;

	Transform frontCheck;

	bool dead = false;

	
	void Start()
	{
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
        
		GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
        #endregion

        if (health <= 0 && !dead) Die();
	}
	
	public void Hurt(int hurtAmount)
	{
		health -= hurtAmount;
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