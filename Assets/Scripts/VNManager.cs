using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class VNManager : SingletonMonoBase<VNManager>
{
    public GameObject gamePanel;
    public GameObject dialogueBox;
    public TextMeshProUGUI speakerName;
    public TextMeshProUGUI speakingContent;

    public TypeWriteEffect typeWriterEffect;

    public Image avatorImage;               //头像图片
    public AudioSource vocalAudio;          //人声

    public Image backgroundImage;           //背景图片
    public AudioSource backgroundMusic;     //背景音乐

    public Image[] characterImageArr;            //角色立绘列表
      
    private List<ExcelReader.ExcelData> storyData;

    private int currentLine;
    private string currentStoryFileName;

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

    private int maxReachedLineIndex = 0;
    //全局存储每个文件的最远行索引 string:fileName, int:maxReachedLineIndex
    private Dictionary<string,int> globalMaxReachedLineIndicesDict = new Dictionary<string, int>();

    void Start()
    {
        bottomButtonsAddListener();
        gamePanel.SetActive(false);
    }
    
    public void startGame()
    {
        InitializeAndLoadStory(Constants.DEFAULT_STORY_FILE_NAME);
    }

    void Update()
    {
        if(gamePanel.activeSelf && Input.GetMouseButton(0))
        {
            if(!dialogueBox.activeSelf)
            {
                OpenGameUI();
            }                
            else if(!IsHittingBottomButtons())
            {
                if(!SaveLoadManager.Instance.saveLoadPanel.activeSelf)
                {
                    DisplayNextLine();
                }
            }
        }
    }

    private void bottomButtonsAddListener()
    {
        autoButton.onClick.AddListener(OnAutoButtonClick);
        skipButton.onClick.AddListener(OnSkipButtonClick);
        saveButton.onClick.AddListener(OnSaveButtonClick);
        loadButton.onClick.AddListener(OnLoadButtonClick);

        homeButton.onClick.AddListener(OnHomeButtonClick);
        closeButton.onClick.AddListener(OnCloseButtonClick);
    
    }

    private void InitializeAndLoadStory(string defaultStoryFileName)
    {
        Initialize();
        loadStoryFromFile(defaultStoryFileName);
        DisplayNextLine();
    }

    private void Initialize()
    {
        currentLine= Constants.DEFAULT_START_LINE;
        avatorImage.gameObject.SetActive(false);
        backgroundImage.gameObject.SetActive(false);
        foreach (var img in characterImageArr)
        {
            img.gameObject.SetActive(false);
        }
        choicePanel.SetActive(false);
    }

    private void loadStoryFromFile(string fileName)
    {
        currentStoryFileName = fileName;
        string path = Constants.STORY_PATH + fileName + Constants.DEFAULT_FILE_EXTENSION;
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

    private void DisplayNextLine()
    {
        if(currentLine > maxReachedLineIndex)
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
            return;
        }
        
        if(currentLine >= storyData.Count)
        {
            Debug.Log(Constants.END_OF_STORY);
            return;
        }

        if(typeWriterEffect.IsTyping())
        {
            typeWriterEffect.CompleteLine();
        }
        else{
            displayThisLine();
        }
    }

    private void showChociePanel()
    {
        var data = storyData[currentLine];
        choiceButton1.onClick.RemoveAllListeners();
        choiceButton2.onClick.RemoveAllListeners();
        choicePanel.SetActive(true);
        choiceButton1.GetComponentInChildren<TextMeshProUGUI>().text = data.speakingContent;
        choiceButton1.onClick.AddListener(() => InitializeAndLoadStory(data.avatorImageFileName));
        choiceButton2.GetComponentInChildren<TextMeshProUGUI>().text = data.vocalAudioFileName;
        choiceButton2.onClick.AddListener(() => InitializeAndLoadStory(data.bgImageFileName));
    }

    private void displayThisLine()
    {
        var data = storyData[currentLine];
        speakerName.text = data.speakerName;
        speakingContent.text = data.speakingContent;
        typeWriterEffect.StartTyping(data.speakingContent);
        
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

    #region  更新角色立绘，角色头像，背景图片，控制角色立绘动画
    private void UpdateAvatarImage(string imageFileName)
    {
        string avatarImagepath = Constants.AVATAR_PATH + imageFileName;
        UpdateImage(avatarImagepath,avatorImage);
    }

    private void UpdateBackgroundImage(string bgImageFileName)
    {
        string bgImagePath = Constants.BACKGROUND_PATH + bgImageFileName;
        UpdateImage(bgImagePath,backgroundImage);
    }
    
    private void UpdateCharacterImage( string Action, string imageFileName, Image characterImage)
    {
        

        if(Action.StartsWith(Constants.CHARACTERACTION_APPEARAT))
        {
            float imgPositionX = CalImgPositionX(Action,
                Constants.DEFAULT_APPEARAT_START_POSITION,
                Constants.DEFAULT_APPEARAT_IRRELEVANT_CHAR);

            if (NotLegalFloatNum(imgPositionX))
            {
                string imagePath = Constants.CHARACTER_PATH + imageFileName;
                UpdateImage(imagePath, characterImage);
                Vector2 newPosition = new Vector2(imgPositionX,
                    characterImage.rectTransform.anchoredPosition.y);
                characterImage.rectTransform.anchoredPosition = newPosition;
                characterImage.DOFade(1, Constants.DEFAULT_DURATION_TIME).From(0);
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
            float imgPositionX = CalImgPositionX(Action,
                Constants.DEFAULT_MOVETO_START_POSITION,
                Constants.DEFAULT_MOVETO_IRRELEVANT_CHAR);
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
    private float CalImgPositionX(string Action,int numStartPosition, int IrrelevantChar)
    {
        ReadOnlySpan<char> span = Action.AsSpan();
        ReadOnlySpan<char> coordinatesSpan = span.Slice(numStartPosition,span.Length - IrrelevantChar);
        var coordinates = coordinatesSpan.ToString().Split(',');

        float _x = -1;
        float imgPositionX = float.TryParse(coordinates[0], out _x) ? _x : -1;
        //float y = float.Parse(coordinates[1]);
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

    #region  音频音乐播放
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

    #region   辅助函数
    private bool NotNullOrEmpty(string str) => !string.IsNullOrEmpty(str);

    private bool NotLegalFloatNum(float num) => num != Constants.DEFAULT_UNEXiST_NUMBER;
    #endregion


    #region 右下底部按钮
    
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


    private bool IsHittingBottomButtons()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(
            bottomButtonsPanel.GetComponent<RectTransform>(),
            Input.mousePosition,
            null
        );
    }

    private void UpdateButtonImage(string imgFileName, Button autoButton)
    {
        string imagePath = Constants.BUTTON_PATH + imgFileName;
        UpdateImage(imagePath,autoButton.image);
    }
    
    #region 自动播放
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



    #region 快速跳过
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
        typeWriterEffect.TYPINGSPEED = Constants.DEFAULT_SKIP_MODE_TYPING_SPEED;
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
        typeWriterEffect.TYPINGSPEED = Constants.DEFAULT_TYPING_SPEED;
        UpdateButtonImage(Constants.AUTO_OFF,skipButton);
    }
    #endregion



    #region 保存和加载
    private void OnSaveButtonClick()
    {
        SaveLoadManager.Instance.ShowSaveLoadUI(true);
    }

    private void OnLoadButtonClick()
    {
        SaveLoadManager.Instance.ShowSaveLoadUI(false);
    }

     private void OnHomeButtonClick()
    {
        gamePanel.SetActive(false);
        MenuManager.Instance.menuPanel.SetActive(true);
    }

    private void OnCloseButtonClick()
    {
        CloseGameUI();
    }


    #endregion



    #endregion
}
