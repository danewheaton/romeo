using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    void OnEnable()
    {
        Invoke("SelfDestruct", .2f);
    }

    void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
