using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class VNManager : MonoBehaviour
{
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

    //分支面板
    public GameObject choicePanel;
    public Button choiceButton1;
    public Button choiceButton2;
    

    void Start()
    {
        InitializeAndLoadStory(Constants.DEFAULT_STORY_FILE_NAME);
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            displayNextLine();
        }
    }

    private void InitializeAndLoadStory(string defaultStoryFileName)
    {
        Initialize();
        loadStoryFromFile(defaultStoryFileName);
        displayNextLine();
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
        string path = Constants.STORY_PATH + fileName + Constants.DEFAULT_FILE_EXTENSION;
        storyData = ExcelReader.ReadExcel(path);
        
        if(storyData == null || storyData.Count == 0)
        {
            Debug.LogError(Constants.NO_DATA_FOUND);
        }
    }


    private void displayNextLine()
    {
        if(currentLine == storyData.Count - 1)
        {
            if(storyData[currentLine].speakerName.Equals(Constants.END_OF_STORY))
            {
                Debug.Log(Constants.END_OF_STORY);
                return;
            }
            
            if(storyData[currentLine].speakerName.Equals(Constants.CHOICE))
            {
                showChociePanel();
                return;
            }
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
}
