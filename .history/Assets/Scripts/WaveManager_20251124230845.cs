// WaveManager.cs

using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    // 💡 Figyelem: A már meglévő hullámkezelő változóid itt vannak (pl. totalWaves, timeBetweenWaves, currentWave, enemiesRemaining, enemyPrefab, spawnPoints)

    // Hullámkezelő változók (feltételezzük, hogy ezek már léteznek)
    public int totalWaves = 5;
    public float timeBetweenWaves = 5f;
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    private int currentWave = 0;
    private int enemiesRemaining = 0;
    private int enemiesToSpawn = 0;

    // Hivatkozás az UI Managerre
    private UIManager1 uiManager1; 

    void Start()
    {
        // UI Manager keresése
        uiManager1 = FindFirstObjectByType<UIManager1>(); 
        StartNextWave();
    }

    // Hullám indítása
    void StartNextWave()
    {
        currentWave++;
        enemiesToSpawn = currentWave * 2; // Példa: Minden hullámban +2 ellenség
        enemiesRemaining = enemiesToSpawn;
        
        Debug.Log("Hullám indítása: " + currentWave);

        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f); // Kisebb késleltetés az ellenségek között
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    // Ellenség halála (Ezt az EnemyHealth.cs hívja)
    public void EnemyDied()
    {
        enemiesRemaining--;
        Debug.Log("Ellenség maradt: " + enemiesRemaining);

        // Győzelem/hullám vége ellenőrzése
        if (enemiesRemaining <= 0)
        {
            // Ha van még hullám, akkor folytatjuk.
            if (currentWave < totalWaves)
            {
                Invoke("StartNextWave", timeBetweenWaves); // Várakozás után új hullám
            }
            else
            {
                // Nincs több hullám: JÁTÉK NYERT!
                Debug.Log("Minden hullám befejeződött. JÁTÉK NYERT!");
                
                // Játék megállítása és győzelem képernyő
                Time.timeScale = 0f;
                
                // 🏆 JAVÍTVA: uiManager1 használata
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