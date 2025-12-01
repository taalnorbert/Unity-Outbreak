using UnityEngine;
using System.Collections;
using System.Linq; // Szükséges a GameObject.FindGameObjectsWithTag-hez

public class GameFlowManager : MonoBehaviour
{
    [Header("Összekötések")]
    public HostageCage cageScript;
    public WaveManager waveManager;
    public GameObject act1ExitGate; 
    
    [Header("Act 1 Spawn Settings")]
    public float continuousSpawnRate = 1.5f; 
    public string enemyTag = "Enemy";

    [Header("Act 2: Boss Fight")]
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public string bossTag = "Boss";

    private bool act1Completed = false;
    private bool bossFightStarted = false;
    private bool gameWon = false;
    private Coroutine spawnCoroutine;

    void Start()
    {
        // Act 1 Indítása: Indítjuk a folyamatos spawnolást
        if (waveManager != null)
        {
            // FIGYELEM: A WaveManager.cs-ben lévő StartContinuousSpawn metódust publikussá kell tennünk!
            waveManager.StartContinuousSpawn(continuousSpawnRate);
        }
        
        // Feliratkozunk a ketrec eseményére
        cageScript.OnRescueCompleted.AddListener(OnAct1RescueCompleted);
    }
    
    public void OnAct1RescueCompleted()
    {
        if (act1Completed) return;
        act1Completed = true;

        // 1. Leállítjuk a spawnolást
        waveManager.StopSpawning();
        
        // 2. Várjuk, amíg a maradék ellenség meghal
        StartCoroutine(WaitForRemainingEnemies());
    }
    
    IEnumerator WaitForRemainingEnemies()
    {
        int remainingAct1Enemies = GameObject.FindGameObjectsWithTag(enemyTag).Length;
        Debug.Log($"Túsz szabad! Meg kell ölni a maradék {remainingAct1Enemies} zombit.");

        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag(enemyTag).Length == 0);

        Debug.Log("Minden Act 1 ellenség kiiktatva. Kapu nyílik az Act 2-be.");
        
        Gate gateScript = act1ExitGate.GetComponent<Gate>();
        if (gateScript != null)
        {
            gateScript.Open();
        }

        if (!bossFightStarted && bossPrefab != null && bossSpawnPoint != null)
        {
            StartBossFight();
        }
    }

    void StartBossFight()
    {
        bossFightStarted = true;
        Debug.Log("Act 2: BOSS FIGHT INDUL!");

        GameObject boss = Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
        boss.tag = bossTag;

        StartCoroutine(WaitForBossDefeat());
    }

    IEnumerator WaitForBossDefeat()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag(bossTag) == null);

        if (!gameWon)
        {
            gameWon = true;
            Debug.Log("JÁTÉK NYERT! A Boss legyőzve!");
            Time.timeScale = 0f;
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}