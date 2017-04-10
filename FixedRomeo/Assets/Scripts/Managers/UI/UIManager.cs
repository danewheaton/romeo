using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    SigmaTriggers ST;

    [SerializeField]
    public Slider HealthSlider;
    [SerializeField]
    public float PlayerHealth, repeatDamagePeriod = 2f, hurtForce = 10f, damageAmount = 10f;

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

    void TakeDamage()
    {
        PlayerHealth -= 10f;

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if (Time.time > lastHitTime + repeatDamagePeriod) // if past cool-down time
            {

                if (PlayerHealth > 0f)
                {
                    TakeDamage();
                    lastHitTime = Time.time - 1f;
                }

                if (PlayerHealth <= 0f)
                {
                    PlayersDeath();

                }
            }
        }

    }
    void OnTriggerEnter2D (Collider2D col)
    { 
        if (col.gameObject.tag == "Killzone")
        {
            PlayersDeath();

        }
    }

    public void PlayersDeath ()
    {

        PlayerHealth = 100f;
        ST.PlayerDeath();
    }
}