using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;
	[HideInInspector]
	public bool canJump = false;


	public float moveForce = 365f, maxSpeed = 5f, jumpForce = 1000f, tauntProbability = 50f, tauntDelay = 1f;
    public AudioClip[] jumpClips;

    Transform groundCheck;
	Animator anim;

    bool grounded;

    void Start()
	{
		groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator>();
	}


	void Update()
	{
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  
        
		if(Input.GetButtonDown("Jump") && grounded)
			canJump = true;
	}


	void FixedUpdate ()
	{
		float h = Input.GetAxis("Horizontal");
		anim.SetFloat("Speed", Mathf.Abs(h));

        #region weird movement logic
        if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);
		if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
			GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
        #endregion

        if ((h > 0 && !facingRight) || (h < 0 && facingRight)) Flip();
        if (canJump) Jump();
	}
	
	void Flip ()
	{
		facingRight = !facingRight;
        
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    void Jump()
    {
        anim.SetTrigger("Jump");
        int i = Random.Range(0, jumpClips.Length);
        AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
        canJump = false;
    }
}
