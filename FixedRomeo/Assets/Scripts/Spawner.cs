using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	public float spawnTime = 5;
	public float spawnDelayMin = 1, spawnDelayMax = 3;
	public GameObject[] enemies;


	void Start ()
	{
		InvokeRepeating("Spawn", Random.Range(spawnDelayMin * 100, spawnDelayMax * 100) / 100, Random.Range(spawnDelayMin * 100, spawnDelayMax * 100) / 100);
	}


	void Spawn ()
	{
		int enemyIndex = Random.Range(0, enemies.Length);
        int direction = Random.Range(0, 1);
		Instantiate(enemies[enemyIndex], transform.position, direction == 0 ? transform.rotation : new Quaternion(-transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w));
	}
}
