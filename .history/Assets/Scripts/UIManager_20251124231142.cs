// UIManager1.cs

using UnityEngine;
using TMPro; // 🚨 Ha TextMeshPro-t használsz

public class UIManager1 : MonoBehaviour
{
    [Header("UI References")]
    public PlayerHealth playerHealth;  // Húzd be az Inspectorban!
    public WaveManager waveManager;    // Húzd be az Inspectorban!
    public TextMeshProUGUI healthText; // 🚨 A Text/TextMeshPro UI elem!
    public TextMeshProUGUI waveText;   // 🚨 A Text/TextMeshPro UI elem!

    [Header("Game Over & Win UI")]
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;

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
        UpdateHealthUI();
        UpdateWaveUI();
    }

    private void UpdateHealthUI()
    {
        if (playerHealth != null && healthText != null)
        {
            // ✅ GETTER METÓDUSOK HASZNÁLATA
            float currentHealth = playerHealth.GetCurrentHealth();
            float maxHealth = playerHealth.GetMaxHealth();
            
            healthText.text = $"Élet: {Mathf.CeilToInt(currentHealth)} / {Mathf.CeilToInt(maxHealth)}";
        }
    }

    private void UpdateWaveUI()
    {
        if (waveManager != null && waveText != null)
        {
            // ✅ GETTER METÓDUSOK HASZNÁLATA (megszüntetve a 'waves' hibát)
            int currentWave = waveManager.GetCurrentWaveNumber();
            int enemiesRemaining = waveManager.GetEnemiesRemaining();
            int totalWaves = waveManager.GetTotalWaves();
            
            waveText.text = $"Hullám: {currentWave} / {totalWaves}\nEllenség: {enemiesRemaining}";
        }
    }

    // PlayerHealth.cs hívja meg
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    // WaveManager.cs hívja meg
    public void ShowGameWin()
    {
        if (gameWinPanel != null)
        {
            gameWinPanel.SetActive(true);
        }
    }
}