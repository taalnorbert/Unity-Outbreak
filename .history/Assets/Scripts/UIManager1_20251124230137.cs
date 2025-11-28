// UIManager.cs

using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Game Over & Win UI")]
    public GameObject gameOverPanel; // Ezt kell behúzni az Inspectorban
    public GameObject gameWinPanel;  // Ezt kell behúzni az Inspectorban

    void Start()
    {
        // Alapértelmezésben elrejtjük a paneleket
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        if (gameWinPanel != null)
        {
            gameWinPanel.SetActive(false);
        }
        // Játékmenet sebességének biztosítása
        Time.timeScale = 1f; 
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