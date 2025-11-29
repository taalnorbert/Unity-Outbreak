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
    public string enemyTag = "Enemy"; // Fontos: a Zombin a Tag-nek 'Enemy' kell lennie!

    private bool act1Completed = false;
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
    
    // Várja, amíg a maradék zombi meghal
    IEnumerator WaitForRemainingEnemies()
    {
        // Megszámoljuk a maradék zombikat a Tag alapján
        int remainingEnemies = GameObject.FindGameObjectsWithTag(enemyTag).Length;
        Debug.Log($"Túsz szabad! Meg kell ölni a maradék {remainingEnemies} zombit.");

        // Várjuk, amíg nincsenek 'Enemy' Tag-gel rendelkező objektumok a jelenetben
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag(enemyTag).Length == 0);

        Debug.Log("Minden ellenség kiiktatva. Act 1 VÉGE.");
        
        // 3. Kinyitjuk a kaput
        Gate gateScript = act1ExitGate.GetComponent<Gate>();
        if (gateScript != null)
        {
            gateScript.Open();
        }
    }
}