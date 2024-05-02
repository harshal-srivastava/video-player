using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class VideoLibraryManager : MonoBehaviour
{
    [SerializeField]
    private List<string> availableLibraryOfVideosList;

    private string videosLibraryPath = "";

    [SerializeField]
    private GameObject videoThumbnailPrefab;

    [SerializeField]
    private GameObject videoThumbnailHolder;

    public delegate void LibraryGridSetCallBack();
    public static LibraryGridSetCallBack LibrarySetCB;

    public delegate void PlayVideoEvent(string videoURL);
    public static PlayVideoEvent PlayVideoCall;

    private void Awake()
    {
        GetAllAvailableVideos();
    }

    private void GetAllAvailableVideos()
    {
        videosLibraryPath = Application.streamingAssetsPath;
        DirectoryInfo d = new DirectoryInfo(videosLibraryPath);
        foreach (var file in d.GetFiles("*.mp4"))
        {
            availableLibraryOfVideosList.Add(file.FullName);
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
