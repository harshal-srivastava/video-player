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

    public VideoClip clip;

    public Slider movieSlider;

    float currVideoLength;

    string movieClipFolderPath = "";

    [SerializeField]
    private List<string> availableVideosList;

    private void Start()
    {
        
        // movieClipFolderPath = Application.streamingAssetsPath + "/Videos/Video1.mp4";
        //player.url = movieClipFolderPath;
        //player.Prepare();
        //player.prepareCompleted += PlayVideo;
        movieClipFolderPath = Application.streamingAssetsPath + "/Videos";
        DirectoryInfo d = new DirectoryInfo(movieClipFolderPath);
        string fileName = "";
        foreach (var file in d.GetFiles("*.mp4"))
        {
            availableVideosList.Add(file.FullName);
            

        }
        //player.url = fileName;
       // player.Prepare();
        //player.prepareCompleted += PlayVideo;

        
    }

    void PlayVideo(VideoPlayer source)
    {
        if (source != null)
        {
            source.Play();
        }
        float length = source.frameCount / source.frameRate;
        currVideoLength = length;
        movieSlider.minValue = 0;
        movieSlider.maxValue = currVideoLength;
        movieSlider.onValueChanged.AddListener(ChangeMovieRuntime);
        Debug.Log("length : " + length);
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
