using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoPlayerTest : MonoBehaviour
{
    public VideoPlayer player;

    public int fastForwardTime = 5;

    public VideoClip clip;

    public Slider movieSlider;

    float currVideoLength;

    private void Start()
    {
        currVideoLength = (float)clip.length;
        movieSlider.minValue = 0;
        movieSlider.maxValue = currVideoLength;
        movieSlider.onValueChanged.AddListener(ChangeMovieRuntime);

    }

    public void ChangeMovieRuntime(float value)
    {
        if (!player.isPlaying)
            return;
        player.time = value;
    }

    public void PauseVideo()
    {
        if (player.isPlaying)
        {
            player.Pause();
        }
    }

    public void StopVideo()
    {

    }

    public void PlayVideo()
    {
        if (player.isPaused)
        {
            player.Play();
        }
    }

    public void Skip10Seconds()
    {
        if (!IsVideoPlayerRunning())
            return;
        float playerTime = (float)player.time;
        playerTime += fastForwardTime;
        playerTime = Mathf.Clamp(playerTime, 0, currVideoLength);

        player.time = playerTime;
    }

    public void Rewind10Seconds()
    {
        if (!IsVideoPlayerRunning())
            return;
        float playerTime = (float)player.time;
        playerTime -= fastForwardTime;
        playerTime = Mathf.Clamp(playerTime, 0, currVideoLength);

        player.time = playerTime;
    }

    bool IsVideoPlayerRunning()
    {
        return player.isPlaying;
    }
}
