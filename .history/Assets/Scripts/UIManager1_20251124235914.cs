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

    [Header("Game Over & Win UI")]
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;

    // 🏆 Csak egy Start() metódus lehet!
    void Start()
    {
        // Alapértelmezésben elrejtjük a paneleket
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (gameWinPanel != null) gameWinPanel.SetActive(false);
        
        // Játékmenet sebességének biztosítása
        Time.timeScale = 1f; 
    }

    void Update()
    {
        // Csak akkor frissítsük az UI-t, ha a játék fut (Time.timeScale > 0)
        if (Time.timeScale > 0f)
        {
            UpdateHealthUI();
            UpdateWaveUI();
        }
    }

    private void UpdateHealthUI()
    {
        if (playerHealth != null && healthText != null)
        {
            float currentHealth = playerHealth.GetCurrentHealth();
            float maxHealth = playerHealth.GetMaxHealth();
            
            // Lekerekítés az egész szám megjelenítéséhez
            healthText.text = $"Élet: {Mathf.CeilToInt(currentHealth)} / {Mathf.CeilToInt(maxHealth)}";
        }
    }

    private void UpdateWaveUI()
    {
        if (waveManager != null && waveText != null)
        {
            int currentWave = waveManager.GetCurrentWaveNumber();
            int enemiesRemaining = waveManager.GetEnemiesRemaining();
            int totalWaves = waveManager.GetTotalWaves();
            
            waveText.text = $"Hullám: {currentWave} / {totalWaves}\nEllenség: {enemiesRemaining}";
        }
    }


    // 🏆 Csak egy ShowGameOver() metódus lehet!
    // PlayerHealth.cs hívja meg
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    // 🏆 Csak egy ShowGameWin() metódus lehet!
    // WaveManager.cs hívja meg
    public void ShowGameWin()
    {
        if (gameWinPanel != null)
        {
            gameWinPanel.SetActive(true);
        }
    }

    // --- MENÜ GOMB FUNKCIÓK (Még nincs beállítva, de itt a helye!) ---
    
    public void RestartGame()
    {
        // 1. Állítsuk vissza a játékidőt normálra
        Time.timeScale = 1f;
        // 2. Töltsük újra az aktuális Scene-t (Például "MainScene" vagy a jelenlegi Scene neve)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void QuitGame()
    {
        Debug.Log("Játék bezárása...");
        // 1. Mentési logika (opcionális)
        
        // 2. Alkalmazás bezárása
        Application.Quit();

        // Editorban nem zárja be, de kilépéskor igen
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}