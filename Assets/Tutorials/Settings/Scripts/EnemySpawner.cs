using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // 0: Fast, 1: Heavy, 2: Progressive
    public float minSpawnInterval = 1.0f;
    public float maxSpawnInterval = 2.5f;
    public float spawnYRange = 3.8f; // Vertical bounds inside camera view

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (GameManager.Instance != null && GameManager.Instance.IsGameRunning())
            {
                SpawnRandomEnemy();
                float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
                yield return new WaitForSeconds(waitTime);
            }
            else
            {
                yield return null;
            }
        }
    }

    void SpawnRandomEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        float randomY = Random.Range(-spawnYRange, spawnYRange);
        Vector3 spawnPos = new Vector3(transform.position.x, randomY, 0f);

        Instantiate(enemyPrefabs[index], spawnPos, Quaternion.identity);
    }
}