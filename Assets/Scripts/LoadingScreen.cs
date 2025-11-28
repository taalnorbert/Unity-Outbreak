using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Slider loadingBar;

    void Start()
    {
        loadingBar = this.GetComponent<Slider>();
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneLoader.nextScene);
        op.allowSceneActivation = false;

        float fakeProgress = 0f;

        float minLoadTime = 3.0f;
        float timer = 0f;

        while (!op.isDone)
        {
            timer += Time.deltaTime;

            float realProgress = Mathf.Clamp01(op.progress / 0.9f);

            fakeProgress = Mathf.MoveTowards(fakeProgress, realProgress, Time.deltaTime * 0.3f);

            if (fakeProgress < realProgress)
                fakeProgress = Mathf.MoveTowards(fakeProgress, realProgress, Time.deltaTime * 0.3f);

            fakeProgress = Mathf.Min(fakeProgress + Time.deltaTime * 0.1f, 1f);

            loadingBar.value = fakeProgress;

            if (fakeProgress >= 1f && realProgress >= 1f && timer >= minLoadTime)
            {
                yield return new WaitForSeconds(0.8f); 
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }

}
