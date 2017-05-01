using UnityEngine;
using System.Collections;

// all code by DW unless otherwise noted

public enum Boss2States { INACTIVE, PATROLLING, MOVING_TOWARD_SIGMA, STOMP_ATTACK, CHARGE_ATTACK, INJURED }

public class Boss2 : MonoBehaviour
{
    public delegate void Stomp();
    public static event Stomp OnBoss1Stomp;

    [SerializeField]
    float
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
        range = 3,
        injuredTimer = 5,
        deathTimer = 1;
    [SerializeField]
    Transform navPointLeft, navPointRight;
    [SerializeField]
    GameObject blastWave, gunObject;

    Boss2States currentState = Boss2States.INACTIVE;

    Gun gun;
    Rigidbody2D myRigidbody;
    SpriteRenderer myRenderer;
    Animator myAnim;
    Transform sigmaTransform;

    Color originalRendererColor;

    Vector3 targetPos, originalBlastWaveScale;
    bool canAttack = true;

    private void OnEnable()
    {
        BossHealth.OnDeath += CallDeathSequence;
    }
    private void OnDisable()
    {
        BossHealth.OnDeath -= CallDeathSequence;
    }

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        myAnim = GetComponent<Animator>();
        gun = GetComponentInChildren<Gun>();

        sigmaTransform = GameObject.FindGameObjectWithTag("Player").transform;

        originalRendererColor = myRenderer.color;
        originalBlastWaveScale = blastWave.transform.localScale;

        targetPos = navPointLeft.position;

