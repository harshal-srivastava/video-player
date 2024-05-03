using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Utility class to store generic requirement functions
/// </summary>
public static class VideoUtility
{
    /// <summary>
    /// Function to convert the given input into hours, minutes and seconds
    /// </summary>
    /// <param name="inputTime"></param>
    /// <returns></returns>
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
