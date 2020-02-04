using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndingSequence : MonoBehaviour
{
    public CanvasGroup group;
    // Start is called before the first frame update
    void Start()
    {
        AudioSystem.instance.PlayMusic(2);
        DialogueSystem.instance.StartDialogue(2);
        DialogueSystem.instance.OnDialogueEnd.AddListener(()=> Invoke("Next",3.0f));
    }

    private void Next()
    {
        AudioSystem.instance.PlayMusic(1);
        DOTween.To(() => group.alpha, x => group.alpha = x, 1, 1.0f);
        DialogueSystem.instance.StartDialogue(3);
        DialogueSystem.instance.OnDialogueEnd.RemoveListener(Next);
        DialogueSystem.instance.OnDialogueEnd.AddListener(() => SceneSystem.instance.FadeToNextScene((1)));
    }
}
