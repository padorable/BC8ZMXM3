using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
 
 public class Timer : MonoBehaviour
{
    public Image timerBar;
    public Image Lamp;
    public float timeMax = 60.0f;
    float timeRemaining;

    public UnityEvent OnEnd = new UnityEvent();
    public bool isStart = false;

    public void StartTimer()
    {
        isStart = true;
    }

    private void Start()
    {
        timeRemaining = timeMax;
    }

    private void Update()
    {
        if (!isStart) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timerBar.fillAmount = timeRemaining / timeMax;
            // Lamp image gradually disappears
            Lamp.color = new Color(1,1,1, timeRemaining / timeMax);
            if (timeRemaining <= 0)
                OnEnd.Invoke();
        }
    }

    public void AddTime(float timeToAdd)
    {
        timeRemaining += timeToAdd;
        if (timeRemaining > timeMax) timeRemaining = timeMax;
    }
}