using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Timer : MonoBehaviour
{
    private bool running;
    private float eventTime;
    public delegate void OnTimerDone();
    public OnTimerDone onTimerDone;

    private void Update()
    {    
        if (running)
        {
            TimeEvent();
        }
    }

    internal void StartTimer(float time)
    {
        eventTime = time;
        running = true;
    }

    internal void TimeEvent()
    {
        eventTime -= Time.deltaTime;
        Debug.Log(eventTime.ToString("0"));
        if(eventTime <= 0)
        {
            Debug.Log("Timer Done");
            eventTime = 0;
            running = false; 
            onTimerDone?.Invoke();
            return;
        }
    }

}
