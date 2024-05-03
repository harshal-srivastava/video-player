using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for the UI/UX Flow of the application
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject homeScreen;

    [SerializeField]
    private GameObject galleryScreen;

    [SerializeField]
    private GameObject tvPlayerScreen;

    private void Awake()
    {
        AttachEventListeners();
    }

    /// <summary>
    /// Function called when user press the button on TV
    /// </summary>
    private void OpenTVButtonPressed()
    {
        homeScreen.SetActive(false);
        galleryScreen.SetActive(true);
    }

    /// <summary>
    /// Function called when user selects any one video to play
    /// </summary>
    private void OpenTVPlayerScreen()
    {
        galleryScreen.SetActive(false);
        tvPlayerScreen.SetActive(true);
    }

    /// <summary>
    /// Callback function to show the gallery
    /// </summary>
    private void EnableGalleryScreen()
    {
        tvPlayerScreen.SetActive(false);
        galleryScreen.SetActive(true);
    }

    /// <summary>
    /// Function to attach listeners to respective class game events
    /// </summary>
    private void AttachEventListeners()
    {
        VideoLibraryManager.LibrarySetCB += OpenTVButtonPressed;
        VideoPlayerController.VideoPlayerReadyCB += OpenTVPlayerScreen;
        VideoPlayerController.VideoStopCB += EnableGalleryScreen;
    }

    /// <summary>
    /// Function to detach listeners to respective class game events
    /// This is done as a safe keeping in future if a scene reload is required
    /// Static events couple with delegates don't work so well on scene reloads
    /// So detach them if object is destroyed and it will be attached again when instance of class is created
    /// </summary>
    private void DetachEventListeners()
    {
        VideoLibraryManager.LibrarySetCB -= OpenTVButtonPressed;
        VideoPlayerController.VideoPlayerReadyCB -= OpenTVPlayerScreen;
        VideoPlayerController.VideoStopCB -= EnableGalleryScreen;
    }

    private void OnDestroy()
    {
        DetachEventListeners();
    }



}
