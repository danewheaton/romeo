using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    [HideInInspector]
    public bool facingRight = true;

    public float moveForce = 365f, maxSpeed = 5f, jumpForce = 1000f, tauntProbability = 50f, tauntDelay = 1f;
    public AudioClip[] jumpClips;



    Transform groundCheck;
    Animator anim;
    Rigidbody2D rb;
    [SerializeField]
    bool grounded;

    void Start()
    {
        groundCheck = GameObject.Find("groundCheck").transform;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        grounded = Physics2D.OverlapCircleAll(groundCheck.position, .5f).Length > 2;

        print(Physics2D.OverlapCircleAll(groundCheck.position, .5f).Length);
    }

    void FixedUpdate()
    {
        if (grounded)
        {
            float h = Input.GetAxis("Horizontal");
            anim.SetFloat("Speed", Mathf.Abs(h));

            if (h * rb.velocity.x < maxSpeed)
                rb.AddForce(Vector2.right * h * moveForce);
            if (Mathf.Abs(rb.velocity.x) > maxSpeed)
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);    // mathf.sign? wtf?
            if ((h > 0 && !facingRight) || (h < 0 && facingRight)) Flip();

            if (Input.GetButtonDown("Jump")) Jump();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    void Flip()
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
        GetComponent<Rigidbody2D>().AddForce(new Vector2(facingRight ? jumpForce : -jumpForce, jumpForce));
    }


}
