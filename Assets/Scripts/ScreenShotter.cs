using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotter : MonoBehaviour
{
    public Texture2D CaptureScreenshot()
    {
        //截图纹理大小需要与屏幕尺寸匹配
        int width = Screen.width;
        int height = Screen.height;

        //创建临时渲染纹理
        RenderTexture rt = RenderTexture.GetTemporary(width, height, 24);

        //检查主摄像机
        Camera mainCamera = Camera.main;
        if(mainCamera == null)
        {
            Debug.Log(Constants.CAMERA_NOT_FOUND);
            return null;
        }

        //设置摄像机渲染目标
        mainCamera.targetTexture = rt;
        RenderTexture.active = rt;
        mainCamera.Render();

        //读取像素数据
        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();

        //销毁临时渲染纹理
        mainCamera.targetTexture = null;
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);

        //缩小截图
        Texture2D resizedScreenshot = ResizeTexture(screenshot, width/6, height/6);

        //销毁原始截图，释放内存
        Destroy(screenshot);

        return resizedScreenshot;
    }

    private Texture2D ResizeTexture(Texture2D original, int newWidth, int newHeight)
    {
        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight, 24);
        RenderTexture.active = rt;

        //使用GPU缩放
        Graphics.Blit(original, rt);

        Texture2D resized = new Texture2D(newWidth, newHeight, TextureFormat.RGB24, false);
        resized.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        resized.Apply();

        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);

        return resized;
    }
}
