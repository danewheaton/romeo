using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SigmaTriggers : MonoBehaviour
{
    #region events
    public delegate void EnterArena();
    public static event EnterArena OnEnterArena;
    public delegate void ReachCheckpoint(Transform checkpoint);
    public static event ReachCheckpoint OnReachedCheckpoint;
    #endregion
    [SerializeField]
    GameObject Player;
    [SerializeField] Transform mostRecentCheckpoint;
    [SerializeField]
    GameObject[] fallingPlatforms;

    Vector3[] platformOriginalPositions, platformOriginalEulers;

    void Start()
    {
        platformOriginalPositions = new Vector3[fallingPlatforms.Length];
        platformOriginalEulers = new Vector3[fallingPlatforms.Length];

        for (int i = 0; i < fallingPlatforms.Length; i++)
        {
            platformOriginalPositions[i] = fallingPlatforms[i].transform.position;
            platformOriginalEulers[i] = fallingPlatforms[i].transform.eulerAngles;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.tag)
        {
            case "Checkpoint":
                mostRecentCheckpoint = collider.transform;
                if (OnReachedCheckpoint != null) OnReachedCheckpoint(mostRecentCheckpoint);
                break;
            case "BossArena":
                if (OnEnterArena != null) OnEnterArena();
                break;
        }
    }


    public void PlayerDeath ()
    {
        Player.transform.position = mostRecentCheckpoint.position;
        for (int i = 0; i < fallingPlatforms.Length; i++)
        {
            fallingPlatforms[i].transform.position = platformOriginalPositions[i];
            fallingPlatforms[i].transform.eulerAngles = platformOriginalEulers[i];
            fallingPlatforms[i].GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }
   
}
