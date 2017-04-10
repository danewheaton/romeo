using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Killzone : MonoBehaviour
{
    SigmaTriggers ST;
    UIManager UI;

    void Start ()
    {
        UI = GetComponent<UIManager>();
        ST = GetComponent<SigmaTriggers>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            UI.PlayersDeath();
        }
    }

    IEnumerator ReloadGame()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
