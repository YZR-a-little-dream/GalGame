using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.IO;
using Newtonsoft.Json;

public class VNManager : SingletonMonoBase<VNManager>
{
    public CharacterImgController characterImgController;

    #region variables
    public GameObject gamePanel;
    public GameObject dialogueBox;
    public TextMeshProUGUI speakerName;

    public TypeWriteEffect typeWriterEffect;

    public ScreenShotter screenShotter;      //截图工具

    public Image avatorImage;               //头像图片
    public AudioSource vocalAudio;          //人声

    public Image backgroundImage;           //背景图片
    public AudioSource backgroundMusic;     //背景音乐

    public Image[] characterImageArr;            //角色立绘列表
      
    private List<ExcelReader.ExcelData> storyData;

    private int currentLine;
    private string currentStoryFileName;
    private float currentTypingSpeed = Constants.DEFAULT_TYPING_SPEED;

    //分支面板
    public GameObject choicePanel;
    public Button choiceButton1;
    public Button choiceButton2;

    //右下角的控制按钮
    public GameObject bottomButtonsPanel;
    public Button autoButton,skipButton,saveButton,loadButton,
    historyButton,settingsButton,homeButton,closeButton;   
    private bool isAutoPlay = false;
    private bool isSkip = false;
    private bool isLoad = false;

    private int maxReachedLineIndex = 0;
    //全局存储每个文件的最远行索引 string:fileName, int:maxReachedLineIndex
    private Dictionary<string,int> globalMaxReachedLineIndicesDict = new Dictionary<string, int>();


    private string saveFolderPath;
    private byte[] screenshotData;          //保存截图数据
    private string currentSpeakingContent;  //保存当前对话内容

    /// <summary>
    /// string:chracterImgName, string:chracterImgLastPos
    /// </summary>
    Dictionary<string, string> characterImgLoadDicts = new Dictionary<string, string>();

    //保存已解锁的背景
    public HashSet<string> unlockedBGHashSets = new HashSet<string>();

    //历史记录
    public LinkedList<string> historyRecords = new LinkedList<string>();
    
    //本地化
    // public class historyData
    // {
    //     public string chineseName;
    //     public string chineseContent;
    //     public string englishName;
    //     public string englishContent;
    //     public string japaneseName;
    //     public string japaneseContent;
    // }

    //private LinkedList<historyData> historyRecords;

    #endregion

    #region life cycle
    void Start()
    {
        InitializeSaveFilePath();
        bottomButtonsAddListener();
    }

    void Update()
    {
        if(!MenuManager.Instance.menuPanel.activeSelf &&
            !SaveLoadManager.Instance.saveLoadPanel.activeSelf &&
            !HistoryManager.Instance.historyScrollView.activeSelf &&
            !SettingManager.Instance.settingPanel.activeSelf &&
            gamePanel.activeSelf  )
        {
            if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                if(!dialogueBox.activeSelf)
                {
                    OpenGameUI();
                }                
                else if(!IsHittingBottomButtons())
                {
                    DisplayNextLine();
                }
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(dialogueBox.activeSelf)   
                {
                    CloseGameUI();
                }
                else{
                    OpenGameUI();
                }
            }

            if(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            {
                Debug.Log("按下Ctrl按键");
                CtrlSkip();
            } 
        }
    }

    #endregion

    #region Initalize
    private void InitializeSaveFilePath()
    {
        saveFolderPath = Path.Combine(Application.persistentDataPath, Constants.SAVE_FILE_PATH);
        if(!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }
    }

