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
    public Button settingsButton;
    public Button exitButton;

    private bool hasStarted = false;

    private void Start() {
        continueButton.interactable = false;
        menuButtonsAddListener();
    }

    private void menuButtonsAddListener()
    {
        startButton.onClick.AddListener(StartGame);
        continueButton.onClick.AddListener(ContinueGame);
        loadButton.onClick.AddListener(LoadGame);
        settingsButton.onClick.AddListener(OpenSettings);
        exitButton.onClick.AddListener(ExitGame);
    }

    private void StartGame()
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

    private void OpenSettings()
    {
        //TODO: 实现设置界面功能
    }

    private void ExitGame()
    {
    }

    private void ShowGamePanel()
    {
        menuPanel.SetActive(false);
        VNManager.Instance.gamePanel.SetActive(true);
    }
}
