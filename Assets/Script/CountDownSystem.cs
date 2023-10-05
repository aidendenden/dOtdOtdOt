using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownSystem : MonoBehaviour
{
    public CircularTimer[] circularTimers;
    //public Text text;

    private void Start()
    {
        StartTimer();
    }

    public void StartTimer()
    {
        //text.text = "Start";
        foreach (CircularTimer timer in circularTimers)
        {
            timer.StartTimer();
        }
    }

    public void PauseTimer()
    {
        //text.text = "Pause";
        foreach (CircularTimer timer in circularTimers)
        {
            timer.PauseTimer();
        }
    }

    public void StopTimer()
    {
        //text.text = "Stop";
        foreach (CircularTimer timer in circularTimers)
        {
            timer.StopTimer();
        }
    }
    
    public void ReStart()
    {
        foreach (CircularTimer timer in circularTimers)
        {
            timer.StopTimer();
            timer.StartTimer();
        }
    }

    public void DidFinishedTimer()
    {
        //text.text = "Finished";
    }
}
