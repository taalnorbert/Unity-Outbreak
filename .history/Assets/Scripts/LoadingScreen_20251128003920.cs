using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public Slider loadingBar;

    void Start()
    {
        // Start loading the scene you want
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        // Replace "GameScene" with your actual scene name
        AsyncOperation op = SceneManager.LoadSceneAsync("GameScene");

        op.allowSceneActivation = false;  // stops automatic switching

        float progress = 0f;

        while (!op.isDone)
        {
            // Unity loads 0–0.9, then waits
            float target = Mathf.Clamp01(op.progress / 0.9f);
            progress = Mathf.MoveTowards(progress, target, Time.deltaTime * 2f);

            loadingBar.value = progress;

            // When bar is full → activate scene
            if (progress >= 1f)
            {
                yield return new WaitForSeconds(0.4f); // optional dramatic pause
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
