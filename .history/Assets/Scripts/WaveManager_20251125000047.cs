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

    private UIManager1 uiManager1; 

    void Start()
    {
        uiManager1 = FindFirstObjectByType<UIManager1>(); 
        StartNextWave();
    }

    void StartNextWave()
    {
        currentWave++;
        enemiesToSpawn = currentWave * 2; 
        enemiesRemaining = enemiesToSpawn;
        
        Debug.Log("Hullám indítása: " + currentWave);

        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f); 
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

        if (enemiesRemaining <= 0)
        {
            if (currentWave < totalWaves)
            {
                Invoke("StartNextWave", timeBetweenWaves);
            }
            else
            {
                Debug.Log("Minden hullám befejeződött. JÁTÉK NYERT!");
                
                Time.timeScale = 0f;
                
                if (uiManager1 != null) 
                {
                    uiManager1.ShowGameWin(); 
                }
                
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    // Hozzáférés az aktuális hullámszámhoz
    public int GetCurrentWaveNumber()
    {
        return currentWave;
    }

    // Hozzáférés a hátralévő ellenségek számához
    public int GetEnemiesRemaining()
    {
        return enemiesRemaining;
    }

    // Hozzáférés a teljes hullámszámhoz
    public int GetTotalWaves()
    {
        return totalWaves;
    }
}