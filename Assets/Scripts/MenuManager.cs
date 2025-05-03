using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : SingletonMonoBase<MenuManager>
{
    public GameObject menuPanel;
    public Button startButton;
    public Button continueButton;
    public Button loadButton;
    public Button galleryButton;
    public Button settingsButton;
    public Button exitButton;

    private bool hasStarted = false;

    private void Start() {
        continueButton.interactable = false;
        menuButtonsAddListener();
    }

    private void menuButtonsAddListener()
    {
        //startButton.onClick.AddListener(StartGame);
        startButton.onClick.AddListener(ShowInputPanel);
        continueButton.onClick.AddListener(ContinueGame);
        loadButton.onClick.AddListener(LoadGame);
        galleryButton.onClick.AddListener(ShowGalleryPanel);
        settingsButton.onClick.AddListener(OpenSettings);
        exitButton.onClick.AddListener(ExitGame);
    }

    private void ShowInputPanel()
    {
        InputManager.Instance.showInputPanel();
    }

    public void StartGame()
    {
        hasStarted = true;
        continueButton.interactable = true;
        ShowGamePanel();
        VNManager.Instance.startGame();
    }

    private void ContinueGame()
    {
        if(hasStarted)
        {
            ShowGamePanel();
        }
    }

    private void LoadGame()
    {
        //实现加载游戏功能
        VNManager.Instance.ShowLoadPanel(ShowGamePanel);
    }

    private void ShowGalleryPanel()
    {
        //GalleryManager
        GalleryManager.Instance.ShowGalleryPanel();
    }

    private void OpenSettings()
    {
        //实现设置界面功能
        SettingManager.Instance.ShowSettingPanel();
    }

    private void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;    // 编辑器模式下停止播放
        #else
            Application.Quit();                                 // 构建后退出应用
        #endif
    }

    private void ShowGamePanel()
    {
        menuPanel.SetActive(false);
        VNManager.Instance.gamePanel.SetActive(true);
    }
}
