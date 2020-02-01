using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ReturnHandler : MonoBehaviour
{
    public CanvasGroup ButtonsGroup;

    private void Start()
    {
        ReturnFromDungeon();
    }

    public void ReturnFromDungeon()
    {
        Camera mainCamera = Camera.main;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(3.0f);
        seq.AppendCallback(()=> DOTween.To(() => mainCamera.transform.position, x => mainCamera.transform.position = x, new Vector3(0, 14,mainCamera.transform.position.z), 3.0f));
        seq.AppendInterval(1.0f);
        seq.AppendCallback(() => { DialogueSystem.instance.StartDialogue(0, false); DialogueSystem.instance.OnDialogueEnd.AddListener(ShowButtons); });
        seq.PlayForward();
    }

    public void TalkToItem()
    {
        DialogueSystem.instance.StartDialogue(0,false);
        HideButtons();
        DialogueSystem.instance.OnDialogueEnd.AddListener(ShowButtons);
    }

    public void ReturnToDungeon()
    {
        DialogueSystem.instance.HideImages();
        HideButtons();
        Camera mainCamera = Camera.main;
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => DOTween.To(() => mainCamera.transform.position, x => mainCamera.transform.position = x, new Vector3(0, 0, mainCamera.transform.position.z), 3.0f));
        seq.AppendInterval(2.0f);
        seq.Append(DOTween.To(() => mainCamera.orthographicSize, x => mainCamera.orthographicSize = x, 3.0f, 3.0f));
        // Return to Dungeon code
        //seq.AppendCallback(()=> )
        seq.PlayForward();
    }

    public void ShowButtons()
    {
        DialogueSystem.instance.OnDialogueEnd.RemoveListener(ShowButtons);
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(1.0f);
        seq.Append(DOTween.To(() => ButtonsGroup.alpha, x => ButtonsGroup.alpha = x, 1f, 1f));
        seq.AppendCallback(() => ButtonsGroup.blocksRaycasts = true);
        seq.PlayForward();
    }

    public void HideButtons()
    {
        ButtonsGroup.blocksRaycasts = false;
        DOTween.To(() => ButtonsGroup.alpha, x => ButtonsGroup.alpha = x, 0, 0.5f);
    }

}
