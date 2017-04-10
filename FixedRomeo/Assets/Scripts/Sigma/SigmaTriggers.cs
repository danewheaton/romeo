using UnityEngine;
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
    }
   
}
