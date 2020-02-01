using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueSystem : MonoBehaviour
{
    public DialogueUIHandler UIHandler;

    public List<DialogueSequence> Sequences;
    private DialogueSequence CurrentSequence;
    private bool StartedDialogue = false;

    public static DialogueSystem instance;
    public bool WillRemoveImagesOnEnd = true;

    [HideInInspector] public UnityEvent OnDialogueEnd;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        if (!StartedDialogue) return;

        if (UIHandler.IsTyping)
        {
            Debug.Log("Skip");
            UIHandler.SkipTyping();
        }
        else if (UIHandler.CanTypeAgain)
        {
            Debug.Log("Next");
            NextLine();
            UIHandler.CanTypeAgain = false;
        }
    }

    public void StartDialogue(int index)
    {
        StartDialogue(index, true);
    }
    // Handles the sequence of starting the dialogue
    public void StartDialogue(int index, bool willRemoveImage)
    {
        WillRemoveImagesOnEnd = willRemoveImage;
        CurrentSequence = Sequences[index];
        CurrentSequence.ResetDialogue();

        if (CurrentSequence.CurrentDialogue.LeftImage != null)
        {
            UIHandler.SetImage(true, CurrentSequence.CurrentDialogue.LeftImage, CurrentSequence.CurrentDialogue.FlipLeft);
            UIHandler.ShowUIObject(UIHandler.ImageLeft);
        }

        if (CurrentSequence.CurrentDialogue.RightImage != null)
        {
            UIHandler.SetImage(false, CurrentSequence.CurrentDialogue.RightImage, CurrentSequence.CurrentDialogue.FlipRight);
            UIHandler.ShowUIObject(UIHandler.ImageRight);
        }
        UIHandler.UpdateBackGround(CurrentSequence.CurrentDialogue.Background);
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(1.0f);
        seq.AppendCallback(() => UIHandler.ShowUIObject(UIHandler.DialogeBox));
        seq.AppendInterval(.5f);
        seq.AppendCallback(() => { UIHandler.StartTyping(CurrentSequence.CurrentDialogue.Name, CurrentSequence.CurrentDialogue.DialogueText); StartedDialogue = true; ; });
        seq.PlayForward();
    }

    // Call to read the Next line
    public void NextLine()
    {
        UIHandler.CanTypeAgain = false;
        // if the current dialogue is on its last line, remove all dialogue components
        if(CurrentSequence.IsDone)
        {
            if (WillRemoveImagesOnEnd)
            {
                UIHandler.HideUIObject(UIHandler.ImageLeft);
                UIHandler.HideUIObject(UIHandler.ImageRight);
            }
            UIHandler.HideUIObject(UIHandler.DialogeBox);
            UIHandler.ClearDialogue();
            StartedDialogue = false;
            UIHandler.UpdateBackGround(null);
            OnDialogueEnd.Invoke();
            // End of Dialogue
        }
        else
        {
            // Changes the dialogue, and then changes the images, then starts typing the dialogue
            Dialogue next = CurrentSequence.NextDialogue();
            Sequence seq = DOTween.Sequence();
            seq.AppendCallback(() =>
            {
                UIHandler.SetImage(true, next.LeftImage, next.FlipLeft);
                UIHandler.SetImage(false, next.RightImage, next.FlipRight);
            });
            seq.AppendInterval(.5f);
            seq.AppendCallback(() => UIHandler.StartTyping(next.Name, next.DialogueText));
            seq.PlayForward();
        }
    }

    public void HideImages()
    {
        UIHandler.HideUIObject(UIHandler.ImageLeft);
        UIHandler.HideUIObject(UIHandler.ImageRight);
    }
}
