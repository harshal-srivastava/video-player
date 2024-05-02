using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void OpenTVButtonPressed()
    {
        homeScreen.SetActive(false);
        galleryScreen.SetActive(true);
    }

    private void OpenTVPlayerScreen()
    {
        galleryScreen.SetActive(false);
        tvPlayerScreen.SetActive(true);
    }

    private void EnableGalleryScreen()
    {
        tvPlayerScreen.SetActive(false);
        galleryScreen.SetActive(true);
    }

    private void AttachEventListeners()
    {
        VideoLibraryManager.LibrarySetCB += OpenTVButtonPressed;
        VideoPlayerController.VideoPlayerReadyCB += OpenTVPlayerScreen;
        VideoPlayerController.VideoStopCB += EnableGalleryScreen;
    }

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
