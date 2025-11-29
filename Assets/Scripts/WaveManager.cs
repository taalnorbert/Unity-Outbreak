using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public int totalWaves = 5;
    public float timeBetweenWaves = 5f;
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    private int currentWave = 0;
    private int enemiesRemaining = 0;
    private int enemiesToSpawn = 0;
    private Coroutine currentSpawnCoroutine;

    private UIManager1 uiManager1;

    void Start()
    {
        uiManager1 = FindFirstObjectByType<UIManager1>();
    }

    public void StartContinuousSpawn(float rate)
    {
        if (enemyPrefab == null || spawnPoints.Length == 0) return;
        
        if (currentSpawnCoroutine != null) StopCoroutine(currentSpawnCoroutine);
        
        currentSpawnCoroutine = StartCoroutine(ContinuousSpawnLoop(rate));
    }

    public void StopSpawning()
    {
        if (currentSpawnCoroutine != null)
        {
            StopCoroutine(currentSpawnCoroutine);
            Debug.Log("Zombi spawnolás leállítva!");
        }
    }

    IEnumerator ContinuousSpawnLoop(float rate)
    {
        while (true)
        {
            SpawnEnemy();
            enemiesRemaining++;
            yield return new WaitForSeconds(1f / rate);
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void EnemyDied()
    {
        enemiesRemaining--;
        Debug.Log("Ellenség maradt: " + enemiesRemaining);
    }

    public int GetCurrentWaveNumber()
    {
        return currentWave;
    }

    public int GetEnemiesRemaining()
    {
        return enemiesRemaining;
    }

    public int GetTotalWaves()
    {
        return totalWaves;
    }
}