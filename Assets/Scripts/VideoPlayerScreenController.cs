using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public void PlayPauseButtonPressed(bool isPaused)
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

    private void AttachEventListeners()
    {
        VideoPlayerController.VideoPlayPausedCB += PlayPauseButtonPressed;
        VideoPlayerController.UpdateVideoTimeCB += UpdateVideoPlayBackTime;
        VideoPlayerController.VideoStopCB += StopVideoAndDisableVideoScreen;
        VideoPlayerController.VideoEndedCB += EnablePlayAgainButton;
    }

    private void StopVideoAndDisableVideoScreen()
    {
        videoStartingTimeText.text = "";
        videoEndingTimeText.text = "";
        videoCurrentTimeText.text = "";
        pauseButton.GetComponent<Image>().sprite = pauseVideoSprite;
        playAgainButton.SetActive(false);
    }

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

    private void EnablePlayAgainButton()
    {
        playAgainButton.SetActive(true);
        videoPlayBackControlsGroup.alpha = 0;
    }


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
