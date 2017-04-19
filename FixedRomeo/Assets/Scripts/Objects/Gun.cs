using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public class Gun : MonoBehaviour
{
	public Rigidbody2D energyBlast, energyBlast2, energyBlast3;
	public float speed = 20f, energyBlast2ChargeAmount = .25f, energyBlast3ChargeAmount = .75f;

	PlatformerCharacter2D playerCtrl;
	Animator anim;
    AudioSource source;

    float chargeAmount = 0;

    void Start()
	{
		anim = transform.root.gameObject.GetComponent<Animator>();
		playerCtrl = transform.root.GetComponent<PlatformerCharacter2D>();
        source = GetComponent<AudioSource>();
	}


	void Update ()
	{
        if (Input.GetButton("Fire1")) chargeAmount += Time.deltaTime;

        if (chargeAmount > .2f && chargeAmount < .22f) anim.SetTrigger("Charge");

        Rigidbody2D projectile;
        if (chargeAmount > energyBlast3ChargeAmount) projectile = energyBlast3;
        else if (chargeAmount > energyBlast2ChargeAmount) projectile = energyBlast2;
        else projectile = energyBlast;
        
		if(Input.GetButtonUp("Fire1"))
		{
			anim.SetTrigger("Shoot");
			source.Play();
            
			if(playerCtrl.m_FacingRight)
			{
				Rigidbody2D bulletInstance = Instantiate(projectile, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(speed, 0);
			}
			else
			{
				Rigidbody2D bulletInstance = Instantiate(projectile, transform.position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(-speed, 0);
			}

            chargeAmount = 0;
		}
	}
}
