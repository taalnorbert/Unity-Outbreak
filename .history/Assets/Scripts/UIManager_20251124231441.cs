// UIManager.cs

using UnityEngine;
using TMPro; // Ezt muszáj használni a TextMeshPro-hoz!

public class UIManager : MonoBehaviour
{
    // Publikus hivatkozások az UI elemekre (Inspectorban húzd rá!)
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI ammoText;

    // Hivatkozások a frissítendő scriptekre
    private PlayerHealth playerHealth;
    private WaveManager waveManager;
    private Weapon weapon; 

    void Start()
    {
        // Keresés
        // ⚠️ Figyelem: Ha a UIManager-t UIManager1-re nevezted át, akkor itt
        // a FindFirstObjectByType<UIManager>() sort FindFirstObjectByType<UIManager1>() kellene hívnia
        
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
        // HP Frissítése
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


        // Hullám Frissítése
        if (waveManager != null)
        {
            int waveNum = waveManager.GetCurrentWaveNumber();
            int remaining = waveManager.GetEnemiesRemaining();
            int totalWaves = waveManager.GetTotalWaves(); // ✅ GetTotalWaves metódus használata
            
            waveText.text = $"Hullám: {waveNum} / Zombi Maradt: {remaining}";
            
            // 🏆 JAVÍTVA: waves helyett GetTotalWaves() használata a győzelem ellenőrzésére
            if (waveNum >= totalWaves && remaining <= 0)
            {
                waveText.text = "JÁTÉK VÉGE! (NYERT)";
            }
        }

        // Lőszer Frissítése
        if (ammoText != null && weapon != null)
        {
            // Feltételezzük, hogy a currentAmmo és totalAmmo a Weapon.cs-ben publikus (vagy getter metódusokat használ)
            // Mivel a Weapon.cs-ben public int currentAmmo; public int totalAmmo; szerepel:
            ammoText.text = $"{weapon.currentAmmo} / {weapon.totalAmmo}";
        }
    }
}