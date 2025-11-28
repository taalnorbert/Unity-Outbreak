using UnityEngine;

// A játékosnak is meg kell valósítania az IDamageable interfészt
public class PlayerHealth : MonoBehaviour, IDamageable
{
    // Publikus változók
    public float maxHealth = 100f;
    public float currentHealth;
    public float deathScreenDelay = 2f; // Késleltetés halál után

    // Privát hivatkozások
    private CharacterController characterController;
    private StarterAssets.FirstPersonController fpsController; // <<< FirstPersonController hivatkozás

    void Start()
    {
        currentHealth = maxHealth;
        characterController = GetComponent<CharacterController>();
        
        Debug.Log("Játékos HP: " + currentHealth);
    }

    // IDamageable.TakeDamage metódus
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Játékos sebződött! Új HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("GAME OVER! A játékos meghalt.");
        
        // Tiltjuk a mozgást
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        // Késleltetett újraindítás, vagy Game Over UI
        // Jelenleg csak leállítjuk a játékot
        // Ide később tehetjük a UI megjelenítését:
        Invoke("EndGame", deathScreenDelay);
    }
    
    void EndGame()
    {
        // Ez a sor leállítja a játékot a Unity Editorban
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}