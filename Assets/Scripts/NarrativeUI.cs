using UnityEngine;
using TMPro; 
using System.Collections; 

public class NarrativeUI : MonoBehaviour
{
    private TextMeshProUGUI subtitleText;
    private GameObject textObject;

    void Awake()
    {
        subtitleText = GetComponent<TextMeshProUGUI>();
        textObject = gameObject;
        
    }

    public void DisplayMessage(string message, float duration)
    {
        if (subtitleText == null) return;
        
        subtitleText.text = message;
        textObject.SetActive(true);
        
        StartCoroutine(HideAfterDelay(duration));
    }

    IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); 
        
        textObject.SetActive(false);
        subtitleText.text = ""; 
    }
}