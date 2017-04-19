using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// all code by CK unless otherwise noted

public class PlayerHealth : MonoBehaviour, IDamageable
{
    #region events - DW
    public delegate void Death();
    public static event Death OnDeath;
    #endregion

    [SerializeField]
    Slider HealthSlider;

    [SerializeField]
    Image panel;

    [SerializeField]
    float health = 100, repeatDamagePeriod = 2f, hurtForce = 10f, damageAmount = 10f, screenFlashOnDamageTime = .15f;

    float lastHitTime;
    
    void Start()
    {
        HealthSlider.value = health;
    }
    
    void Update()
    {
        HealthSlider.value = health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        #region DW
        GetComponent<SpriteRenderer>().color = Color.red;
        panel.gameObject.SetActive(true);
        Invoke("ChangeSpriteColorBack", screenFlashOnDamageTime);
        #endregion
    }

    void ChangeSpriteColorBack()    // DW
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
                if (health > 0f)
                {
                    TakeDamage(10);
                    lastHitTime = Time.time - 1f;
                }

                if (health <= 0f)
                {
                    Die();
                }
            }
        }

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Killzone")
        {
            Die();
        }
        else if (col.gameObject.tag == "Enemy")
        {
            if (Time.time > lastHitTime + repeatDamagePeriod) // if past cool-down time
            {

                if (health > 0f)
                {
                    TakeDamage(10);
                    lastHitTime = Time.time - 1f;
                }

                if (health <= 0f)
                {
                    Die();
                }
            }
        }
    }

    public void Die()
    {
        if (OnDeath != null) OnDeath(); // DW
        health = 100f;
    }
}
