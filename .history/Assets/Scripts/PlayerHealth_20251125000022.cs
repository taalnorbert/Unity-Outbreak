using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    public float startingHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

    private UIManager1 uiManager; 

    void Start()
    {
        currentHealth = startingHealth;
        isDead = false;

        uiManager = FindFirstObjectByType<UIManager1>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

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

        Time.timeScale = 0f;

        if (uiManager != null)
        {
            uiManager.ShowGameOver();
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return startingHealth;
    }
}