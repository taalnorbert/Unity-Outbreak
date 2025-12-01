using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement;
using StarterAssets; 

public class UIManager1 : MonoBehaviour
{
    [Header("UI References")]
    public PlayerHealth playerHealth;  
    public WaveManager waveManager;    
    public TextMeshProUGUI healthText; 
    public TextMeshProUGUI waveText;   
    public TextMeshProUGUI ammoText;

    [Header("Game Over & Win UI")]
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;

    private FirstPersonController fpsController; 
    private CharacterController characterController;

    void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (gameWinPanel != null) gameWinPanel.SetActive(false);
        Time.timeScale = 1f; 
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            fpsController = player.GetComponent<FirstPersonController>();
            characterController = player.GetComponent<CharacterController>();
            playerHealth = player.GetComponent<PlayerHealth>();
        }
        
        waveManager = FindFirstObjectByType<WaveManager>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Time.timeScale > 0f)
        {
            UpdateHealthUI();
            UpdateWaveUI();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
            
            GameObject boss = GameObject.FindGameObjectWithTag("Boss");
            
            if (boss != null)
            {
                waveText.text = "BOSS FIGHT KÉSZ!"; 
            }
            else
            {
                waveText.text = $"Hullám: {currentWave}\nEllenség Maradt: {enemiesRemaining}";
            }
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            if (fpsController != null) fpsController.enabled = false;
            if (characterController != null) characterController.enabled = false;
            Time.timeScale = 0f; 
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            gameOverPanel.SetActive(true);
        }
    }

    public void ShowGameWin()
    {
        if (gameWinPanel != null)
        {
            if (fpsController != null) fpsController.enabled = false;
            if (characterController != null) characterController.enabled = false;

            Time.timeScale = 0f; 
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            gameWinPanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}