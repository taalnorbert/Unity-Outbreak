using StarterAssets;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    public float startingHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

    private UIManager1 uiManager;


    [Header("Regeneration Settings")]
    public float regenDelay = 10f; 
    public float regenRate = 0.2f; 
    public float regenAmount = 1f; 
    private float timeSinceLastDamage = 0f; 

    private Coroutine regenerateCoroutine; 

    void Start()
    {
        currentHealth = startingHealth;
        isDead = false;

        uiManager = FindFirstObjectByType<UIManager1>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if(!isDead && currentHealth < startingHealth)
        {
            timeSinceLastDamage += Time.deltaTime;

            if (timeSinceLastDamage >= regenDelay && regenerateCoroutine == null)
            {
                regenerateCoroutine = StartCoroutine(Regenerate());
            }
            else if (currentHealth >= startingHealth && regenerateCoroutine != null)
            {
                StopCoroutine(regenerateCoroutine);
                regenerateCoroutine = null;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"Játékos sebződött: {currentHealth}");

        timeSinceLastDamage = 0f;

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
            GameObject hud = GameObject.FindGameObjectWithTag("HUD");
            UIManager uimanager = hud.GetComponent<UIManager>();
            uimanager.onGameOver();
            uiManager.ShowGameOver();
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        StarterAssetsInputs _input = player.GetComponent<StarterAssetsInputs>();
        if (_input != null) { 
            _input.cursorLocked = true;
            _input.cursorInputForLook = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return startingHealth;
    }

    IEnumerator Regenerate()
    {
        WaitForSeconds waitTime = new WaitForSeconds(regenRate);

        while (currentHealth < startingHealth && !isDead)
        {
            yield return waitTime;

            if (timeSinceLastDamage >= regenDelay)
            {
                currentHealth += regenAmount;

                if (currentHealth > startingHealth)
                {
                    currentHealth = startingHealth;
                }
            }
        }
        regenerateCoroutine = null;
    }
}