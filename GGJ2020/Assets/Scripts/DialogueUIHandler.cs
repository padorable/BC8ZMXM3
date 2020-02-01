using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public struct TweeningUIObject
{
    public RectTransform Object;
    public Vector2 From;
    public Vector2 To;
    public bool IsOut;
    public bool isFlipped;
}

public class DialogueUIHandler : MonoBehaviour
{
    public TweeningUIObject DialogeBox;
    [SerializeField] Text DialogueName;
    [SerializeField] Text DialogueText;
    [Space(10)]

    public TweeningUIObject ImageLeft;
    public TweeningUIObject ImageRight;
    [HideInInspector] public bool IsTyping = false;
    [HideInInspector] public bool CanTypeAgain = true;
    private Coroutine TypingCoroutine;

    public void ShowUIObject(TweeningUIObject obj)
    {
        DOTween.To(() => obj.Object.anchoredPosition, x => obj.Object.anchoredPosition = x, obj.To, 0.5f);
        obj.IsOut = true;
    }

    public void HideUIObject(TweeningUIObject obj)
    {
        DOTween.To(() => obj.Object.anchoredPosition, x => obj.Object.anchoredPosition = x, obj.From, 0.5f);
        obj.IsOut = false;
    }

    // Handles Changing of Image
    public void SetImage(bool isLeft, Sprite spriteToChange, bool isFlipped)
    {
        TweeningUIObject obj = ImageLeft;
        if (!isLeft)
            obj = ImageRight;

        // If there is no sprite, return image then make it null
        if (spriteToChange == null)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(DOTween.To(() => obj.Object.anchoredPosition, x => obj.Object.anchoredPosition = x, obj.From, 0.25f));
            seq.AppendCallback(new TweenCallback(() => obj.Object.GetComponent<Image>().overrideSprite = null));
            seq.PlayForward();
            return;
        }

        // If the same sprite, don't do anything
        if (obj.Object.GetComponent<Image>().sprite != null)
        { 
            if (obj.Object.GetComponent<Image>().overrideSprite.name == spriteToChange.name)
                return;
        }

        // Handles flipping of image
        float flip = Mathf.Abs(obj.Object.localScale.x);
        if (isFlipped) flip *= -1;
        obj.Object.localScale = new Vector3(flip, obj.Object.localScale.y, 1);

        // If the image is out, return the image, change the image, then take out image
        if (obj.IsOut)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(DOTween.To(() => obj.Object.anchoredPosition, x => obj.Object.anchoredPosition = x, obj.From, 0.25f));

            seq.AppendCallback(new TweenCallback(() => 
            {
                obj.Object.GetComponent<Image>().overrideSprite = spriteToChange;
            } ));
            seq.Append(DOTween.To(() => obj.Object.anchoredPosition, x => obj.Object.anchoredPosition = x, obj.To, 0.25f));

            seq.PlayForward();
        }
        // If the image is not out, change image then take out image
        else
        {
            obj.Object.GetComponent<Image>().overrideSprite = spriteToChange;
            DOTween.To(() => obj.Object.anchoredPosition, x => obj.Object.anchoredPosition = x, obj.To, 0.25f);
        }
    }

    // Call to start typing
    public void StartTyping(string name, string dialogue)
    {
        DialogueName.text = name;
        string color = "<color=#FFFFFF00>";
        DialogueText.text = color + dialogue + "</color>";
        IsTyping = true;
        TypingCoroutine = StartCoroutine(TypeDialogue(dialogue));
    }

    IEnumerator TypeDialogue(string dialogue)
    {
        CanTypeAgain = false;
        int current = 0;
        string color = "<color=#FFFFFF00>";
        while (current < dialogue.Length && IsTyping)
        {
            string start = dialogue.Remove(current);
            string end = dialogue.Remove(0, current);

            DialogueText.text = start + color + end + "</color>";
            current++;
            yield return null;
            yield return null;
        }
        DialogueText.text = dialogue;

        yield return new WaitForSeconds(.5f);
        CanTypeAgain = true;
        IsTyping = false;
    }

    // Call to skip tying
    public void SkipTyping()
    {
        IsTyping = false;
    }

    // Call to clear Dialogue Box
    public void ClearDialogue()
    {
        DialogueName.text = "";
        DialogueText.text = "";
    }
}
