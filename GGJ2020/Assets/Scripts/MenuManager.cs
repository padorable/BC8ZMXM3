using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    public CanvasGroup ButtonGroup;
    public CanvasGroup ButtonGroup2;

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
        AudioSystem.instance.PlaySFX(2);
        Camera mainCam = Camera.main;
        DOTween.To(() => mainCam.transform.position, x => mainCam.transform.position = x, new Vector3(0, -7,mainCam.transform.position.z), 8.0f);
        DOTween.To(() => ButtonGroup.alpha, x => ButtonGroup.alpha = x, 0, 1.0f);
        ButtonGroup.blocksRaycasts = false;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(3.0f);
        seq.AppendCallback(() => DialogueSystem.instance.StartDialogue(0));
        seq.PlayForward();
        DialogueSystem.instance.OnDialogueEnd.AddListener(OnEndDialogue);
    }


    public void OnEndDialogue()
    {
        Camera mainCam = Camera.main;
        Sequence seq = DOTween.Sequence();
        ButtonGroup2.blocksRaycasts = true;
        DOTween.To(() => mainCam.transform.position, x => mainCam.transform.position = x, new Vector3(0, -22, mainCam.transform.position.z), 3.0f);
        seq.AppendInterval(5.0f);
        seq.AppendCallback(() => DOTween.To(()=> ButtonGroup2.alpha, x=> ButtonGroup2.alpha = x, 1, 1.0f));
        seq.PlayForward();
    }

    public void Test()
    {
        AudioSystem.instance.PlaySFX(2);
        ButtonGroup2.blocksRaycasts = false;
        DOTween.To(() => ButtonGroup2.alpha, x => ButtonGroup2.alpha = x, 0, 1.0f);
        AudioSystem.instance.PlayMusic(3);
        DialogueSystem.instance.StartDialogue(1);
        DialogueSystem.instance.OnDialogueEnd.RemoveAllListeners();
        DialogueSystem.instance.OnDialogueEnd.AddListener(()=>SceneSystem.instance.FadeToNextScene("RandomGenerator"));
    }

    private void Start()
    {
        AudioSystem.instance.PlayMusic(4);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            DialogueSystem.instance.OnDialogueEnd.RemoveAllListeners();
            SceneSystem.instance.FadeToNextScene("RandomGenerator");
        }
    }
}
