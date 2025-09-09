using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerClass
{
    bool Started;
    float TimeStarted;
    float TimeFinished;
    float Duration;

    float InitPercentage = 0;
    public TimerClass(bool started, float duration = 0, float currentTime = 0, float initPercentage = 0)
    {
        Started = started;
        Duration = duration;
        TimeStarted = currentTime;
        TimeFinished = TimeStarted + duration;
        InitPercentage = initPercentage;
    }

    public float GetTimeFinished()
    {
        return TimeFinished - (InitPercentage * Duration);
    }

    public float percentageComplete(float currentTime)
    {
        return Mathf.Clamp01((currentTime + (InitPercentage*Duration) - TimeStarted) / Duration);
    }

    public bool TimePastThreshold(float threshold, float currentTime)
    {
        return(TimeStarted + threshold) < currentTime;
    }

    public bool IsOn()
    {
        return Started;
    }

    public void TurnOff()
    {
        Started = false;
    }

    public bool TimerStillGoing(float currentTime)
    {
        if (!Started) { return false; }
        if (currentTime + (InitPercentage * Duration) < TimeFinished)
        {
            return true;
        }
        Started = false;
        return false;
    }
}
