using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IDamageable
{
    SigmaTriggers ST;

    [SerializeField]
    public Slider HealthSlider;
    [SerializeField]
    public float PlayerHealth, repeatDamagePeriod = 2f, hurtForce = 10f, damageAmount = 10f;
    [SerializeField]
    Image panel;

    float lastHitTime;


    // Use this for initialization
    void Start()
    {
        ST = GetComponent<SigmaTriggers>();
        HealthSlider.value = PlayerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        HealthSlider.value = PlayerHealth;
    }

    public void TakeDamage(int damage)
    {
        PlayerHealth -= damage;

        GetComponent<SpriteRenderer>().color = Color.red;
        panel.gameObject.SetActive(true);
        Invoke("ChangeSpriteColorBack", .2f);
    }

    void ChangeSpriteColorBack()
    {
        panel.gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if (Time.time > lastHitTime + repeatDamagePeriod) // if past cool-down time
            {

                if (PlayerHealth > 0f)
                {
                    TakeDamage(10);
                    lastHitTime = Time.time - 1f;
                }

                if (PlayerHealth <= 0f)
                {
                    Die();

                }
            }
        }

    }
    void OnTriggerEnter2D (Collider2D col)
    { 
        if (col.gameObject.tag == "Killzone")
        {
            Die();
        }
        else if (col.gameObject.tag == "Enemy")
        {
            if (Time.time > lastHitTime + repeatDamagePeriod) // if past cool-down time
            {

                if (PlayerHealth > 0f)
                {
                    TakeDamage(10);
                    lastHitTime = Time.time - 1f;
                }

                if (PlayerHealth <= 0f)
                {
                    Die();

                }
            }
        }
    }

    public void Die ()
    {
        PlayerHealth = 100f;
        ST.PlayerDeath();
    }
}