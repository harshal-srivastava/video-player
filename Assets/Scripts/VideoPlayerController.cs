using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.IO;

public class VideoPlayerController : MonoBehaviour
{
    public VideoPlayer player;

    public int fastForwardTime = 5;

    public Slider videoSlider;

    private float currVideoLength;

    public delegate void VideoPlayerReadyEvent();
    public static VideoPlayerReadyEvent VideoPlayerReadyCB;

    public delegate void VideoPlayPausedEvent(bool status);
    public static VideoPlayPausedEvent VideoPlayPausedCB;

    public delegate void UpdateVideoPlaybackTime(string endingtime, bool doOnce);
    public static UpdateVideoPlaybackTime UpdateVideoTimeCB;

    public delegate void VideoStopEvent();
    public static VideoStopEvent VideoStopCB;

    private bool isPaused = false;

    private void Awake()
    {
        AttachEventListeners();
    }

    void PlayVideo(string url)
    {
        player.url = url;
        player.Prepare();
        player.prepareCompleted += PlayVideoOnPlayer;
    }

    void PlayVideoOnPlayer(VideoPlayer source)
    {
        VideoPlayerReadyCB?.Invoke();
        if (source != null)
        {
            source.Play();
        }
        currVideoLength = source.frameCount / source.frameRate;
        videoSlider.minValue = 0;
        videoSlider.maxValue = currVideoLength;
        videoSlider.onValueChanged.AddListener(ChangeMovieRuntime);
        UpdateVideoTimeCB?.Invoke(VideoUtility.GetTimeStampFromTotalTime(currVideoLength), true);
    }

    private void Update()
    {
        UpdateVideoSlider();
    }

    void UpdateVideoSlider()
    {
        videoSlider.SetValueWithoutNotify((float)player.time);
        UpdateVideoTimeCB?.Invoke(VideoUtility.GetTimeStampFromTotalTime((float)player.time), false);
    }

    public void ChangeMovieRuntime(float value)
    {
        player.time = value;
        UpdateVideoTimeCB?.Invoke(VideoUtility.GetTimeStampFromTotalTime((float)player.time), false);
    }

    public void ToggleVideoPlayPause()
    {
        isPaused = !isPaused;
        UpdateVideoPlayBack();
        VideoPlayPausedCB?.Invoke(isPaused);
    }

    private void UpdateVideoPlayBack()
    {
        if (isPaused)
        {
            PauseVideo();
        }
        else
        {
            PlayVideo();
        }
    }

    private void PauseVideo()
    {
        if (player.isPlaying)
        {
            player.Pause();
        }
    }

    private void PlayVideo()
    {
        if (player.isPaused)
        {
            player.Play();
        }
    }

    public void FastForward()
    {
        float playerTime = (float)player.time;
        playerTime += fastForwardTime;
        playerTime = Mathf.Clamp(playerTime, 0, currVideoLength);
        player.time = playerTime;
    }

    public void Rewind()
    {
        float playerTime = (float)player.time;
        playerTime -= fastForwardTime;
        playerTime = Mathf.Clamp(playerTime, 0, currVideoLength);
        player.time = playerTime;
    }

    bool IsVideoPlayerRunning()
    {
        return player.isPlaying;
    }
    
    public void StopVideo()
    {
            player.Stop();
            player.url = "";
            VideoStopCB?.Invoke();
            isPaused = false;
    }

    void AttachEventListeners()
    {
        VideoLibraryManager.PlayVideoCall += PlayVideo;
    }

    void DetachEventListeners()
    {
        VideoLibraryManager.PlayVideoCall -= PlayVideo;
    }

    private void OnDestroy()
    {
        DetachEventListeners();
    }
}