    public void startGame(string fileName,int defaultStartLine)
    {
        InitializeAndLoadStory(fileName,defaultStartLine);
    }
    private void bottomButtonsAddListener()
    {
        autoButton.onClick.AddListener(OnAutoButtonClick);
        skipButton.onClick.AddListener(OnSkipButtonClick);
        saveButton.onClick.AddListener(OnSaveButtonClick);
        loadButton.onClick.AddListener(OnLoadButtonClick);
        historyButton.onClick.AddListener(OnHistoryClick);
        settingsButton.onClick.AddListener(OnSettingButtonClick);
        homeButton.onClick.AddListener(OnHomeButtonClick);
        closeButton.onClick.AddListener(OnCloseButtonClick);
    
    }

    private void InitializeAndLoadStory(string fileName,int lineIndex)
    {
        Initialize(lineIndex);
        loadStoryFromFile(fileName);
        if(isLoad)
        {
            RecoverLastBgAndCharcter();
            DisplayNextLine();          //FIXME：自己修改部分可能会出现问题，完成读取文本内容
            isLoad = false;
        }

        DisplayNextLine();
    }

    private void Initialize(int lineIndex)
    {
        currentLine = lineIndex;

        backgroundImage.gameObject.SetActive(false);
        backgroundMusic.gameObject.SetActive(false);

        avatorImage.gameObject.SetActive(false);
        vocalAudio.gameObject.SetActive(false);

        foreach (var img in characterImageArr)
        {
            img.gameObject.SetActive(false);
        }

        choicePanel.SetActive(false);
        //historyRecords = new LinkedList<historyData>();
    }
    

    private void loadStoryFromFile(string fileName)
    {
        currentStoryFileName = fileName;
        //string path = Constants.STORY_PATH + fileName + Constants.DEFAULT_FILE_EXTENSION;
        string path = Path.Combine(Application.streamingAssetsPath,
                                    Constants.LANGUAGE_PATH,
                                    LocalizationManager.Instance.currentLanguage,
                                    fileName + Constants.DEFAULT_FILE_EXTENSION);
        storyData = ExcelReader.ReadExcel(path);
        
        if(storyData == null || storyData.Count == 0)
        {
            Debug.LogError(Constants.NO_DATA_FOUND);
        }

        if(globalMaxReachedLineIndicesDict.ContainsKey(currentStoryFileName))
        {
            maxReachedLineIndex = globalMaxReachedLineIndicesDict[currentStoryFileName]; 
        }
        else{
            maxReachedLineIndex = 0;
            globalMaxReachedLineIndicesDict[currentStoryFileName] = maxReachedLineIndex;
        }
    }
    #endregion

    
    public void SetLanguage()
    {
        loadStoryFromFile(currentStoryFileName);
    }

    public void ReloadStoryLine()
    {
        //historyRecords.RemoveLast();        //移除最后一条记录
        //从当前语言的语句为初始记录
        currentLine--;
        DisplayNextLine();
    }

    #region  Display Line
    private void DisplayNextLine()
    {        
        if (currentLine > maxReachedLineIndex)
        {
            maxReachedLineIndex = currentLine;
            globalMaxReachedLineIndicesDict[currentStoryFileName] = maxReachedLineIndex;
        }
        if(currentLine >= storyData.Count - 1)
        {
            //退出自动播放
            if(isAutoPlay)
            {
                isAutoPlay = false;
                UpdateButtonImage(Constants.AUTO_OFF,autoButton);
            }

            if(storyData[currentLine].speakerName.Equals(Constants.END_OF_STORY))
            {
                Debug.Log(Constants.END_OF_STORY);
            }
            
            if(storyData[currentLine].speakerName.Equals(Constants.CHOICE))
            {
                showChociePanel();
            }

            if(storyData[currentLine].speakerName == Constants.GOTO)
            {
                InitializeAndLoadStory(storyData[currentLine].speakingContent,Constants.DEFAULT_START_LINE);
            }
            return;
        }
        
        if(currentLine >= storyData.Count)
        {
            Debug.Log(Constants.END_OF_STORY);
            return;
        }

        if (typeWriterEffect.IsTyping())
        {
            typeWriterEffect.CompleteLine();
        }
        else
        {
            displayThisLine();
            
        }
    }

