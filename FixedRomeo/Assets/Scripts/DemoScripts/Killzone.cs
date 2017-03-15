﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Killzone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") StartCoroutine(ReloadGame());
        Destroy(other.gameObject);
    }

    IEnumerator ReloadGame()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}