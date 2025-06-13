using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class LocalizationManager : SingletonMonoBase<LocalizationManager>
{
    public Dictionary<string, string> localizedText;
    public string currentLanguage = Constants.DEFAULT_LANGUAGE;

    public Action LanguageChanged;

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
