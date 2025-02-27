using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VNManager : MonoBehaviour
{
    // 文本文件路径
    private string FILEPATH = Constants.STORY_PATH + Constants.DEFAULT_STORY_FILR_NAME;

    public TextMeshProUGUI speakerName;
    public TextMeshProUGUI speakingContent;

    public TypeWriteEffect typeWriterEffect;

    public Image avatorImage;
    public AudioSource vocalAudio;
      
    private List<ExcelReader.ExcelData> storyData;

    private int currentLine = 0;

    void Start()
    {
        loadStoryFromFile(FILEPATH);        
        displayNextLine();                  
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            displayNextLine();
        }
    }

    private void loadStoryFromFile(string filepath)
    {
        storyData = ExcelReader.ReadExcel(filepath);
        
        if(storyData == null || storyData.Count == 0)
        {
            Debug.LogError(Constants.NO_DATA_FOUND);
        }
    }
        

    private void displayNextLine()
    {
        if(currentLine >= storyData.Count)
        {
            Debug.Log(Constants.END_OF_STORY);
            return;
        }

        if(typeWriterEffect.IsTyping())
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
        speakerName.text = data.speakerName;
        speakingContent.text = data.SpeakingContent;
        typeWriterEffect.StartTyping(data.SpeakingContent);
        
        if(NotNullOrEmpty(data.avatorImageFileName))
        {
            UpdateAvatarImage(data.avatorImageFileName);
        }
        else
        {
            avatorImage.gameObject.SetActive(false);
        }
        
        if(NotNullOrEmpty(data.vocalAudioFileName))
        {
            PlayerVocalAudio(data.vocalAudioFileName);
        }

        currentLine++;
    }

    private void UpdateAvatarImage(string imageFileName)
    {
        string imagepath = Constants.AVATAR_PATH + imageFileName;
        Sprite sprite = Resources.Load<Sprite>(imagepath);
        if(sprite != null)
        {
            avatorImage.sprite = sprite;
            avatorImage.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError(Constants.IMAGE_LOAD_FAILED + imagepath);
        }
    }
    

    private void PlayerVocalAudio(string audioFileName)
    {
        string audioPath = Constants.VOCAL_PATH + audioFileName;
        AudioClip audioClip = Resources.Load<AudioClip>(audioPath);
        if(audioClip != null)
        {
            vocalAudio.clip = audioClip;
            vocalAudio.Play();
        }
        else
        {
            Debug.LogError(Constants.AUDIO_LOAD_FAILED + audioPath);
        }
    }

    private bool NotNullOrEmpty(string str) => !string.IsNullOrEmpty(str);
    
}
