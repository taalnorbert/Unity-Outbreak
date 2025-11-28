using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    public float startingHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

    // Hivatkozás az átnevezett UIManager1 osztályra
    private UIManager1 uiManager; 

    void Start()
    {
        currentHealth = startingHealth;
        isDead = false;

        // UI Manager keresése
        uiManager = FindFirstObjectByType<UIManager1>();
        
        // Elrejti az egeret a játék indításakor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Az IDamageable.TakeDamage(float) metódus implementálása
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"Játékos sebződött: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log("Játékos meghalt! GAME OVER.");

        // Játék vége logika
        
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

    // Hozzáférés az aktuális életerőhöz (a ZombieAI-nak és az UI-nak)
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    // Hozzáférés a maximális életerőhöz (az UI-nak)
    public float GetMaxHealth()
    {
        return startingHealth;
    }
}