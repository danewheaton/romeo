using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum GameStates { TITLE_MENU, GAMEPLAY }

public class GameStateManager : MonoBehaviour
{
    public static bool won; // COREY: when the boss is dead, set this to true

    static GameStateManager instance = null;
    public static GameStateManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameStateManager>();
            return instance;
        }
    }

    GameStates currentState = GameStates.TITLE_MENU;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        Cursor.visible = false;
    }

    private void Update()
    {
        switch (currentState)
        {
            case GameStates.TITLE_MENU:
                if (Input.GetButtonDown("Jump"))
                {
                    currentState = GameStates.GAMEPLAY;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                break;
            case GameStates.GAMEPLAY:
                if (won)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                break;
        }
    }
}
