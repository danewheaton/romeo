using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// all code by DW unless otherwise noted

public class PlayerCheckpoints : MonoBehaviour
{
    #region events
    public delegate void EnterArena();
    public static event EnterArena OnEnterArena;
    public delegate void ReachCheckpoint(Transform checkpoint);
    public static event ReachCheckpoint OnReachedCheckpoint;
    #endregion
    
    [SerializeField]
    GameObject[] fallingPlatforms;

    Vector3[] platformOriginalPositions, platformOriginalEulers;

    private void OnLevelWasLoaded(int level)
    {
        if (CheckpointManager.latestCheckpoint != null)
            transform.position = CheckpointManager.latestCheckpoint.position;
    }

    private void OnEnable()
    {
        PlayerHealth.OnDeath += RestartFromCheckpoint;
    }
    private void OnDisable()
    {
        PlayerHealth.OnDeath -= RestartFromCheckpoint;
    }

    void Start()
    {
        if (fallingPlatforms != null)
        {
            platformOriginalPositions = new Vector3[fallingPlatforms.Length];
            platformOriginalEulers = new Vector3[fallingPlatforms.Length];

            for (int i = 0; i < fallingPlatforms.Length; i++)
            {
                platformOriginalPositions[i] = fallingPlatforms[i].transform.position;
                platformOriginalEulers[i] = fallingPlatforms[i].transform.eulerAngles;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.tag)
        {
            case "Checkpoint":
                CheckpointManager.latestCheckpoint = collider.transform;
                if (OnReachedCheckpoint != null) OnReachedCheckpoint(CheckpointManager.latestCheckpoint);
                break;
            case "BossArena":
                if (OnEnterArena != null) OnEnterArena();
                break;
        }
    }

    public void RestartFromCheckpoint()
    {
        transform.position = CheckpointManager.latestCheckpoint.position;

        if (fallingPlatforms != null)
        {
            for (int i = 0; i < fallingPlatforms.Length; i++)
            {
                fallingPlatforms[i].transform.position = platformOriginalPositions[i];
                fallingPlatforms[i].transform.eulerAngles = platformOriginalEulers[i];
                fallingPlatforms[i].GetComponent<Rigidbody2D>().isKinematic = true;
            }
        }
    }
}
