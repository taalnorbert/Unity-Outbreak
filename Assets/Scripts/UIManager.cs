using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI ammoText;

    public Image deathWarningImage;

    private PlayerHealth playerHealth;

    private Weapon weapon; 
    private WaveManager waveManager;


    public GameObject hpContainer;

    public void onGameOver()
    {
        hpContainer.SetActive(false);
        ammoText.gameObject.SetActive(false);
    }

    void Start()
    {
       
        playerHealth = FindFirstObjectByType<PlayerHealth>();
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
            float currentHP = playerHealth.GetCurrentHealth();
            hpText.text = Mathf.CeilToInt(currentHP).ToString();
            if(playerHealth.GetCurrentHealth() <= 50)
            {
                float remainingHP = 50f - currentHP;
                float normalizedOpacity = remainingHP / 50f * 0.25f;
                deathWarningImage.color = new Color(1f, 0f, 0f, normalizedOpacity);
            } else
            {
                deathWarningImage.color = new Color(1f, 0f, 0f, 0f);
            }
        }
        else
        {
             hpText.text = 0.ToString();
        }

        if (ammoText != null && weapon != null)
        {
            ammoText.text = $"{weapon.currentAmmo} / {weapon.totalAmmo}";
        }
    }
}