using UnityEngine;
using System.Collections;

public enum Boss1States { INACTIVE, COMBAT, CHARGE_ATTACK, INJURED, DEAD }
public enum Attacks { PUNCH, STOMP, CHARGE }    // TODO: replace weird random coroutines with a function that takes in these - mega man can only do one attack at a time, after all

public class Boss1 : MonoBehaviour
{
    public delegate void Stomp();
    public static event Stomp OnBoss1Stomp;

    [SerializeField] float
        speed = 5,
        stompForce = 500,
        punchFrequencyMin = .5f,
        punchFrequencyMax = 3,
        stompFrequencyMin = 2,
        stompFrequencyMax = 6,
        chargefrequencyMin = 4,
        chargeFrequencyMax = 8,
        activationRange = 30,
        verticalReach = 1, 
        injuredTimer = 5,
        deathTimer = 1;
    [SerializeField] Transform navPointLeft, navPointRight;
    [SerializeField] GameObject blastWave;
    
    Boss1States currentState = Boss1States.INACTIVE;

    Transform sigmaTransform;
    Rigidbody2D myRigidbody;
    SpriteRenderer myRenderer;
    Animator myAnim;

    Color originalColor;

    Vector3 targetPos, originalBlastWaveScale;
    bool canStomp = true, canCharge = true, charging;

    void Start ()
    {
        sigmaTransform = GameObject.FindGameObjectWithTag("Player").transform;

        myRigidbody = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        myAnim = GetComponent<Animator>();

        targetPos = navPointLeft.position;
        
        originalBlastWaveScale = blastWave.transform.localScale;
        blastWave.SetActive(false);
        originalColor = myRenderer.color;
    }
	
	void Update ()
    {
        switch (currentState)
        {
            case Boss1States.INACTIVE:
                UpdateInactiveBehavior();
                break;
            case Boss1States.COMBAT:
                UpdateCombatBehavior();
                break;
            case Boss1States.CHARGE_ATTACK:
                UpdateChargeBehavior();
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
        if (Vector2.Distance(transform.position, sigmaTransform.position) < activationRange)
            currentState = Boss1States.COMBAT;
    }

    void UpdateCombatBehavior()
    {
        if (sigmaTransform.position.y > (transform.position.y + verticalReach))
        {
            UpdatePatrol();
            if (canStomp) StartCoroutine(StompRandomly());
            StopCoroutine(Charge());
        }
        else if (sigmaTransform.position.y < (transform.position.y - verticalReach))
        {
            UpdatePatrol();
            StopCoroutine(StompRandomly());
            StopCoroutine(Charge());
        }
        else
        {
            if (sigmaTransform.position.x < transform.position.x) myRenderer.flipX = false;
            else myRenderer.flipX = true;

            if (!canCharge) myRigidbody.MovePosition(transform.position + (myRenderer.flipX ? transform.right : -transform.right) * speed * Time.deltaTime); // placeholder

            if (canStomp) StartCoroutine(StompRandomly());
            if (canCharge) StartCoroutine(ChargeRandomly());

            // move toward sigma if sigma is too far away, otherwise keep sigma in range of punch attacks
            // stomp every once in a while if sigma is not quite in range of punch
            // punch every once in a while
            // charge every once in a longer while
        }
    }

    void UpdateChargeBehavior()
    {
        if (charging) StartCoroutine(Charge());
    }

    void UpdateInjuredBehavior()
    {
        // should probably stay empty, since mega man can't do anything while injured and the timer was started in OnCollisionEnter2D
    }

    void UpdatePatrol()
    {
        if (Vector2.Distance(transform.position, targetPos) < 3)
            targetPos = (targetPos == navPointLeft.position ? navPointRight.position : navPointLeft.position);

        myRenderer.flipX = (targetPos == navPointLeft.position ? false : true);
        myRigidbody.MovePosition(transform.position + (targetPos == navPointLeft.position ? -transform.right : transform.right) * speed * Time.deltaTime); // placeholder
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

    IEnumerator StompRandomly()
    {
        canStomp = false;
        
        yield return new WaitForSeconds(Random.Range(stompFrequencyMin + 0f, stompFrequencyMax + 1f));

        StopCoroutine(Charge());

        myRigidbody.AddForce(Vector2.up * stompForce);
        if (OnBoss1Stomp != null) OnBoss1Stomp();

        blastWave.SetActive(true);

        float elapsedTime = 0;
        while (elapsedTime < 1)
        {
            blastWave.transform.localScale = new Vector3(blastWave.transform.localScale.x + 2, blastWave.transform.localScale.y, blastWave.transform.localScale.z);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        blastWave.transform.localScale = originalBlastWaveScale;
        blastWave.SetActive(false);

        canStomp = true;
    }

    IEnumerator ChargeRandomly()
    {
        canCharge = false;

        yield return new WaitForSeconds(Random.Range(chargefrequencyMin + 0f, chargeFrequencyMax + 1f));

        StopCoroutine(StompRandomly());

        currentState = Boss1States.CHARGE_ATTACK;

        canCharge = true;
        charging = true;
    }

    IEnumerator Charge()
    {
        canCharge = false;
        
        myRigidbody.velocity = Vector2.zero;

        float elapsedTime = 0;
        while (elapsedTime < 3)
        {
            myRenderer.color = Color.Lerp(originalColor, Color.red, elapsedTime / 3);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        myRigidbody.AddForce((myRenderer.flipX ? Vector2.right : Vector2.left) * 2000);
        myRenderer.color = originalColor;

        canCharge = true;
    }

    IEnumerator InjurySequence()
    {
        yield return new WaitForSeconds(injuredTimer);
        myRenderer.color = originalColor;
        currentState = Boss1States.COMBAT;
    }

    IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(deathTimer);
    }
}
