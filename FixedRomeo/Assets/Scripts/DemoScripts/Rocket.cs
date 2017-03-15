using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour 
{
    public int damageModifier;
	public GameObject explosion;

	void Start () 
	{
		Destroy(gameObject, 2);
	}
	
	void OnTriggerEnter2D (Collider2D other) 
	{
		if(other.tag == "Enemy")
		{
			other.gameObject.GetComponent<Enemy>().Hurt(damageModifier);
			OnExplode();
			if (damageModifier < 3) Destroy(gameObject);
		}
		else if(other.gameObject.tag != "Player")
		{
			OnExplode();
            if (damageModifier < 3) Destroy(gameObject);
		}
	}

    void OnExplode()
    {
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        Instantiate(explosion, transform.position, randomRotation);
    }
}
