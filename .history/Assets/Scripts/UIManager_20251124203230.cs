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
    private Weapon weapon; // <<< ÚJ: Fegyver hivatkozás 

    void Start()
    {
        // Keresés
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        waveManager = FindFirstObjectByType<WaveManager>();
        weapon = FindFirstObjectByType<Weapon>(); // <<< ÚJ: Fegyver keresése
        
        // Előzetes teszt
        if (hpText == null || waveText == null)
        {
            Debug.LogError("UI Text hivatkozások nincsenek beállítva az UIManager-en!");
        }
    }

    void Update()
    {
        // HP Frissítése
        if (playerHealth != null)
        {
            // Egész számra kerekítjük és kiírjuk
            hpText.text = $"HP: {Mathf.CeilToInt(playerHealth.currentHealth)}";
        }

        // Hullám Frissítése
        if (waveManager != null)
        {
            int waveNum = waveManager.GetCurrentWaveNumber();
            int remaining = waveManager.GetEnemiesRemaining();
            
            waveText.text = $"Hullám: {waveNum} / Zombi Maradt: {remaining}";
            
            // Extra ellenőrzés a Játék Vége esetére
            if (waveManager.waves.Length > 0 && waveNum > waveManager.waves.Length && remaining <= 0)
            {
                waveText.text = "JÁTÉK VÉGE!";
            }
        }

        // Lőszer Frissítése
        if (ammoText != null && weapon != null)
        {
            // Kiírja az aktuális lőszert / teljes készletet
            ammoText.text = $"{weapon.currentAmmo} / {weapon.totalAmmo}";
        }
    }
}