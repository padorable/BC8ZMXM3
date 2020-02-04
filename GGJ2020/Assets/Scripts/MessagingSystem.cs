using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// Used on dungeon

public class MessagingSystem : MonoBehaviour
{
    public static MessagingSystem instance;
    public RectTransform MessageGameObject;
    Coroutine showRoutine;
    public void Message(string print)
    {
        if (showRoutine != null) StopCoroutine(showRoutine);
        MessageGameObject.GetComponentInChildren<Text>().text = print;
        showRoutine = StartCoroutine(Show());
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    IEnumerator Show()
    {
        DOTween.To(() => MessageGameObject.anchoredPosition, x => MessageGameObject.anchoredPosition = x, new Vector2(243, -30), 1.0f);
        float elapsedTime = 0;
        while(elapsedTime <= 3.0f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        DOTween.To(() => MessageGameObject.anchoredPosition, x => MessageGameObject.anchoredPosition = x, new Vector2(0, -30), 1.0f);
    }
}
