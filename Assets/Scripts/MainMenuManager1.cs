using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject zombieObject;

    private void Start()
    {
        StartCoroutine(PreloadLoadingScene());
    }


    private IEnumerator PreloadLoadingScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);

        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            if (op.progress >= 0.9f)
            {
                Debug.Log("Loading Scene preloaded");
                break;
            }
            yield return null;
        }
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif 
        Application.Quit();
    }

    public void StartGame()
    {
        SceneLoader.LoadScene("Main");
    }

    public void Settings(bool open)
    {
        settingsPanel.SetActive(open);   
        zombieObject.SetActive(!open);
    }
}
