using UnityEngine;
using TMPro; 

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI ammoText;

    private PlayerHealth playerHealth;
    private WaveManager waveManager;
    private Weapon weapon; 

    void Start()
    {
       
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        waveManager = FindFirstObjectByType<WaveManager>();
        weapon = FindFirstObjectByType<Weapon>(); 
        
        if (hpText == null || waveText == null || ammoText == null)
        {
            Debug.LogError("UI Text hivatkozások nincsenek beállítva az UIManager-en!");
        }
    }

    void Update()
    {
        if (playerHealth != null)
        {
            // 🏆 JAVÍTVA: currentHealth helyett GetCurrentHealth()
            hpText.text = $"HP: {Mathf.CeilToInt(playerHealth.GetCurrentHealth())}";
        }
        else
        {
             // Ha meghalt a Player és eltűnik, ne próbáljunk hívni
             hpText.text = "HP: 0";
        }


        if (waveManager != null)
        {
            int waveNum = waveManager.GetCurrentWaveNumber();
            int remaining = waveManager.GetEnemiesRemaining();
            int totalWaves = waveManager.GetTotalWaves(); // ✅ GetTotalWaves metódus használata
            
            waveText.text = $"Hullám: {waveNum} / Zombi Maradt: {remaining}";
            
            if (waveNum >= totalWaves && remaining <= 0)
            {
                waveText.text = "JÁTÉK VÉGE! (NYERT)";
            }
        }

        if (ammoText != null && weapon != null)
        {
            ammoText.text = $"{weapon.currentAmmo} / {weapon.totalAmmo}";
        }
    }
}