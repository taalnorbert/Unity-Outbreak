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
        // Megkeressük a FirstPersonController scriptet is:
        fpsController = GetComponent<StarterAssets.FirstPersonController>();
        
        Debug.Log("Játékos HP: " + currentHealth);
    }

    // IDamageable.TakeDamage metódus
    // PlayerHealth.cs
public void TakeDamage(float damage)
{
    // Ha már halott, ne tegyen semmit
    if (currentHealth <= 0) return; 
    
    currentHealth -= damage;
    
    // HP korlátozása 0-ra
    if (currentHealth < 0)
    {
        currentHealth = 0;
    }
    
    Debug.Log("Játékos sebződött! Új HP: " + currentHealth);

    if (currentHealth <= 0)
    {
        Die();
    }
}
    void Die()
    {
        Debug.Log("GAME OVER! A játékos meghalt.");
        
        // Tiltjuk a FirstPersonController scriptet (ez a kulcs a hiba megoldásához!)
        if (fpsController != null)
        {
            fpsController.enabled = false; // <<< A HIBA JAVÍTÁSA
        }
        
        // Tiltjuk a mozgást (bár már felesleges lehet, maradhat)
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        // Késleltetett leállítás
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