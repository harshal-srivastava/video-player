using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class VideoUtility
{
    public static string GetTimeStampFromTotalTime(float inputTime)
    {
        TimeSpan time = TimeSpan.FromSeconds(inputTime);
        string timeInString = "";
        if (time.Hours >= 1)
        {
            timeInString = time.ToString(@"hh\:mm\:ss");
        }
        else
        {
            timeInString = time.ToString(@"mm\:ss");
        }
        return timeInString;
    }
}
