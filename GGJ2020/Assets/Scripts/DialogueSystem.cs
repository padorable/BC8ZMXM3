using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueSystem : MonoBehaviour
{
    public DialogueUIHandler UIHandler;

    public DialogueSequence CurrentSequence;
    private bool StartedDialogue = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (!StartedDialogue)
            {
                StartDialogue();

                StartedDialogue = true;
            }
            else
            {
                NextLine();
            }
        }
    }

    // Handles the sequence of starting the dialogue
    public void StartDialogue()
    {
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

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(1.0f);
        seq.AppendCallback(() => UIHandler.ShowUIObject(UIHandler.DialogeBox));
        seq.AppendInterval(.5f);
        seq.AppendCallback(() => UIHandler.StartTyping(CurrentSequence.CurrentDialogue.Name, CurrentSequence.CurrentDialogue.DialogueText));
        seq.PlayForward();
    }

    // Call to read the Next line
    public void NextLine()
    {
        // if the current dialogue is on its last line, remove all dialogue components
        if(CurrentSequence.IsDone)
        {
            UIHandler.HideUIObject(UIHandler.ImageLeft);
            UIHandler.HideUIObject(UIHandler.ImageRight);
            UIHandler.HideUIObject(UIHandler.DialogeBox);
            UIHandler.ClearDialogue();
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
}
