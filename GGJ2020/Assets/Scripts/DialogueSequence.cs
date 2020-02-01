using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Dialogue
{
    public Sprite LeftImage;
    public bool FlipLeft;
    public Sprite RightImage;
    public bool FlipRight;
    public Sprite BG;
    public string Name;
    public string DialogueText;
    public Sprite Background;
}

public class DialogueSequence : MonoBehaviour
{
    [SerializeField] List<Dialogue> DialogueList;
    private int currentIndex = 0;

    public bool IsDone { get { return currentIndex + 1 >= DialogueList.Count; } }

    public Dialogue NextDialogue()
    {
        currentIndex++;
        return DialogueList[currentIndex];
    }

    public Dialogue CurrentDialogue { get { return DialogueList[currentIndex]; } }
    public void ResetDialogue()
    {
        currentIndex = 0;
    }
}
