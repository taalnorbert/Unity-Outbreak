using UnityEngine;
using System.Collections;
using System.Linq; 

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
    private UIManager1 uiManager1;
    private NarrativeUI narrativeUI;

    void Start()
    {
        int savedAct = PlayerPrefs.GetInt("ActProgress", 1); 
        
        uiManager1 = FindFirstObjectByType<UIManager1>();
        narrativeUI = FindFirstObjectByType<NarrativeUI>();
        
        if (savedAct >= 3)
        {
            PlayerPrefs.DeleteKey("ActProgress");
            PlayerPrefs.Save();
            savedAct = 1;
            Debug.Log("JÁTÉK VÉGE mentés törölve. Új játék indul.");
        }
        
        if (savedAct >= 2)
        {
            Debug.Log("MENTÉS BETÖLTVE: Act 2 (Boss Fight) indul.");
            StartCoroutine(StartBossFightFromSave());
        }
        else
        {
            Debug.Log("ÚJ JÁTÉK INDUL: Prológus és Act 1.");
            InitializeGameFlow();
        }
    }
    
    void InitializeGameFlow()
    {
        cageScript.OnRescueCompleted.AddListener(OnAct1RescueCompleted);
        
        StartCoroutine(StartPrologueAndGame());
    }

    IEnumerator StartPrologueAndGame()
    {
        if (narrativeUI != null)
        {
            narrativeUI.DisplayMessage("Ébredj! Zombi apokalipszis tört ki, és egy katona vagy. A túszt a gyárban tartják fogva!", 5f);
        }
        yield return new WaitForSecondsRealtime(5.5f);

        if (narrativeUI != null)
        {
            narrativeUI.DisplayMessage("Keresd meg a dobozt, amibe bezárták a túszt! Nyomd az 'E' gombot 5 másodpercig a kiszabadításhoz.", 6f);
        }
        
        if (waveManager != null)
        {
            waveManager.StartContinuousSpawn(continuousSpawnRate);
        }
    }
    
    IEnumerator StartBossFightFromSave()
    {
        yield return new WaitForSeconds(1f); 
        
        if (narrativeUI != null)
        {
            narrativeUI.DisplayMessage("Mentés betöltve. Folytasd a Boss harccal!", 4f);
        }
        yield return new WaitForSecondsRealtime(4.5f);
        
        if (!bossFightStarted && bossPrefab != null && bossSpawnPoint != null)
        {
            Gate gateScript = act1ExitGate.GetComponent<Gate>();
            if (gateScript != null) gateScript.Open();
            
            StartBossFight();
        }
    }
    
    public void OnAct1RescueCompleted()
    {
        if (act1Completed) return;
        act1Completed = true;

        if (waveManager != null)
        {
            waveManager.StopSpawning();
        }
        
        StartCoroutine(WaitForRemainingEnemies());
    }
    
    IEnumerator WaitForRemainingEnemies()
    {
        int remainingAct1Enemies = GameObject.FindGameObjectsWithTag(enemyTag).Length;
        Debug.Log($"Túsz szabad! Meg kell ölni a maradék {remainingAct1Enemies} zombit.");

        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag(enemyTag).Length == 0);

        Debug.Log("Minden Act 1 ellenség kiiktatva. Kapu nyílik az Act 2-be.");
        
        PlayerPrefs.SetInt("ActProgress", 2); 
        PlayerPrefs.Save(); 
        
        if (narrativeUI != null)
        {
            narrativeUI.DisplayMessage("A kijárat most nyitva. Keresd meg a főgonoszt és küzdj meg vele!", 5f);
        }
        yield return new WaitForSecondsRealtime(5.5f);

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
            
            if (narrativeUI != null)
            {
                narrativeUI.DisplayMessage("Főgonosz legyőzve. A túsz sikeresen kiszabadítva. Küldetés teljesítve!", 6f);
            }
            yield return new WaitForSecondsRealtime(6.5f);

            PlayerPrefs.SetInt("ActProgress", 3);
            PlayerPrefs.Save();

            Debug.Log("JÁTÉK NYERT! A Boss legyőzve!");
            
            Time.timeScale = 0f;
            
            if (uiManager1 != null)
            {
                uiManager1.ShowGameWin();
            }
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
     public void DeleteSaveData()
    {
        PlayerPrefs.DeleteKey("ActProgress");
        PlayerPrefs.Save();
        Debug.Log("MENTÉS TÖRÖLVE! A következő indításkor újra a Prológus indul.");
    }
}