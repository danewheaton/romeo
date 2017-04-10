using UnityEngine;
using System.Collections;

public enum Boss1States { INACTIVE, COMBAT, CHARGE_ATTACK, INJURED }
public enum Attacks { PUNCH, STOMP, CHARGE }    // TODO: replace weird random coroutines with a function that takes in these - mega man can only do one attack at a time, after all

public class Boss1 : MonoBehaviour
{
    public delegate void Stomp();
    public static event Stomp OnBoss1Stomp;

    [SerializeField] float
        speed = 5,
        stompForce = 500,
        chargeForce = 20000,
        punchFrequencyMin = .5f,
        punchFrequencyMax = 3,
        stompFrequencyMin = 2,
        stompFrequencyMax = 6,
        chargeFrequencyMin = 4,
        chargeFrequencyMax = 8,
        activationRange = 30,
        verticalReach = 3, 
        injuredTimer = 5,
        deathTimer = 1;
    [SerializeField] Transform navPointLeft, navPointRight;
    [SerializeField] GameObject blastWave, weakSpot;
    
    Boss1States currentState = Boss1States.INACTIVE;

    Transform sigmaTransform;
    Rigidbody2D myRigidbody;
    SpriteRenderer myRenderer;
    Animator myAnim;

    Color originalColor;

    Vector3 targetPos, originalBlastWaveScale;
    bool canAttack = true, charging;

    void Start ()
    {
        sigmaTransform = GameObject.FindGameObjectWithTag("Player").transform;

        myRigidbody = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        myAnim = GetComponent<Animator>();

        targetPos = navPointLeft.position;
        
        originalBlastWaveScale = blastWave.transform.localScale;
        blastWave.SetActive(false);
        weakSpot.SetActive(false);
        originalColor = myRenderer.color;
    }
	
	void Update ()
    {
        // I think there's too much getting called every frame - especially attacks

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
            if (canAttack) StartCoroutine(AttackRandomly(Attacks.STOMP));
            StopCoroutine(AttackRandomly(Attacks.PUNCH));
            StopCoroutine(AttackRandomly(Attacks.CHARGE));
        }
        else if (sigmaTransform.position.y < (transform.position.y - verticalReach))
        {
            UpdatePatrol();
            StopCoroutine(AttackRandomly(Attacks.PUNCH));
            StopCoroutine(AttackRandomly(Attacks.STOMP));
            StopCoroutine(AttackRandomly(Attacks.CHARGE));
        }
        else
        {
            if (sigmaTransform.position.x < transform.position.x) myRenderer.flipX = false;
            else myRenderer.flipX = true;

            if (!charging) myRigidbody.MovePosition(transform.position + (myRenderer.flipX ? transform.right : -transform.right) * speed * Time.deltaTime); // placeholder

            if (canAttack) StartCoroutine(AttackRandomly((Attacks)Random.Range(0, 3)));

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
                if (currentState == Boss1States.CHARGE_ATTACK)
                {
                    StartCoroutine(GoBackToCombatState());
                }
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

    IEnumerator AttackRandomly(Attacks attackType)
    {
        canAttack = false;

        switch (attackType)
        {
            case Attacks.PUNCH:
                break;
            case Attacks.STOMP:

                yield return new WaitForSeconds(Random.Range(stompFrequencyMin + 0f, stompFrequencyMax + 1f));

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

                break;
            case Attacks.CHARGE:
                yield return new WaitForSeconds(Random.Range(chargeFrequencyMin + 0f, chargeFrequencyMax + 1f));
                currentState = Boss1States.CHARGE_ATTACK;
                charging = true;
                break;
        }

        canAttack = true;
    }

    IEnumerator Charge()
    {
        charging = false;

        StopCoroutine(AttackRandomly(Attacks.PUNCH));
        StopCoroutine(AttackRandomly(Attacks.STOMP));
        StopCoroutine(AttackRandomly(Attacks.CHARGE));

        myRigidbody.velocity = Vector2.zero;
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        float elapsedTime = 0;
        float timer = 3;
        while (elapsedTime < timer)
        {
            myRenderer.color = Color.Lerp(originalColor, Color.red, elapsedTime / timer);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        myRigidbody.AddForce((myRenderer.flipX ? Vector2.right : Vector2.left) * 20000);
    }

    IEnumerator InjurySequence()
    {
        weakSpot.SetActive(true);

        #region blink red
        float elapsedTime = 0;
        while (elapsedTime < (injuredTimer / 10))
        {
            myRenderer.color = Color.Lerp(Color.red, originalColor, elapsedTime / (injuredTimer / 10));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
        while (elapsedTime < (injuredTimer / 10))
        {
            myRenderer.color = Color.Lerp(originalColor, Color.red, elapsedTime / (injuredTimer / 10));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
        while (elapsedTime < (injuredTimer / 10))
        {
            myRenderer.color = Color.Lerp(Color.red, originalColor, elapsedTime / (injuredTimer / 10));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
        while (elapsedTime < (injuredTimer / 10))
        {
            myRenderer.color = Color.Lerp(originalColor, Color.red, elapsedTime / (injuredTimer / 10));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
        while (elapsedTime < (injuredTimer / 10))
        {
            myRenderer.color = Color.Lerp(Color.red, originalColor, elapsedTime / (injuredTimer / 10));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
        while (elapsedTime < (injuredTimer / 10))
        {
            myRenderer.color = Color.Lerp(originalColor, Color.red, elapsedTime / (injuredTimer / 10));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
        while (elapsedTime < (injuredTimer / 10))
        {
            myRenderer.color = Color.Lerp(Color.red, originalColor, elapsedTime / (injuredTimer / 10));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
        while (elapsedTime < (injuredTimer / 10))
        {
            myRenderer.color = Color.Lerp(originalColor, Color.red, elapsedTime / (injuredTimer / 10));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
        while (elapsedTime < (injuredTimer / 10))
        {
            myRenderer.color = Color.Lerp(Color.red, originalColor, elapsedTime / (injuredTimer / 10));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
        while (elapsedTime < (injuredTimer / 10))
        {
            myRenderer.color = Color.Lerp(originalColor, Color.red, elapsedTime / (injuredTimer / 10));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
        while (elapsedTime < (injuredTimer / 10))
        {
            myRenderer.color = Color.Lerp(Color.red, originalColor, elapsedTime / (injuredTimer / 10));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        #endregion

        weakSpot.SetActive(false);
        myRigidbody.constraints = RigidbodyConstraints2D.None;

        currentState = Boss1States.COMBAT;
    }

    IEnumerator GoBackToCombatState()
    {
        yield return new WaitForSeconds(injuredTimer);

        myRenderer.color = originalColor;
        myRigidbody.constraints = RigidbodyConstraints2D.None;

        currentState = Boss1States.COMBAT;
    }

    public IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(deathTimer);
        GameStateManager.won = true;
    }
}
