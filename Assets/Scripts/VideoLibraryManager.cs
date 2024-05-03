using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.Video;

/// <summary>
/// Class responsible for loading all the videos in the application and displaying them in the gallery
/// </summary>
public class VideoLibraryManager : MonoBehaviour
{
    [SerializeField]
    private List<VideoClip> availableLibraryOfVideosList;

    private string videosLibraryPath = "";

    [SerializeField]
    private GameObject videoThumbnailPrefab;

    [SerializeField]
    private GameObject videoThumbnailHolder;

    public delegate void LibraryGridSetCallBack();
    public static LibraryGridSetCallBack LibrarySetCB;

    public delegate void PlayVideoEvent(VideoClip video);
    public static PlayVideoEvent PlayVideoCall;

    private void Awake()
    {
        GetAllAvailableVideos();
    }

    /// <summary>
    /// Function to load all available videos from resources folder
    /// </summary>
    private void GetAllAvailableVideos()
    {
        VideoClip[] items = Resources.LoadAll<VideoClip>("Videos/");
        for (int i=0;i<items.Length;i++)
        {
            availableLibraryOfVideosList.Add(items[i]);
        }
    }


    /// <summary>
    /// Function called when user presses the button on TV
    /// </summary>
    public void OpenTVPressed()
    {
        SetLibraryGrid();
    }

    /// <summary>
    /// Sets the display grid of the gallery with all available videos
    /// </summary>
    private void SetLibraryGrid()
    {
        for (int i=0;i<availableLibraryOfVideosList.Count;i++)
        {
            Button videoThumbnail = Instantiate(videoThumbnailPrefab, videoThumbnailHolder.transform).GetComponent<Button>();
            int j = i;
            videoThumbnail.onClick.AddListener(()=>PlayVideoWithIndex(j));
        }
        LibrarySetCB?.Invoke();
    }

    /// <summary>
    /// Single listener funtion for all video thumbnail buttons
    /// </summary>
    /// <param name="index"></param>
    private void PlayVideoWithIndex(int index)
    {
        Debug.Log("playing video : " + index);
        PlayVideoCall?.Invoke(availableLibraryOfVideosList[index]);
    }
}
