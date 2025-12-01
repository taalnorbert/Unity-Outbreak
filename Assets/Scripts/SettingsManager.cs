using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Volume Sliders")]
    public Slider[] volumeSliders; 
    
    [Header("Value Display Texts")]
    public TMP_Text[] valueDisplayTexts; 

    public GameObject mainMenuMusicObject;

    void Start()
    {
        volumeSliders[0].value = Settings.gameMusic;
        
        volumeSliders[1].value = Settings.gameVolume;

        volumeSliders[2].value = Settings.mainMenuMusic;

        for (int i = 0; i < volumeSliders.Length; i++)
        {
            UpdateText(i);
        }
    }

    public void OnSliderValueChanged(int index)
    {
        float newValue = volumeSliders[index].value;

        switch (index)
        {
            case 0:
                Settings.gameMusic = newValue;
                break;
            case 1:
                Settings.gameVolume = newValue;
                break;
            case 2:
                Settings.mainMenuMusic = newValue;
                if (mainMenuMusicObject != null)
                {
                    AudioSource audioSource = mainMenuMusicObject.GetComponent<AudioSource>();
                    if (audioSource != null)
                    {
                        audioSource.volume = newValue / 100f; 
                    }
                }
                break;
            default:
                break;
        }
        
        UpdateText(index);
    }
    
    private void UpdateText(int index)
    {
        if (index >= 0 && index < volumeSliders.Length && index < valueDisplayTexts.Length)
        {
            float value = volumeSliders[index].value;
            
            valueDisplayTexts[index].text = (value).ToString("F0") + "%"; 
        }
    }
}