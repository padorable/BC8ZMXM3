using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

// This is used to switch scenes, so that persistent scenes do not get unloaded

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

    // This is the usual start, but as an IEnumerator
    private IEnumerator Start()
    {
        // Loads Dialogue System before opening Main Menu
        AsyncOperation op = SceneManager.LoadSceneAsync("DialogueSystem", LoadSceneMode.Additive);
        while (!op.isDone) yield return null;

        op = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
        //op = SceneManager.LoadSceneAsync("RandomGenerator", LoadSceneMode.Additive);
        while (!op.isDone) yield return null;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainMenu"));
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("RandomGenerator"));
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
        if (loadingScene)
            return;
        loadingScene = true;

        AsyncOperation op = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        op.allowSceneActivation = false;
        Scene nextScene = SceneManager.GetSceneByName(scene);
        FadeSystem.instance.ChangeColor(color);

        // Basically Fade first, then activate the new scene while removing the current scene
        // then setting the new scene as the active scene, and then fade out

        Sequence seq = DOTween.Sequence();
        seq.Append(FadeSystem.instance.Willfade(true));
        seq.AppendCallback(() =>
        {
            Scene current = SceneManager.GetActiveScene();
            op.allowSceneActivation = true;
            AsyncOperation ap = SceneManager.UnloadSceneAsync(current);
        });
        seq.AppendInterval(.2f);
        seq.AppendCallback(() => SceneManager.SetActiveScene(nextScene));
        seq.AppendInterval(1.3f);
        seq.Append(FadeSystem.instance.Willfade(false));
        seq.AppendCallback(() => loadingScene = false);
        seq.Play();
    }

    public void FadeToNextScene(Scene scene, Color color, AsyncOperation op)
    {

    }

}
