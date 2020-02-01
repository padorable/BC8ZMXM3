using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
 public class Timer : MonoBehaviour
{
    Image timerBar;
    public float timeMax = 60.0f;
    float timeRemaining;

    private void Start()
    {
        timerBar = GetComponent<Image>();
        timeRemaining = timeMax;
    }

    private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timerBar.fillAmount = timeRemaining / timeMax;
        }
    }

    public void AddTime(float timeToAdd)
    {
        timeRemaining += timeToAdd;
        if (timeRemaining > timeMax) timeRemaining = timeMax;
    }
}