    private void displayThisLine()
    {
        var data = storyData[currentLine];

        string playerName = PlayerData.Instance.playerName;
        string speaker = data.speakerName.Replace(Constants.NAME_PLACEHOLDER,playerName);
        string content = data.speakingContent.Replace(Constants.NAME_PLACEHOLDER,playerName);
        speakerName.text = speaker;
        currentSpeakingContent = content;

        // speakerName.text = data.speakerName;
        // currentSpeakingContent = data.speakingContent;
        typeWriterEffect.StartTyping(currentSpeakingContent,currentTypingSpeed);
        
        //记录历史文本
        RecordHistory(speakerName.text,currentSpeakingContent);

        if(NotNullOrEmpty(data.avatorImageFileName))
        {
            UpdateAvatarImage(data.avatorImageFileName);
        }
        else{
            avatorImage.gameObject.SetActive(false);
        }
        
        if(NotNullOrEmpty(data.vocalAudioFileName))
        {
            PlayerVocalAudio(data.vocalAudioFileName);
        }

        if(NotNullOrEmpty(data.bgImageFileName))
        {
            UpdateBackgroundImage(data.bgImageFileName);
        }

        if(NotNullOrEmpty(data.bgMusicFileName))
        {
            PlayBackgroundMusic(data.bgMusicFileName);
        }

        if(NotNullOrEmpty(data.characterAction))
        {
            if(data.characterNum != Constants.DEFAULT_UNEXiST_NUMBER)
            {
                UpdateCharacterImage( data.characterAction, data.characterImgFileName,characterImageArr[data.characterNum]);
            }
        }

        currentLine++;
    }

    private void RecordHistory(string speaker, string currentSpeakingContent)
    {
        string historyRecord = speaker + Constants.COLON + currentSpeakingContent;
        if(historyRecords.Count > Constants.DEFAULT_MAX_LENGTH)
        {
            historyRecords.RemoveFirst();       //移除队列头部元素
        }

        historyRecords.AddLast(historyRecord);
    }

    #endregion

    #region  show choice panel
    private void showChociePanel()
    {
        var data = storyData[currentLine];
        choiceButton1.onClick.RemoveAllListeners();
        choiceButton2.onClick.RemoveAllListeners();
        choicePanel.SetActive(true);
        choiceButton1.GetComponentInChildren<TextMeshProUGUI>().text = data.speakingContent;
        choiceButton1.onClick.AddListener(() => InitializeAndLoadStory(data.avatorImageFileName,Constants.DEFAULT_START_LINE));
        choiceButton2.GetComponentInChildren<TextMeshProUGUI>().text = data.vocalAudioFileName;
        choiceButton2.onClick.AddListener(() => InitializeAndLoadStory(data.bgImageFileName,Constants.DEFAULT_START_LINE));
    }
    #endregion

    #region  Images and Character Animations
    private void UpdateAvatarImage(string imageFileName)
    {
        string avatarImagepath = Constants.AVATAR_PATH + imageFileName;
        UpdateImage(avatarImagepath,avatorImage);
    }

    private void UpdateBackgroundImage(string bgImageFileName)
    {
        string bgImagePath = Constants.BACKGROUND_PATH + bgImageFileName;
        UpdateImage(bgImagePath,backgroundImage);
        if(!unlockedBGHashSets.Contains(bgImageFileName))
        {
            unlockedBGHashSets.Add(bgImageFileName);
        }
    }
    
