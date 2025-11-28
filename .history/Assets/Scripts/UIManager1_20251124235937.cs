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

    void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (gameWinPanel != null) gameWinPanel.SetActive(false);
        Time.timeScale = 1f; 
    }

    void Update()
    {
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


    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

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