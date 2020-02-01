using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    public CanvasGroup ButtonGroup;
    public void QuitGame()
    {
        Debug.Log("Quitting...");
        FadeSystem.instance.ChangeColor(Color.black);
        Sequence seq = DOTween.Sequence();
        seq.Append(FadeSystem.instance.Willfade(true));
        seq.AppendInterval(1f);
        seq.AppendCallback(() => { Debug.Log(FadeSystem.instance.name); Application.Quit(); });
        seq.PlayForward();
    }

    public void GoToWatchScene()
    {
        SceneSystem.instance.FadeToNextScene(3, Color.white);
    }

    public void StartGame()
    {
        Camera mainCam = Camera.main;
        DOTween.To(() => mainCam.transform.position, x => mainCam.transform.position = x, new Vector3(0, -7,mainCam.transform.position.z), 8.0f);
        DOTween.To(() => ButtonGroup.alpha, x => ButtonGroup.alpha = x, 0, 1.0f);
        ButtonGroup.blocksRaycasts = false;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(1.0f);
        seq.AppendCallback(() => DialogueSystem.instance.StartDialogue(0));
        seq.PlayForward();
    }

    public void Test()
    {
        SceneSystem.instance.FadeToNextScene("ReturnToDungeon");
    }
}
