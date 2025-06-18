using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    public Button languagebutton;

    private bool hasStarted = false;

    //多语言本地化
    public TextMeshProUGUI languageBtnText;
    private string currentLanguage;
    private int lastLanguageIndex = Constants.DEFAULE_LANGUAGE_INDEX;
    private int currentLanguageIndex = Constants.DEFAULE_LANGUAGE_INDEX;

    private void Start() {
        continueButton.interactable = false;
        menuButtonsAddListener();
        LocalizationManager.Instance.LoadLanguage(Constants.DEFAULT_LANGUAGE);
        updateLanguageBtnText();
    }

    private void menuButtonsAddListener()
    {
        startButton.onClick.AddListener(StartGame);
        //startButton.onClick.AddListener(ShowInputPanel);
        continueButton.onClick.AddListener(ContinueGame);
        loadButton.onClick.AddListener(LoadGame);
        galleryButton.onClick.AddListener(ShowGalleryPanel);
        settingsButton.onClick.AddListener(OpenSettings);
        exitButton.onClick.AddListener(ExitGame);
        languagebutton.onClick.AddListener(UpdateLanguage);
    }

    private void ShowInputPanel()
    {
        InputManager.Instance.showInputPanel();
    }

    public void StartGame()
    {
        hasStarted = true;
        continueButton.interactable = true;
        if(lastLanguageIndex != currentLanguageIndex)
        {
            SetLanguage();
        }
        
        VNManager.Instance.startGame(Constants.DEFAULT_STORY_FILE_NAME,Constants.DEFAULT_START_LINE);
        ShowGamePanel();
    }

    private void ContinueGame()
    {
        if(hasStarted)
        {
            if(lastLanguageIndex != currentLanguageIndex)
            {
                SetLanguage();
                VNManager.Instance.ReloadStoryLine();
            }
            ShowGamePanel();
        }
    }

    private void LoadGame()
    {
        if(lastLanguageIndex != currentLanguageIndex)
        {
            SetLanguage();
        }
        //实现加载游戏功能
        VNManager.Instance.ShowLoadPanel(ShowGamePanel);
    }

    private void SetLanguage()
    {
        lastLanguageIndex = currentLanguageIndex;
        VNManager.Instance.SetLanguage();
        
        //清空之前语言的历史记录
        VNManager.Instance.historyRecords.Clear();
        //清空现有历史记录
        foreach (Transform child in HistoryManager.Instance.historyContent)
        {
            Destroy(child.gameObject);
        }
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

    private void UpdateLanguage()
    {
        currentLanguageIndex = (currentLanguageIndex + 1) % Constants.LANGUAGES.Length;
        currentLanguage = Constants.LANGUAGES[currentLanguageIndex];
        LocalizationManager.Instance.LoadLanguage(currentLanguage);
        updateLanguageBtnText();
    }

    private void updateLanguageBtnText()
    {
        switch(currentLanguageIndex)
        {
            case 0 :
                languageBtnText.text = Constants.CHINESE;
                break;
            case 1:
                languageBtnText.text = Constants.ENGLISH;
                break;
            case 2: 
                languageBtnText.text = Constants.JAPANESE;
                break;
        }

    }

    private void ShowGamePanel()
    {
        menuPanel.SetActive(false);
        VNManager.Instance.gamePanel.SetActive(true);
    }
}
