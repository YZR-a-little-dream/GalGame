using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class LocalizationManager : SingletonMonoBase<LocalizationManager>
{
    /// <summary>
    /// string: 语言 string: 对应语言的文件
    /// </summary>
    public Dictionary<string, string> localizedText;
    public string currentLanguage = Constants.DEFAULT_LANGUAGE;

    public Action LanguageChanged;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        LoadLanguage(currentLanguage);
    }

    public void LoadLanguage(string language)
    {
        currentLanguage = language;
        //Json文件路径
        string filePath = Path.Combine(Application.streamingAssetsPath,
                                        Constants.LANGUAGE_PATH,
                                        language + Constants.JSON_FILE_EXTENSION
        );
        //检查文件是否存在
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            localizedText = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataAsJson);

            LanguageChanged?.Invoke();
        }
        else
        {
            Debug.LogError($"Localization file not found: {filePath}");
        }

    }

    public string GetLocalizedValue(string key)
    {
        if (localizedText != null && localizedText.ContainsKey(key))
        {
            return localizedText[key];
        }
        return key;
    }

    
}


//  @2  29集7:00  静态函数封装单例？？？
public static class LM
{
    public static string GLV(string curlanguageIndex)
    {
        return LocalizationManager.Instance.GetLocalizedValue(curlanguageIndex);
    }

    public static string GetSpeakerName(ExcelData data)
    {
        string currentSpeakName = string.Empty;
        switch (GameManager.Instance.currentLanguageIndex)
        {
            case 0:
                currentSpeakName = ReplaceName(data.speakerName);
                break;
            case 1:
                currentSpeakName = ReplaceName(data.englishName);
                break;
            case 2:
                currentSpeakName = ReplaceName(data.japaneseName);
                break;
            default:
                Debug.Log(GameManager.Instance.currentLanguageIndex);
                break;
        }
        return currentSpeakName;
    }

    public static string GetSpeakingContent(ExcelData data)
    {
        string currentSpeekingContent = string.Empty;
        switch (GameManager.Instance.currentLanguageIndex)
        {
            case 0:
                currentSpeekingContent = ReplaceName(data.speakingContent);
                break;
            case 1:
                currentSpeekingContent = ReplaceName(data.englishContent);
                break;
            case 2:
                currentSpeekingContent = ReplaceName(data.japaneseContent);
                break;
            default:
                Debug.Log(GameManager.Instance.currentLanguageIndex);
                break;
        }
        return currentSpeekingContent;
    }

    public static string ReplaceName(string Content)
    {
        //string类里提供好的
        return Content.Replace(Constants.NAME_PLACEHOLDER, GameManager.Instance.playerName);
    }
}