// WaveManager.cs

using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    // ... (Már meglévő változók)

    private UIManager1 uiManager1; // Hivatkozás az UI Managerre 🔴 ÚJ VÁLTOZÓ

    void Start()
    {
        // ... (Már meglévő Start logika)
        uiManager = FindFirstObjectByType<UIManager>(); // UI Manager keresése
        StartNextWave();
    }

    // ... (SpawnEnemy metódus marad)

    public void EnemyDied()
    {
        enemiesRemaining--;
        Debug.Log("Ellenség maradt: " + enemiesRemaining);

        // Győzelem/hullám vége ellenőrzése
        if (enemiesRemaining <= 0)
        {
            // Ha van még hullám, akkor folytatjuk.
            if (currentWave < totalWaves)
            {
                Invoke("StartNextWave", timeBetweenWaves); // Várakozás után új hullám
            }
            else
            {
                // Nincs több hullám: JÁTÉK NYERT!
                Debug.Log("Minden hullám befejeződött. JÁTÉK NYERT!");
                
                // ÚJ KÓD: Játék megállítása és győzelem képernyő
                Time.timeScale = 0f;
                if (uiManager != null)
                {
                    uiManager.ShowGameWin();
                }
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}