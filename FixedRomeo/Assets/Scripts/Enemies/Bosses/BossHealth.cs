using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// IF THE BOSS'S HEALTH RESETS BACK TO 100% AFTER HE RECOVERS, IT'S BECAUSE OF SETACTIVE(TRUE/FALSE)

public class BossHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    public Slider HealthSlider;
    [SerializeField] Boss1 boss1;
    [SerializeField] int health = 10;

    bool dead = false;
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

    void Die()
    {
        dead = true;
        StartCoroutine(boss1.DeathSequence());
    }
}
