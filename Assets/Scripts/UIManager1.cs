using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement;

public class UIManager1 : MonoBehaviour
{
    [Header("UI References")]
    public PlayerHealth playerHealth;  
    public WaveManager waveManager;    
    public TextMeshProUGUI healthText; 
    public TextMeshProUGUI waveText;   
    public TextMeshProUGUI ammoText; // Feltételezve, hogy ez is kell a UI-ban

    [Header("Game Over & Win UI")]
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;

    void Start()
    {
        // UI Panelek elrejtése
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (gameWinPanel != null) gameWinPanel.SetActive(false);
        Time.timeScale = 1f; 
        
        // Elrejti az egeret
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Csak akkor frissítsük az UI-t, ha a játék fut
        if (Time.timeScale > 0f)
        {
            UpdateHealthUI();
            UpdateWaveUI();
        }
        
        // ESC gomb a kurzor megjelenítéséhez (opcionális menü)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle kurzor lock/unlock (szüneteltetés nélkül)
            if (Time.timeScale == 1f)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    private void UpdateHealthUI()
    {
        if (playerHealth != null && healthText != null)
        {
            float currentHealth = playerHealth.GetCurrentHealth();
            float maxHealth = playerHealth.GetMaxHealth();
            healthText.text = $"Élet: {Mathf.CeilToInt(currentHealth)} / {Mathf.CeilToInt(maxHealth)}";
        }
    }

    private void UpdateWaveUI()
    {
        if (waveManager != null && waveText != null)
        {
            int currentWave = waveManager.GetCurrentWaveNumber();
            int enemiesRemaining = waveManager.GetEnemiesRemaining();
            
            // 🚨 BOSS VIZSGÁLAT: Ha a BossZombi létezik, írjuk ki az életét
            GameObject boss = GameObject.FindGameObjectWithTag("Boss");
            
            if (boss != null)
            {
                // Feltéve, hogy a Boss HP sávja látható
                waveText.text = "BOSS FIGHT KÉSZ!"; 
            }
            else
            {
                // Normál hullám információ
                waveText.text = $"Hullám: {currentWave}\nEllenség Maradt: {enemiesRemaining}";
            }
        }
    }


    // Ezt a PlayerHealth.cs hívja
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            // Megjelenítjük az egeret (ezt a PlayerHealth.Die() is csinálja, de itt is biztonságos)
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            gameOverPanel.SetActive(true);
        }
    }

    // Ezt a GameFlowManager.cs hívja
    public void ShowGameWin()
    {
        if (gameWinPanel != null)
        {
            // Megállítja a játékot (bár a GameFlowManager is megteszi)
            Time.timeScale = 0f; 
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            gameWinPanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        // 1. Állítsuk vissza a játékidőt
        Time.timeScale = 1f;
        // 2. Töltsük újra az aktuális Scene-t
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void QuitGame()
    {
        Debug.Log("Játék bezárása...");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}