    private void UpdateCharacterImage( string Action, string imageFileName, Image characterImage)
    {
        if(Action.StartsWith(Constants.CHARACTERACTION_APPEARAT))
        {
            float imgPositionX = CalImgPositionX(Action);

            if (NotLegalFloatNum(imgPositionX))
            {
                string imagePath = Constants.CHARACTER_PATH + imageFileName;
                UpdateImage(imagePath, characterImage);
                Vector2 newPosition = new Vector2(imgPositionX,
                    characterImage.rectTransform.anchoredPosition.y);
                characterImage.rectTransform.anchoredPosition = newPosition;

                int durarution = Constants.DEFAULT_DURATION_TIME;
                if(isLoad || Action.StartsWith(Constants.APPEARAT_INSTANTLY) )
                {
                    durarution = 0;
                }
                characterImage.DOFade(1, durarution).From(0);
            }
            else{
                Debug.LogError(Constants.COORDINATE_MISSING);
            }

        }
        else if(Action.StartsWith(Constants.CHARACTERACTION_DISAPPEAR))
        {
            //隐藏角色立绘,播放消失动画
            characterImage.DOFade(0,Constants.DEFAULT_DURATION_TIME).OnComplete
                (() => characterImage.gameObject.SetActive(false));
        }
        else if(Action.StartsWith(Constants.CHARACTERACTION_MOVETO))
        {
            float imgPositionX = CalImgPositionX(Action);
            //移动立绘位置
            if(NotLegalFloatNum(imgPositionX))
            {
                characterImage.rectTransform.DOAnchorPosX(imgPositionX, Constants.DEFAULT_DURATION_TIME);
            }
            else{
                Debug.LogError(Constants.COORDINATE_MISSING);
            }          
        }
    }

    //计算角色立绘应该出现的位置坐标
    private float CalImgPositionX(string Action)
    {
        // ReadOnlySpan<char> span = Action.AsSpan();
        // ReadOnlySpan<char> coordinatesSpan = span.Slice(numStartPosition,span.Length - IrrelevantChar);
        // var coordinates = coordinatesSpan.ToString().Split(',');

        // float _x = -1;
        // float imgPositionX = float.TryParse(coordinates[0], out _x) ? _x : -1;
        // //float y = float.Parse(coordinates[1]);
        // return imgPositionX;

        ReadOnlySpan<char> span = Action.AsSpan();
        int openBrace = span.IndexOf('(');
        int closeBrace = span.IndexOf(')');

        if (openBrace == -1 || closeBrace == -1 || closeBrace <= openBrace)
            Debug.Log("无效的坐标格式");

        ReadOnlySpan<char> content = span.Slice(openBrace + 1, closeBrace - openBrace - 1);

        string[] coordinates = content.ToString().Split(',');

         float _x = -1;
         float imgPositionX = float.TryParse(coordinates[0], out _x) ? _x : -1;
        // //float y = float.Parse(coordinates[1]);
        return imgPositionX;
    }

    /// <summary>
    /// 更新图片
    /// </summary>
    /// <param name="imagePath">图片资源路径</param>
    /// <param name="characterImg">需要更新的图片组件</param>
    private void UpdateImage(string imagePath, Image characterImg)
    {
        Sprite sprite = Resources.Load<Sprite>(imagePath);
        if(sprite != null)
        {
            characterImg.sprite = sprite;
            characterImg.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError(Constants.IMAGE_LOAD_FAILED + imagePath);
        }
    }
    #endregion

    #region  Music and Audio
    private void PlayerVocalAudio(string audioFileName)
    {
        string audioPath = Constants.VOCAL_PATH + audioFileName;
        PlayAudio(audioPath,vocalAudio,false);
    }

    private void PlayBackgroundMusic(string bgMusicFileName)
    {
        string bgMusicPath = Constants.MUSIC_PATH + bgMusicFileName;
        PlayAudio(bgMusicPath,backgroundMusic,true);
    }

    private void PlayAudio(string bgMusicPath, AudioSource audioSource, bool isLoop)
    {
        AudioClip audioClip = Resources.Load<AudioClip>(bgMusicPath);
        if (audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.gameObject.SetActive(true);
            audioSource.Play();
            audioSource.loop = isLoop;
        }
        else
        {
            if(audioSource == backgroundMusic)
            {
                Debug.LogError(Constants.MUSIC_LOAD_FAILED + bgMusicPath);
            }
            else if (audioSource == vocalAudio)
            {
                Debug.LogError(Constants.AUDIO_LOAD_FAILED + bgMusicPath);
            }
        }
    }

