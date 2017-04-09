using UnityEngine;
using System.Collections;

public class SigmaTriggers : MonoBehaviour
{
    public delegate void EnterArena();
    public static event EnterArena OnEnterArena;

    void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.tag)
        {
            case "BossArena":
                if (OnEnterArena != null) OnEnterArena();
                break;
        }
    }
}
