using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class responsible for updating visuals and UI when the video is being played
/// </summary>
public class VideoPlayerScreenController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup videoPlayBackControlsGroup;

    [SerializeField]
    private Button pauseButton;

    [SerializeField]
    private GameObject playAgainButton;

    [SerializeField]
    private Sprite pauseVideoSprite;

    [SerializeField]
    private Sprite playVideoSprite;

    [SerializeField]
    private TextMeshProUGUI videoStartingTimeText;

    [SerializeField]
    private TextMeshProUGUI videoEndingTimeText;

    [SerializeField]
    private TextMeshProUGUI videoCurrentTimeText;


    private void Awake()
    {
        AttachEventListeners();
    }

    /// <summary>
    /// Callback function to update the pause button sprite
    /// </summary>
    /// <param name="isPaused"></param>
    private void PlayPauseButtonPressed(bool isPaused)
    {
        if (isPaused)
        {
            pauseButton.GetComponent<Image>().sprite = playVideoSprite;
        }
        else
        {
            pauseButton.GetComponent<Image>().sprite = pauseVideoSprite;
        }
    }

    /// <summary>
    /// Function to attach listeners to respective class game events
    /// </summary>
    private void AttachEventListeners()
    {
        VideoPlayerController.VideoPlayPausedCB += PlayPauseButtonPressed;
        VideoPlayerController.UpdateVideoTimeCB += UpdateVideoPlayBackTime;
        VideoPlayerController.VideoStopCB += StopVideoAndDisableVideoScreen;
        VideoPlayerController.VideoEndedCB += EnablePlayAgainButton;
    }

    /// <summary>
    /// Function call to reset the visual elements to their default state when user presses cross button
    /// </summary>
    private void StopVideoAndDisableVideoScreen()
    {
        videoStartingTimeText.text = "";
        videoEndingTimeText.text = "";
        videoCurrentTimeText.text = "";
        pauseButton.GetComponent<Image>().sprite = pauseVideoSprite;
        playAgainButton.SetActive(false);
    }

    /// <summary>
    /// Function to update the video initial, current and final time
    /// </summary>
    /// <param name="time"></param>
    /// <param name="doOnce"></param>
    private void UpdateVideoPlayBackTime(string time, bool doOnce)
    {
        if (doOnce)
        {
            videoStartingTimeText.text = time.Length == 5 ? "00:00" :"00:00:00";
            videoEndingTimeText.text = time;
            if (videoPlayBackControlsGroup.alpha == 0)
            {
                videoPlayBackControlsGroup.alpha = 1;
                playAgainButton.SetActive(false);
            }
        }
        else
        {
            videoCurrentTimeText.text = time;
        }
    }

    /// <summary>
    /// Function to enable play again button after a video is finished
    /// </summary>
    private void EnablePlayAgainButton()
    {
        playAgainButton.SetActive(true);
        videoPlayBackControlsGroup.alpha = 0;
    }

    /// <summary>
    /// Function to detach listeners to respective class game events
    /// This is done as a safe keeping in future if a scene reload is required
    /// Static events couple with delegates don't work so well on scene reloads
    /// So detach them if object is destroyed and it will be attached again when instance of class is created
    /// </summary>
    private void DetachEventListeners()
    {
        VideoPlayerController.VideoPlayPausedCB -= PlayPauseButtonPressed;
        VideoPlayerController.UpdateVideoTimeCB -= UpdateVideoPlayBackTime;
        VideoPlayerController.VideoStopCB -= StopVideoAndDisableVideoScreen;
        VideoPlayerController.VideoEndedCB -= EnablePlayAgainButton;
    }

    private void OnDestroy()
    {
        DetachEventListeners();
    }
}
