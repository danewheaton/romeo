using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// all code by CK unless otherwise noted

public class BossHealth : MonoBehaviour, IDamageable
{
    public delegate void Death();
    public static event Death OnDeath;

    [SerializeField]
    Slider HealthSlider;

    [SerializeField]
    int health = 10;

    bool dead;

    void Start ()
    {
        HealthSlider.value = health;

    }
    void Update()
    {
        HealthSlider.value = health;
        if (health <= 0 && !dead) Die();
    }

    public void TakeDamage(int hurtAmount)
    {
        health -= hurtAmount;
    }

    public void Die()
    {
        dead = true;
        if (OnDeath != null) OnDeath();
    }
}
