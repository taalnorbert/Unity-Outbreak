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
    
    // Ide kell a Weapon script is, de azt még nem írtuk meg
    // private Weapon weapon; 

    void Start()
    {
        // Keresés
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        waveManager = FindFirstObjectByType<WaveManager>();
        
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
            // Itt kellene a WaveManager-nek publikusan elérhetővé tenni a hullám/maradt zombi számot.
            // Mivel még nem tettük, most csak egy placeholder van:
            // KÉSŐBB IDE JÖN A VALÓDI FRISSÍTÉS
            waveText.text = "Hullám: [Később] / Zombi Maradt: [Később]";
        }

        // Lőszer Frissítése (KÉSŐBB)
        // ammoText.text = $"Lőszer: {weapon.currentAmmo} / {weapon.totalAmmo}";
    }
}