    #endregion

    #region   Utils
    private bool NotNullOrEmpty(string str) => !string.IsNullOrEmpty(str);

    private bool NotLegalFloatNum(float num) => num != Constants.DEFAULT_UNEXiST_NUMBER;
    #endregion


    #region Buttons
    #region  Untils
    private void UpdateButtonImage(string imgFileName, Button autoButton)
    {
        string imagePath = Constants.BUTTON_PATH + imgFileName;
        UpdateImage(imagePath,autoButton.image);
    }
    #endregion

    #region Bottom
    private bool IsHittingBottomButtons()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(
            bottomButtonsPanel.GetComponent<RectTransform>(),
            Input.mousePosition,
            Camera.main
        );
    }
    #endregion
    
    #region AutoPlay
    private void OnAutoButtonClick()
    {
        isAutoPlay = !isAutoPlay;
        UpdateButtonImage((isAutoPlay? Constants.AUTO_ON:Constants.AUTO_OFF),autoButton);
        if(isAutoPlay)
        {
            StartCoroutine(StartAutoPlay());
        }
    }

    private IEnumerator StartAutoPlay()
    {
        while(isAutoPlay)
        {
            if(!typeWriterEffect.IsTyping())
            {
                DisplayNextLine();
            }
            yield return new WaitForSeconds(Constants.DEFAULT_AUTO_WAITING_SECONDS);
        }
    }

    #endregion 
    
    #region Skip
    private void OnSkipButtonClick()
    {
        if(!isSkip && CanSkip())
        {
            StartSkip();
        }
        else if(isSkip){
            StopCoroutine(skipToMaxReachedLine());
            EndSkip();
        }
    }

    private bool CanSkip() => currentLine < maxReachedLineIndex;
    
    private void StartSkip()
    {
        isSkip = true;
        UpdateButtonImage(Constants.SKIP_ON,skipButton);
        currentTypingSpeed = Constants.DEFAULT_SKIP_MODE_TYPING_SPEED;
        StartCoroutine(skipToMaxReachedLine());
    }

    private IEnumerator skipToMaxReachedLine()
    {
        while(isSkip)
        {
            if(CanSkip())
            {
                displayThisLine();
            }
            else{
                EndSkip();
            }
            //控制快速跳过的节奏
            yield return new WaitForSeconds(Constants.DEFAULT_AUTO_WAITING_SECONDS);
        }
    }

    private void EndSkip()
    {
        isSkip = false;
        currentTypingSpeed = Constants.DEFAULT_TYPING_SPEED;
        UpdateButtonImage(Constants.SKIP_OFF,skipButton);
    }

    private void CtrlSkip()
    {
        currentTypingSpeed= Constants.DEFAULT_SKIP_MODE_TYPING_SPEED;
        StartCoroutine(skipWhilePressingCtrl());
    }

    private IEnumerator skipWhilePressingCtrl()
    {
        while(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            DisplayNextLine();
            //控制快速跳过的节奏
            yield return new WaitForSeconds(Constants.DEFAULT_SKIP_WAITING_SECONDS);
        }
    }

    #endregion
    #region save 
    private void OnSaveButtonClick()
    {
        CloseGameUI();
        Texture2D screenshot = screenShotter.CaptureScreenshot();
        screenshotData = screenshot.EncodeToPNG();
        SaveLoadManager.Instance.showSavePanel(SaveGame);
        OpenGameUI();
    }

    private void SaveGame(int slotIndex)
    {
        historyRecords.RemoveLast();                    //确保存储的时候是保存当前行之前的历史记录

        var saveData = new saveData
        {
            saveStoryFileName = currentStoryFileName,
            savedLine = currentLine - 1,                //currentLine在运行displayThisLine()后加一，
                                                        // 所以这里保留的是当前正在显示的行索引的下一行，所以要减一
            currentSpeekingContent = currentSpeakingContent,
            characterImgDicts = characterImgController.GetCharcterImgsPositionDic(),
            savedScreenshotData = screenshotData,
            savedHistoryRecords = historyRecords,
            savedPlayerNmae = PlayerData.Instance.playerName
        };
        string savePath = Path.Combine(saveFolderPath, slotIndex + Constants.SAVE_FILE_EXTENSION);
        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        File.WriteAllText(savePath, json);
    }

    

    public class saveData
    {
        public string saveStoryFileName;        //当前保存的故事文件名
        public int savedLine;                   //当前保存的行索引
        public string currentSpeekingContent;
        public byte[] savedScreenshotData;
        public Dictionary<string, string> characterImgDicts;        //人物立绘列表

        public LinkedList<string> savedHistoryRecords;              //历史存档
        public string savedPlayerNmae;
        
    }

    #endregion

    #region load
    private void OnLoadButtonClick()
    {
        ShowLoadPanel(null);
    }

    internal void ShowLoadPanel(Action action)
    {
        SaveLoadManager.Instance.showLoadPanel(LoadGame,action);
    }

    private void LoadGame(int slotIndex)
    {
        string savePath = Path.Combine(saveFolderPath, slotIndex + Constants.SAVE_FILE_EXTENSION);
        if(File.Exists(savePath))
        {
            isLoad = true;
            string json = File.ReadAllText(savePath);
            saveData saveData = JsonConvert.DeserializeObject<saveData>(json);

            historyRecords = saveData.savedHistoryRecords;
            int lineIndex = saveData.savedLine;

            PlayerData.Instance.playerName = saveData.savedPlayerNmae;

            characterImgLoadDicts = saveData.characterImgDicts;
            InitializeAndLoadStory(saveData.saveStoryFileName,lineIndex);
        }
    }

    private void RecoverLastBgAndCharcter()
    {
        var data = storyData[currentLine];
        if(NotNullOrEmpty(data.lastBgImg))
        {
            UpdateBackgroundImage(data.lastBgImg);
        }
        if(NotNullOrEmpty(data.lastBgMusic))
        {
            PlayBackgroundMusic(data.lastBgMusic);
        }
        
        if(characterImgLoadDicts != null)
        {
            foreach(var ChracterImg in characterImgLoadDicts)
            {
                int characterImgIndex = characterImgController.GetChracterImgIndexByName(characterImageArr, ChracterImg.Key);
                if(characterImgIndex != Constants.DEFAULT_UNEXiST_NUMBER)
                {
                    UpdateCharacterImage(ChracterImg.Value,ChracterImg.Key,characterImageArr[characterImgIndex]);
                }
                else{
                    Debug.LogError(ChracterImg.Key);
                }
            }
        }
        
    }
    #endregion

    #region History
    private void OnHistoryClick()
    {
        HistoryManager.Instance.ShowHistory(historyRecords);
    }
    #endregion


    #region Home
     private void OnHomeButtonClick()
    {
        gamePanel.SetActive(false);
        MenuManager.Instance.menuPanel.SetActive(true);
    }
    #endregion
    
    #region  Close
    private void OnCloseButtonClick()
    {
        CloseGameUI();
    }
    private void OpenGameUI()
    {
        dialogueBox.SetActive(true);
        bottomButtonsPanel.SetActive(true);
    }

    private void CloseGameUI()
    {
        dialogueBox.SetActive(false);
        bottomButtonsPanel.SetActive(false);
    }

    

    #endregion

    #region Settings
    private void OnSettingButtonClick()
    {
        SettingManager.Instance.ShowSettingPanel();
    }
    #endregion
    #endregion
}
