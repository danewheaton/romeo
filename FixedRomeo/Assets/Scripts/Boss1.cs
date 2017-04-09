using UnityEngine;
using System.Collections;

public enum Boss1States { INACTIVE, COMBAT, CHARGE_ATTACK, INJURED, DEAD }
public enum Attacks { PUNCH, STOMP, CHARGE }

public class Boss1 : MonoBehaviour
{
    public delegate void Stomp();
    public static event Stomp OnBoss1Stomp;

    [SerializeField] float speed = 2, combatRange = 30, verticalReach = 5,  injuredTimer = 5, deathTimer = 1;

    Transform sigma;
    Boss1States currentState = Boss1States.INACTIVE;
    Attacks currentAttack;

    Rigidbody2D myRigidbody;
    SpriteRenderer myRenderer;
    Animator myAnim;
    
	void Start ()
    {
        sigma = GameObject.FindGameObjectWithTag("Player").transform;

        myRigidbody = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        myAnim = GetComponent<Animator>();
	}
	
	void Update ()
    {
        switch (currentState)
        {
            case Boss1States.INACTIVE:
                UpdateInactiveBehavior();
                break;
            case Boss1States.COMBAT:
                UpdateAttackingBehavior();
                break;
            case Boss1States.INJURED:
                UpdateInjuredBehavior();
                break;
            case Boss1States.DEAD:
                // TODO: add a check so Die doesn't get called every frame when the boss dies, or just delete this from the states
                UpdateDie();
                break;
        }
    }

    void UpdateInactiveBehavior()
    {
        if (Vector2.Distance(transform.position, sigma.position) < combatRange)
            currentState = Boss1States.COMBAT;
    }

    void UpdateAttackingBehavior()
    {
        if (sigma.position.x < transform.position.x) myRenderer.flipX = false;
        else myRenderer.flipX = true;

        Vector2.MoveTowards(transform.position, sigma.position, speed * Time.deltaTime);

        if (sigma.position.y > (transform.position.y + verticalReach))
        {
            // prowl around, frustrated
            // stomp every once in a while
        }
        else if (sigma.position.y < (transform.position.y - verticalReach))
        {
            // prowl around, frustrated
        }
        else
        {
            // move toward sigma if sigma is too far away, otherwise keep sigma in range of punch attacks
            // stomp every once in a while if sigma is not quite in range of punch
            // punch every once in a while
            // charge every once in a longer while
        }
    }

    void UpdateInjuredBehavior()
    {
        // should probably stay empty, since mega man can't do anything while injured and the timer was started in OnCollisionEnter2D
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                // injure player? probably not
                break;
            case "Bullet":
                // play injured anim, but no health stuff for right now - that will be handled with a variant of the existing enemy health script
                break;
            case "Boss1Wall":
                if (currentState == Boss1States.CHARGE_ATTACK)
                {
                    currentState = Boss1States.INJURED;
                    myAnim.SetTrigger("HitWall");
                    StartCoroutine(InjurySequence());
                }
                break;
        }
    }

    void UpdateDie()
    {
        // this function might get deleted, since death is an event, not a state
    }

    void Attack(Attacks attack)
    {
        switch (attack)
        {
            case Attacks.PUNCH:
                myAnim.SetTrigger("Punch");
                break;
            case Attacks.STOMP:
                myAnim.SetTrigger("Stomp");
                if (OnBoss1Stomp != null) OnBoss1Stomp();
                // instantiate shockwave
                break;
            case Attacks.CHARGE:
                myAnim.SetTrigger("Charge");
                break;
        }
    }

    IEnumerator InjurySequence()
    {
        yield return new WaitForSeconds(injuredTimer);
        currentState = Boss1States.COMBAT;
    }

    IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(deathTimer);
    }
}
