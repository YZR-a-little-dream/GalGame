using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : SingletonMonoBase<MenuManager>
{
    public Image backgroundImage;
    
    public Button startButton;
    public Button continueButton;
    public Button loadButton;
    public Button galleryButton;
    public Button settingsButton;
    public Button exitButton;
    public Button languagebutton;

    private int currentLanguageIndex = Constants.DEFAULT_LANGUAGE_INDEX;

    // private bool hasStarted = false;

    // //多语言本地化
    // public TextMeshProUGUI languageBtnText;
    // private string currentLanguage;
    // private int lastLanguageIndex = Constants.DEFAULE_LANGUAGE_INDEX;
    

    private void Start()
    {
        //记录当前场景
        GameManager.Instance.currentScene = Constants.MENU_SCENE;
        if (GameManager.Instance.hasStarted)
        {
            continueButton.interactable = true;
        }
        else
        {
            continueButton.interactable = false;
        }
        menuButtonsAddListener();

        currentLanguageIndex = GameManager.Instance.currentLanguageIndex;
        LocalizationManager.Instance.LoadLanguage(Constants.LANGUAGES[currentLanguageIndex]);
        updateLanguageBtnText();

        //用属性代替静态方法
        string lastPlayedVideo = IntroManager.lastPlayedVideo;
        if (!string.IsNullOrEmpty(lastPlayedVideo))
        {
            string videoFileName = Path.GetFileNameWithoutExtension(lastPlayedVideo);
            string imagePath = Constants.BACKGROUND_PATH + videoFileName;
            Sprite sprite = Resources.Load<Sprite>(imagePath);
            if (sprite != null)
            {
                backgroundImage.sprite = sprite;
            }
            else
            {
                Debug.LogWarning(Constants.IMAGE_LOAD_FAILED + imagePath);
            }
        }
    }

    private void menuButtonsAddListener()
    {
        startButton.onClick.AddListener(StartGame);
        //startButton.onClick.AddListener(ShowInputPanel);
        continueButton.onClick.AddListener(ContinueGame);
        loadButton.onClick.AddListener(LoadGame);
        galleryButton.onClick.AddListener(() => SceneManager.LoadScene(Constants.Gallery_SCENE));
        settingsButton.onClick.AddListener(() => SceneManager.LoadScene(Constants.Setting_SCENE));
        exitButton.onClick.AddListener(ExitGame);
        languagebutton.onClick.AddListener(UpdateLanguage);
    }

    // private void ShowInputPanel()
    // {
    //     InputManager.Instance.showInputPanel();
    // }

    public void StartGame()
    {
        GameManager.Instance.hasStarted = true;
        GameManager.Instance.currentStoryFile = Constants.DEFAULT_STORY_FILE_NAME;
        GameManager.Instance.currentLineIndex = Constants.DEFAULT_STORY_START_LINE;
        GameManager.Instance.currentBGImg = string.Empty;
        GameManager.Instance.currentBGMusic = string.Empty;
        //TODO: @1 立绘位置初始化
        GameManager.Instance.curCharacterName_ActionDic = new Dictionary<string, string>();
        
        //FIXME: 历史记录
        GameManager.Instance.historyRecords = new LinkedList<string>();

        //FIXME:  直接进入游戏画面而不是输入玩家名字
        SceneManager.LoadScene(Constants.GAME_SCENE);

        // continueButton.interactable = true;
        // if(lastLanguageIndex != currentLanguageIndex)
        // {
        //     SetLanguage();
        // }

        // VNManager.Instance.startGame(Constants.DEFAULT_STORY_FILE_NAME,Constants.DEFAULT_START_LINE);
        // ShowGamePanel();
    }

    private void ContinueGame()
    {
        // if(hasStarted)
        // {
        //     if(lastLanguageIndex != currentLanguageIndex)
        //     {
        //         SetLanguage();
        //         VNManager.Instance.ReloadStoryLine();
        //     }
        //     ShowGamePanel();
        // }

        if (GameManager.Instance.hasStarted)
        {
            //退出游戏时添加一遍历史记录 进入游戏后又添加一条历史记录 所以要删除一条
            GameManager.Instance.historyRecords.RemoveLast();
            SceneManager.LoadScene(Constants.GAME_SCENE);
        }
    }

    private void LoadGame()
    {
        // if(lastLanguageIndex != currentLanguageIndex)
        // {
        //     SetLanguage();
        // }
        // //实现加载游戏功能
        // VNManager.Instance.ShowLoadPanel(ShowGamePanel);

        GameManager.Instance.currentSaveLoadMode = GameManager.SaveLoadMode.Load;
        SceneManager.LoadScene(Constants.SAVE_LOAD_SCENE);
    }

    // private void SetLanguage()
    // {
    //     lastLanguageIndex = currentLanguageIndex;
    //     VNManager.Instance.SetLanguage();
        
    //     //清空之前语言的历史记录
    //     VNManager.Instance.historyRecords.Clear();
    //     //清空现有历史记录
    //     foreach (Transform child in HistoryManager.Instance.historyContent)
    //     {
    //         Destroy(child.gameObject);
    //     }
    // }
    

    // private void ShowGalleryPanel()
    // {
    //     //GalleryManager
    //     GalleryManager.Instance.ShowGalleryPanel();
    // }

    // private void OpenSettings()
    // {
    //     //实现设置界面功能
    //     SettingManager.Instance.ShowSettingPanel();
    // }

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
        LocalizationManager.Instance.LoadLanguage(Constants.LANGUAGES[currentLanguageIndex]);

        GameManager.Instance.currentLanguageIndex = currentLanguageIndex;
        GameManager.Instance.currentLanguage = Constants.LANGUAGES[currentLanguageIndex];
        
        updateLanguageBtnText();
    }

    private void updateLanguageBtnText()
    {
        // switch(currentLanguageIndex)
        // {
        //     case 0 :
        //         languageBtnText.text = Constants.CHINESE;
        //         break;
        //     case 1:
        //         languageBtnText.text = Constants.ENGLISH;
        //         break;
        //     case 2: 
        //         languageBtnText.text = Constants.JAPANESE;
        //         break;
        // }
        TextMeshProUGUI languageButtonTMP = languagebutton.GetComponentInChildren<TextMeshProUGUI>();
        languageButtonTMP.text = LocalizationManager.Instance.GetLocalizedValue(Constants.LANGUAGES[currentLanguageIndex]);       
    }

    // private void ShowGamePanel()
    // {
    //     menuPanel.SetActive(false);
    //     VNManager.Instance.gamePanel.SetActive(true);
    // }
}
