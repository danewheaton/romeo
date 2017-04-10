using UnityEngine;
using System.Collections;

public class StalactiteSpawner : MonoBehaviour
{
    [SerializeField] Object stalactitePrefab;

    GameObject stalactiteInstance;
    Vector3 originalStalactitePosition;

    private void OnEnable()
    {
        //Boss1.OnBoss1Stomp += DropStalactite;
    }
    private void OnDisable()
    {
        //Boss1.OnBoss1Stomp -= DropStalactite;
    }

    private void Start()
    {
        stalactiteInstance = (GameObject)Instantiate(stalactitePrefab, transform.position, transform.rotation);
        originalStalactitePosition = stalactiteInstance.transform.position;
    }

    void DropStalactite()
    {
        stalactiteInstance.GetComponent<Rigidbody2D>().isKinematic = false;
        Invoke("DestroyAndRespawnStalactite", 2);
    }

    void DestroyAndRespawnStalactite()
    {
        Destroy(stalactiteInstance);
        stalactiteInstance = (GameObject)Instantiate(stalactitePrefab, transform.position, transform.rotation);
    }
}
