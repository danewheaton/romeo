using UnityEngine;
using System.Collections;

// all code by CK unless otherwise noted

public class Pitfall : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rb2d;

    void Start ()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.isKinematic = true;
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            rb2d.isKinematic = false;
        }
    }
}
