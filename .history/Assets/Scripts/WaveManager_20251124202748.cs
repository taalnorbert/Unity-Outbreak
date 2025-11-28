using UnityEngine;
using System.Collections; // Szükséges a Coroutine-okhoz

public class WaveManager : MonoBehaviour
{
    // Hivatkozás a zombi prefabra és a hullámokra
    public Transform[] spawnPoints; // A 3 Spawnpontunk
    public Wave[] waves; // A Hullámok tömbje
    
    // Privát változók
    private int currentWaveIndex = 0;
    private int totalEnemiesToSpawn = 0;
    private int enemiesRemaining = 0;

    void Start()
    {
        // Elindítjuk az első hullámot
        if (waves.Length > 0 && spawnPoints.Length > 0)
        {
            StartCoroutine(StartNextWave());
        }
    }

    // Coroutine a hullámok kezeléséhez
    IEnumerator StartNextWave()
    {
        // Várás, amíg az előző hullám zombijai elpusztulnak
        yield return new WaitUntil(() => enemiesRemaining <= 0);

        // Nincs több hullám
        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("JÁTÉK VÉGE! Minden hullám teljesítve.");
            yield break; // Leállítja a Coroutine-t
        }

        Wave currentWave = waves[currentWaveIndex];
        totalEnemiesToSpawn = currentWave.count;
        enemiesRemaining = currentWave.count;
        
        Debug.Log("--- HULLÁM INDUL: " + currentWave.waveName + " ---");
        
        // Zombik spawnolása
        yield return StartCoroutine(SpawnWave(currentWave));
        
        currentWaveIndex++;
        // Ez indítja el a következő hullámot, miután minden zombi meghalt
        StartCoroutine(StartNextWave());
    }

    // Coroutine a zombik tényleges időzített spawnolásához
    IEnumerator SpawnWave(Wave wave)
    {
        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemyPrefab);
            yield return new WaitForSeconds(1f / wave.rate); // Várás a következő spawn előtt
        }
    }

    // Zombi spawnolása
    void SpawnEnemy(GameObject enemyPrefab)
    {
        // Véletlenszerű spawnpont kiválasztása
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        
        // Zombi létrehozása
        GameObject newEnemy = Instantiate(enemyPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);
        
        // Csatlakozás a zombi halál eseményéhez (ezzel frissítjük az enemiesRemaining-et)
        // Megjegyzés: Ehhez módosítani kell a ZombieAI.cs-t a következő lépésben!
    }

    // Publikus metódus, amit a zombi hív meg, ha meghal.
    public void EnemyDied()
    {
        enemiesRemaining--;
        Debug.Log("Zombi maradt: " + enemiesRemaining);
    }

    // Publikus getter a jelenlegi hullám indexének lekérdezéséhez
    public int GetCurrentWaveNumber()
    {
        // A hullámok indexe 0-tól indul, de a felhasználónak 1-től akarjuk mutatni.
        return currentWaveIndex + 1;
    }

    // Publikus getter a maradék zombik számának lekérdezéséhez
    public int GetEnemiesRemaining()
    {
        return enemiesRemaining;
    }
}