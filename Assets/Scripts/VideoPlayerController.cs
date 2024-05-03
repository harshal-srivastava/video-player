using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.IO;

/// <summary>
/// Class responsible for the video player functionalities
/// Which includes playing video, pause, rewind, fastforward, seek, etc
/// </summary>
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

    public delegate void VideoEndedEvent();
    public static VideoEndedEvent VideoEndedCB;

    private bool isPaused = false;

    private VideoClip currVideo;

    [SerializeField]
    private GameObject videoPlayer3DObject;

    private void Awake()
    {
        AttachEventListeners();
    }

    /// <summary>
    /// Callback function to play the video passed as parameter
    /// </summary>
    /// <param name="video"></param>
    void PlayVideo(VideoClip video)
    {
        //additional condition added because video player
        //shows some issue when we try to play the last video played
        if (player.clip != null)
        {
            player.clip = null;
        }
        player.clip = video;
        player.Prepare();
        player.prepareCompleted += PlayVideoOnPlayer;
        player.loopPointReached += DisableVideoPlayer;
        currVideo = video;
    }

    /// <summary>
    /// Callback function for when video ends
    /// </summary>
    /// <param name="player"></param>
    void DisableVideoPlayer(VideoPlayer player)
    {
        videoPlayer3DObject.SetActive(false);
        VideoEndedCB?.Invoke();
    }

    /// <summary>
    /// Function called when user presses the button to play last played video again
    /// </summary>
    public void PlayCurrentVideoAgain()
    {
        PlayVideo(currVideo);
    }

    /// <summary>
    /// Callback function for when the video player is prepared to show the video
    /// </summary>
    /// <param name="source"></param>
    void PlayVideoOnPlayer(VideoPlayer source)
    {
        VideoPlayerReadyCB?.Invoke();
        if (source != null)
        {
            source.Play();
            EnableVideoScreen();
        }
        SetScreenVariables(source);
    }

    /// <summary>
    /// Set the initial and total length of the video
    /// </summary>
    /// <param name="source"></param>
    void SetScreenVariables(VideoPlayer source)
    {
        currVideoLength = source.frameCount / source.frameRate;
        videoSlider.minValue = 0;
        videoSlider.maxValue = currVideoLength;
        videoSlider.onValueChanged.AddListener(ChangeMovieRuntime);
        UpdateVideoTimeCB?.Invoke(VideoUtility.GetTimeStampFromTotalTime(currVideoLength), true);
    }

    /// <summary>
    /// Function to enable the Quad using the video render texture
    /// </summary>
    void EnableVideoScreen()
    {
        if (!videoPlayer3DObject.activeSelf)
        {
            videoPlayer3DObject.SetActive(true);
        }
    }

    private void Update()
    {
        UpdateVideoSlider();
    }

    /// <summary>
    /// Function to keep updating the slider as the video progresses
    /// </summary>
    void UpdateVideoSlider()
    {
        videoSlider.SetValueWithoutNotify((float)player.time);
        UpdateVideoTimeCB?.Invoke(VideoUtility.GetTimeStampFromTotalTime((float)player.time), false);
    }

    /// <summary>
    /// Callback function to add seek functionality in the video player
    /// </summary>
    /// <param name="value"></param>
    public void ChangeMovieRuntime(float value)
    {
        player.time = value;
        UpdateVideoTimeCB?.Invoke(VideoUtility.GetTimeStampFromTotalTime((float)player.time), false);
    }

    /// <summary>
    /// Function to toggle the video playing or pausing state
    /// </summary>
    public void ToggleVideoPlayPause()
    {
        isPaused = !isPaused;
        UpdateVideoPlayBack();
        VideoPlayPausedCB?.Invoke(isPaused);
    }

    /// <summary>
    /// Function to update the video playback wether it is paused or is playing
    /// </summary>
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

    /// <summary>
    /// Function call to pause the video
    /// </summary>
    private void PauseVideo()
    {
        if (player.isPlaying)
        {
            player.Pause();
        }
    }

    /// <summary>
    /// Function call to play the video from paused state
    /// </summary>
    private void PlayVideo()
    {
        if (player.isPaused)
        {
            player.Play();
        }
    }

    /// <summary>
    /// Function call to fast forward the video
    /// </summary>
    public void FastForward()
    {
        float playerTime = (float)player.time;
        playerTime += fastForwardTime;
        playerTime = Mathf.Clamp(playerTime, 0, currVideoLength);
        player.time = playerTime;
    }

    /// <summary>
    /// Function call to rewind the video
    /// PS: The amount of time forwarded or reversed is controlled by variable "fastForwardTime"
    /// </summary>
    public void Rewind()
    {
        float playerTime = (float)player.time;
        playerTime -= fastForwardTime;
        playerTime = Mathf.Clamp(playerTime, 0, currVideoLength);
        player.time = playerTime;
    }

    /// <summary>
    /// Function to check if videoplayer is running or not
    /// </summary>
    /// <returns></returns>
    bool IsVideoPlayerRunning()
    {
        return player.isPlaying;
    }
    
    /// <summary>
    /// Function called when user presses cross button on video playback screen
    /// </summary>
    public void StopVideo()
    {
            player.Stop();
            player.url = "";
            VideoStopCB?.Invoke();
            isPaused = false;
        videoPlayer3DObject.SetActive(false);
    }

    /// <summary>
    /// Function to attach listeners to respective class game events
    /// </summary>
    void AttachEventListeners()
    {
        VideoLibraryManager.PlayVideoCall += PlayVideo;
    }

    /// <summary>
    /// Function to detach listeners to respective class game events
    /// This is done as a safe keeping in future if a scene reload is required
    /// Static events couple with delegates don't work so well on scene reloads
    /// So detach them if object is destroyed and it will be attached again when instance of class is created
    /// </summary>
    void DetachEventListeners()
    {
        VideoLibraryManager.PlayVideoCall -= PlayVideo;
    }

    private void OnDestroy()
    {
        DetachEventListeners();
    }
}
