using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif 
        Application.Quit();
    }
}
