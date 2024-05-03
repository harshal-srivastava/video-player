using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.Video;

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

    public TextMeshProUGUI debugText;

    private void Awake()
    {
        GetAllAvailableVideos();
    }

    private void GetAllAvailableVideos()
    {
        /*videosLibraryPath = Application.streamingAssetsPath;
        DirectoryInfo d = new DirectoryInfo(videosLibraryPath);
        Debug.LogError("[video debugging] directory path : " + d.FullName);
        Debug.LogError("[video debugging] directory path : " + videosLibraryPath);
        foreach (var file in d.GetFiles("*.mp4"))
        {
            availableLibraryOfVideosList.Add(file.FullName);
        }*/
        VideoClip[] items = Resources.LoadAll<VideoClip>("Videos/");
        for (int i=0;i<items.Length;i++)
        {
            availableLibraryOfVideosList.Add(items[i]);
        }
    }

    public void OpenTVPressed()
    {
        SetLibraryGrid();
    }

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

    private void PlayVideoWithIndex(int index)
    {
        Debug.Log("playing video : " + index);
        PlayVideoCall?.Invoke(availableLibraryOfVideosList[index]);
    }
}
