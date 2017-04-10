using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	[SerializeField] float spawnTime = 4, spawnDelayMin = 1, spawnDelayMax = 3;
    [SerializeField] GameObject[] enemies;
    [SerializeField] int EnemyCount = 0, MaxEnemies = 3;
    [SerializeField] Transform closestCheckpoint;

    bool closeEnoughToSpawn;

    private void OnEnable()
    {
        SigmaTriggers.OnReachedCheckpoint += GetLatestCheckpointReached;
    }
    private void OnDisable()
    {
        SigmaTriggers.OnReachedCheckpoint -= GetLatestCheckpointReached;
    }

    void Start ()
	{
		InvokeRepeating("Spawn", Random.Range(spawnDelayMin * 100, spawnDelayMax * 100) / 100, Random.Range(spawnDelayMin * 100, spawnDelayMax * 100) / 100);
	}

    void Spawn()
    {
        if (EnemyCount < MaxEnemies && closeEnoughToSpawn)
        {
            int enemyIndex = Random.Range(0, enemies.Length);
            int direction = Random.Range(0, 1);

            Instantiate(enemies[enemyIndex], transform.position,
                direction == 0 ? transform.rotation : new Quaternion
                (-transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w));

            EnemyCount++;
        }
    }

    void GetLatestCheckpointReached(Transform checkpoint)
    {
        if (checkpoint == closestCheckpoint) closeEnoughToSpawn = true;
        print("closeEnoughToSpawn should be true, and it is: " + closeEnoughToSpawn);
    }
}
