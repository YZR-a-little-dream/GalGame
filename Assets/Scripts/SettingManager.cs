using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingManager : SingletonMonoBase<SettingManager>
{
    public Toggle fullscreenToggle;
    public Text toggleLabel;
    public TMP_Dropdown resolutionDropdown;


    private Resolution[] availableResolutions;
    private Resolution defaultResolution;
    public Button defaultButton;
    public Button closeButton;

    private void Start() {
        InitializeResolution();
        fullscreenToggle.isOn = Screen.fullScreenMode == FullScreenMode.FullScreenWindow;
        UpdateToggleLabel(fullscreenToggle.isOn);

        defaultButton.GetComponentInChildren<TextMeshProUGUI>().text = LM.GLV(Constants.RESET);
        closeButton.GetComponentInChildren<TextMeshProUGUI>().text = LM.GLV(Constants.CLOSE);
        UpdateToggleLabel(fullscreenToggle.isOn);

        fullscreenToggle.onValueChanged.AddListener(SetDisplayMode);
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        closeButton.onClick.AddListener(CloseSetting);
        defaultButton.onClick.AddListener(ResetSetting);
    }

    private void InitializeResolution()
    {
        availableResolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        
        Dictionary<String,Resolution>  resolutionMap = new Dictionary<String,Resolution>();
        int currentResolutionIndex = 0;

        foreach (var res in availableResolutions)
        {
            //筛选出符合16 ： 9 的分辨率
            const float aspectRation = 16f/9f;
            const float epsilon = 0.01f;

            if(Mathf.Abs((float)res.width / res.height - aspectRation) > epsilon)
                continue;
            
            string option = res.width + "x" + res.height;
            if(!resolutionMap.ContainsKey(option))
            {
                resolutionMap[option] = res;
                resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(option));

                if(res.width == Screen.currentResolution.width && res.height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = resolutionDropdown.options.Count - 1;
                    defaultResolution = res;
                }
            }
        }

        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void SetDisplayMode(bool isFullscreen)
    {
        Screen.fullScreenMode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        UpdateToggleLabel(isFullscreen);
    }

    private void SetResolution(int index)
    {
        string[] dimensions = resolutionDropdown.options[index].text.Split("x");
        int.TryParse(dimensions[0],out int width);
        int.TryParse(dimensions[1],out int height);
        Screen.SetResolution(width,height,Screen.fullScreenMode);
    }

    private void CloseSetting()
    {
        string sceneName = GameManager.Instance.currentScene;
        if (sceneName == Constants.GAME_SCENE)
        {
            GameManager.Instance.historyRecords.RemoveLast();
        }
        SceneManager.LoadScene(sceneName);
    }
    
    private void SaveSettings()
    {
        // PlayerPrefs.SetInt(Constants.RESOLUTION,resolutionDropdown.value);
        // PlayerPrefs.SetInt(Constants.FULLSCREEN,fullscreenToggle.isOn? 1 : 0);
        // PlayerPrefs.Save();
    }

    private void ResetSetting()
    {
        resolutionDropdown.value = resolutionDropdown.options.FindIndex(
            option => option.text == $"{defaultResolution.width}x{defaultResolution.height}"
        );
        fullscreenToggle.isOn = true;
    }

    private void UpdateToggleLabel(bool isFullscreen) =>
        toggleLabel.text = isFullscreen ? Constants.FULLSCREEN : Constants.WINDIW ;
    
}
