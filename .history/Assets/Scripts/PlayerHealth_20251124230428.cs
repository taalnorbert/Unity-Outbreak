// PlayerHealth.cs

using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    // ... (Már meglévő változók)

    private UIManager uiManager; // Hivatkozás az UI Managerre 🔴 ÚJ VÁLTOZÓ

    void Start()
    {
        // ... (Már meglévő Start logika)

        // UI Manager keresése (csak egyszer fut le)
        uiManager = FindFirstObjectByType<UIManager>();
    }

    // ... (TakeDamage metódus marad)
    
    void Die()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log("Játékos meghalt! GAME OVER.");

        // ÚJ KÓD: Játék vége
        
        // 1. Állítsuk meg a játékot
        Time.timeScale = 0f;

        // 2. Aktiváljuk a Game Over képernyőt
        if (uiManager != null)
        {
            uiManager.ShowGameOver();
        }
        
        // 3. Jelenítsük meg az egeret
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}