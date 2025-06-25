using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.InteropServices;

public class IntroManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private List<string> videoList = new List<string>();
    public static string lastPlayedVideo{ private set; get; } = "";


    private void Start()
    {
        string videopath = Path.Combine(Application.streamingAssetsPath, Constants.videoPath);
        if (Directory.Exists(videopath))
        {
            string[] videoFiles = Directory.GetFiles(videopath, "*" + Constants.VIDEO_FILE_EXTENSION);

            foreach (string videoFile in videoFiles)
            {
                videoList.Add(videoFile);
            }
        }

        PlayRandomVideo();
    }

    private void PlayRandomVideo()
    {
        if (videoList.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, videoList.Count);
            lastPlayedVideo = videoList[randomIndex];       //记录已经播放的视频
            videoPlayer.url = lastPlayedVideo;
            videoPlayer.loopPointReached += OnvideoEnd;
            videoPlayer.Play();
        }
        else
        {
            SceneManager.LoadScene(Constants.MENU_SCENE);
        }
    }

    private void OnvideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(Constants.MENU_SCENE);
    }

    //public static string GetLastPlayedVideo() => lastPlayedVideoPath;
}