        blastWave.SetActive(false);
    }

    void Update()
    {
        print(currentState);

        switch (currentState)
        {
            case Boss2States.INACTIVE:
                UpdateInactiveBehavior();
                break;
            case Boss2States.PATROLLING:
                UpdatePatrolBehavior();
                break;
            case Boss2States.MOVING_TOWARD_SIGMA:
                UpdateMoveTowardSigmaBehavior();
                break;
            case Boss2States.STOMP_ATTACK:
                // should be empty - behavior invoked while in MOVING_TOWARD_SIGMA state
                break;
            case Boss2States.CHARGE_ATTACK:
                // should be empty - behavior handled via coroutine
                break;
            case Boss2States.INJURED:
                // should be empty - behavior handled via coroutine
                break;
            default:
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                if (currentState == Boss2States.CHARGE_ATTACK)
                {
                    collision.gameObject.GetComponent<IDamageable>().TakeDamage(30);
                    myRenderer.color = originalRendererColor;
                    currentState = Boss2States.PATROLLING;
                }
                else collision.gameObject.GetComponent<IDamageable>().TakeDamage(10);
                break;
            case "Boss1Wall":
                if (currentState == Boss2States.CHARGE_ATTACK)
                {
                    currentState = Boss2States.INJURED;
                    myAnim.SetTrigger("HitWall");
                    StartCoroutine(InjurySequence());
                }
                break;
        }
    }

    void UpdateInactiveBehavior()
    {
        if (Vector2.Distance(transform.position, sigmaTransform.position) < activationRange)
            currentState = Boss2States.PATROLLING;
    }

    void UpdatePatrolBehavior()
    {
        // give up on attacking if sigma is out of reach
        StopCoroutine(WaitToAttack(Attacks.STOMP));
        StopCoroutine(WaitToAttack(Attacks.CHARGE));

        // switch patrol points if close to one
        if (Vector2.Distance(transform.position, targetPos) < 3)
            targetPos = (targetPos == navPointLeft.position ? navPointRight.position : navPointLeft.position);

        // move back and forth between patrol points
        myRenderer.flipX = (targetPos == navPointLeft.position ? false : true);
        myRigidbody.MovePosition(transform.position + (targetPos == navPointLeft.position ? -transform.right : transform.right) * speed * Time.deltaTime); // placeholder

        #region change state if sigma is in reach
        // sigma is in reach if he is not higher or lower than the boss's vertical reach
        bool sigmaIsInReach = Vector2.Distance(transform.position, sigmaTransform.position) < range;

        if (sigmaIsInReach) currentState = Boss2States.MOVING_TOWARD_SIGMA;
        #endregion
    }

    void UpdateMoveTowardSigmaBehavior()
    {
        // face sigma
        if (sigmaTransform.position.x < transform.position.x) myRenderer.flipX = false;
        else myRenderer.flipX = true;

        // aim at sigma
        gun.gameObject.transform.LookAt(sigmaTransform);

        // attack
        if (canAttack) StartCoroutine(RapidFire());

        #region change state if sigma is out of reach
        // sigma is out of reach if he is higher or lower than the boss's vertical reach
        bool sigmaIsOutOfReach = Vector2.Distance(transform.position, sigmaTransform.position) >= range;

        if (sigmaIsOutOfReach) currentState = Boss2States.PATROLLING;
        #endregion
    }

    IEnumerator WaitToAttack(Attacks attackType)
    {
        canAttack = false;

        switch (attackType)
        {
            case Attacks.STOMP:
                yield return new WaitForSeconds(Random.Range(stompFrequencyMin + 0f, stompFrequencyMax + 1f));
                StartCoroutine(Attack(Attacks.STOMP));
                break;
            case Attacks.CHARGE:
                yield return new WaitForSeconds(Random.Range(chargeFrequencyMin + 0f, chargeFrequencyMax + 1f));
                StartCoroutine(Attack(Attacks.CHARGE));
                break;
        }

        canAttack = true;
    }

    IEnumerator Attack(Attacks attackType)
    {
        switch (attackType)
        {
            case Attacks.STOMP:
                currentState = Boss2States.STOMP_ATTACK;
                #region stomp
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
                #endregion
                currentState = Boss2States.PATROLLING;
                break;
            case Attacks.CHARGE:
                currentState = Boss2States.CHARGE_ATTACK;
                #region charge
                myRigidbody.velocity = Vector2.zero;
                myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

                elapsedTime = 0;
                float timer = 3;
                while (elapsedTime < timer)
                {
                    myRenderer.color = Color.Lerp(originalRendererColor, Color.red, elapsedTime / timer);

                    elapsedTime += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }

                myRigidbody.AddForce((myRenderer.flipX ? Vector2.right : Vector2.left) * 20000);
                #endregion
                break;
        }
    }

    IEnumerator InjurySequence()
    {
        gunObject.SetActive(true);
        myRigidbody.isKinematic = true;

        #region blink red
        float elapsedTime = 0;
        while (elapsedTime < (injuredTimer / 10))
        {
            myRenderer.color = Color.Lerp(Color.red, originalRendererColor, elapsedTime / (injuredTimer / 10));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
        while (elapsedTime < (injuredTimer / 10))
        {
            myRenderer.color = Color.Lerp(originalRendererColor, Color.red, elapsedTime / (injuredTimer / 10));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
        while (elapsedTime < (injuredTimer / 10))
        {
            myRenderer.color = Color.Lerp(Color.red, originalRendererColor, elapsedTime / (injuredTimer / 10));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
        while (elapsedTime < (injuredTimer / 10))
        {
            myRenderer.color = Color.Lerp(originalRendererColor, Color.red, elapsedTime / (injuredTimer / 10));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
        while (elapsedTime < (injuredTimer / 10))
        {
            myRenderer.color = Color.Lerp(Color.red, originalRendererColor, elapsedTime / (injuredTimer / 10));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
        while (elapsedTime < (injuredTimer / 10))
        {
            myRenderer.color = Color.Lerp(originalRendererColor, Color.red, elapsedTime / (injuredTimer / 10));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
        while (elapsedTime < (injuredTimer / 10))
        {
            myRenderer.color = Color.Lerp(Color.red, originalRendererColor, elapsedTime / (injuredTimer / 10));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
        while (elapsedTime < (injuredTimer / 10))
        {
            myRenderer.color = Color.Lerp(originalRendererColor, Color.red, elapsedTime / (injuredTimer / 10));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
        while (elapsedTime < (injuredTimer / 10))
        {
            myRenderer.color = Color.Lerp(Color.red, originalRendererColor, elapsedTime / (injuredTimer / 10));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
        while (elapsedTime < (injuredTimer / 10))
        {
            myRenderer.color = Color.Lerp(originalRendererColor, Color.red, elapsedTime / (injuredTimer / 10));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
        while (elapsedTime < (injuredTimer / 10))
        {
            myRenderer.color = Color.Lerp(Color.red, originalRendererColor, elapsedTime / (injuredTimer / 10));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        myRenderer.color = originalRendererColor;
        #endregion

        gunObject.SetActive(false);
        myRigidbody.isKinematic = false;

        currentState = Boss2States.PATROLLING;
    }

    void CallDeathSequence()
    {
        StartCoroutine(DeathSequence());
    }

    public IEnumerator DeathSequence()
    {
        // TODO: play death animation
        yield return new WaitForSeconds(deathTimer);
        GameStateManager.won = true;
    }

    IEnumerator RapidFire()
    {
        Vector2 direction = myRenderer.flipX == true ? new Vector2(20, -20) : new Vector2(-20, -20);

        float rapidFireRate = .01f;
        gun.BossShoot(direction);
        yield return new WaitForSeconds(rapidFireRate);
        gun.BossShoot(direction);
        yield return new WaitForSeconds(rapidFireRate);
        gun.BossShoot(direction);
        yield return new WaitForSeconds(rapidFireRate);
        gun.BossShoot(direction);
        yield return new WaitForSeconds(rapidFireRate);
        gun.BossShoot(direction);
    }
}
