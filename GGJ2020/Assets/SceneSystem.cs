using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneSystem : MonoBehaviour
{
    public static SceneSystem instance;
    private static bool loadingScene = false;

    private void Awake()
    {
        if(instance == null)
            instance = this;

        if (instance != this)
            Destroy(this.gameObject);
    }

    private IEnumerator Start()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(4, LoadSceneMode.Additive);
        while (!op.isDone) yield return null;

        op = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        while (!op.isDone) yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
    }

    public void FadeToNextScene(int scene)
    {
        FadeToNextScene(scene, Color.black);
    }

    public void FadeToNextScene(int scene, Color color)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        op.allowSceneActivation = false;
        FadeToNextScene(SceneManager.GetSceneByBuildIndex(scene), color, op);
    }

    public void FadeToNextScene(string scene)
    {
        FadeToNextScene(scene, Color.black);
    }

    public void FadeToNextScene(string scene, Color color)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        op.allowSceneActivation = false;
        FadeToNextScene(SceneManager.GetSceneByName(scene), color, op);
    }

    public void FadeToNextScene(Scene scene, Color color, AsyncOperation op)
    {
        if (loadingScene)
            return;

        loadingScene = true;
        FadeSystem.instance.ChangeColor(color);
        Sequence seq = DOTween.Sequence();
        seq.Append(FadeSystem.instance.Willfade(true));
        seq.AppendCallback(() =>
        {
            Scene current = SceneManager.GetActiveScene();
            op.allowSceneActivation = true;
            Debug.Log(current);
            AsyncOperation ap = SceneManager.UnloadSceneAsync(current);
        });
        seq.AppendInterval(.2f);
        seq.AppendCallback(() => SceneManager.SetActiveScene(scene));
        seq.AppendInterval(1.3f);
        seq.Append(FadeSystem.instance.Willfade(false));
        seq.AppendCallback(() => loadingScene = false);
        seq.Play();
    }

}
