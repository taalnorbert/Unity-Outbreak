using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public Slider loadingBar;

    void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("GameScene");

        op.allowSceneActivation = false;

        float progress = 0f;

        while (!op.isDone)
        {
            float target = Mathf.Clamp01(op.progress / 0.9f);
            progress = Mathf.MoveTowards(progress, target, Time.deltaTime * 2f);

            loadingBar.value = progress;

            if (progress >= 1f)
            {
                yield return new WaitForSeconds(0.4f);
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
