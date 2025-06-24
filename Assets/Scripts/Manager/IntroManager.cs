using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private string videoPath =  "video/spring.mp4";

    private void Start()
    {
        string fullpath = Path.Combine(Application.streamingAssetsPath, videoPath);
        videoPlayer.url = fullpath;
        videoPlayer.loopPointReached += OnvideoEnd;
        videoPlayer.Play();
    }

    private void OnvideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene("MainGame");
    }
}
