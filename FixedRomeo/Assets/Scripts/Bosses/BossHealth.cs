using UnityEngine;
using System.Collections;

// IF THE BOSS'S HEALTH RESETS BACK TO 100% AFTER HE RECOVERS, IT'S BECAUSE OF SETACTIVE(TRUE/FALSE)

public class BossHealth : MonoBehaviour
{
    [SerializeField] Boss1 boss1;
    [SerializeField] int health = 10;

    bool dead = false;

    void Update()
    {
        if (health <= 0 && !dead) Die();
    }

    public void Hurt(int hurtAmount)
    {
        health -= hurtAmount;
    }

    void Die()
    {
        dead = true;
        StartCoroutine(boss1.DeathSequence());
    }
}
