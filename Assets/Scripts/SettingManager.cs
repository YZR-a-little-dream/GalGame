using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.PlayerLoop;

public class SettingManager : SingletonMonoBase<SettingManager>
{
    public Toggle fullscreenToggle;
    public Text toggleLabel;
    public TMP_Dropdown resolutionDropdown;


    private Resolution[] availableResolutions;
    private Resolution defaultResolution;
    public Button defaultButton;
    public Button closeButton;

    public Slider masterVolumeSlider, musicVolumeSlider, voiceVolumeSlider;
    public AudioMixer audioMixer;

    private void Start()
    {
        AddListener();
        Initalization();       
    }

    private void Initalization()
    {
        InitializeDisplayMode();
        InitializeResolution();
        InitializeButtons();
        InitializeVolume();
    }

    private void AddListener()
    {
        fullscreenToggle.onValueChanged.AddListener(SetDisplayMode);
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        closeButton.onClick.AddListener(CloseSetting);
        defaultButton.onClick.AddListener(ResetSetting);

        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        voiceVolumeSlider.onValueChanged.AddListener(SetVoiceVolume);
    }

    private void InitializeDisplayMode()
    {
        fullscreenToggle.isOn = Screen.fullScreenMode == FullScreenMode.FullScreenWindow;
        UpdateToggleLabel(fullscreenToggle.isOn);
    }

    private void InitializeResolution()
    {
        availableResolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        Dictionary<String, Resolution> resolutionMap = new Dictionary<String, Resolution>();
        int currentResolutionIndex = 0;

        foreach (var res in availableResolutions)
        {
            //筛选出符合16 ： 9 的分辨率
            const float aspectRation = 16f / 9f;
            const float epsilon = 0.01f;

            if (Mathf.Abs((float)res.width / res.height - aspectRation) > epsilon)
                continue;

            string option = res.width + "x" + res.height;
            if (!resolutionMap.ContainsKey(option))
            {
                resolutionMap[option] = res;
                resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(option));

                if (res.width == Screen.currentResolution.width && res.height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = resolutionDropdown.options.Count - 1;
                    defaultResolution = res;
                }
            }
        }

        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void InitializeButtons()
    {
        defaultButton.GetComponentInChildren<TextMeshProUGUI>().text = LM.GLV(Constants.RESET);
        closeButton.GetComponentInChildren<TextMeshProUGUI>().text = LM.GLV(Constants.CLOSE);
    }

    private void InitializeVolume()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat(Constants.MASTER_VOLUME, Constants.DEFAULT_VOLUME);
        musicVolumeSlider.value = PlayerPrefs.GetFloat(Constants.MUSIC_VOLUME, Constants.DEFAULT_VOLUME);
        voiceVolumeSlider.value = PlayerPrefs.GetFloat(Constants.VOICE_VOLUME, Constants.DEFAULT_VOLUME);

        SetMasterVolume(masterVolumeSlider.value);
        SetMusicVolume(musicVolumeSlider.value);
        SetVoiceVolume(voiceVolumeSlider.value);
    }

    private void SetDisplayMode(bool isFullscreen)
    {
        Screen.fullScreenMode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        UpdateToggleLabel(isFullscreen);
    }

    //将Slider 数值（0~1) 转换成分贝值（对数曲线），当数值为0时设为 -80dB
    private float SliderValueToDecibel(float value)
    {
        //计算分贝公式 以10为底取对数 在乘以20 
        return value > 0.0001f ? Mathf.Log10(value) * 20f : -80f;
    }

    private void SetMasterVolume(float value)
    {
        audioMixer.SetFloat(Constants.MASTER_VOLUME, SliderValueToDecibel(value));
    }

    private void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(Constants.MUSIC_VOLUME, SliderValueToDecibel(value));
    }

    private void SetVoiceVolume(float value)
    {
        audioMixer.SetFloat(Constants.VOICE_VOLUME, SliderValueToDecibel(value));
    }

    private void SetResolution(int index)
    {
        string[] dimensions = resolutionDropdown.options[index].text.Split("x");
        int.TryParse(dimensions[0], out int width);
        int.TryParse(dimensions[1], out int height);
        Screen.SetResolution(width, height, Screen.fullScreenMode);
    }

    private void CloseSetting()
    {
        string sceneName = GameManager.Instance.currentScene;
        if (sceneName == Constants.GAME_SCENE)
        {
            GameManager.Instance.historyRecords.RemoveLast();
        }

        PlayerPrefs.SetFloat(Constants.MASTER_VOLUME, masterVolumeSlider.value);
        PlayerPrefs.SetFloat(Constants.MUSIC_VOLUME, musicVolumeSlider.value);
        PlayerPrefs.SetFloat(Constants.VOICE_VOLUME, voiceVolumeSlider.value);
        PlayerPrefs.Save();

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

        masterVolumeSlider.value = Constants.DEFAULT_VOLUME;
        musicVolumeSlider.value = Constants.DEFAULT_VOLUME;
        voiceVolumeSlider.value = Constants.DEFAULT_VOLUME;

        SetMasterVolume(masterVolumeSlider.value);
        SetMusicVolume(musicVolumeSlider.value);
        SetVoiceVolume(voiceVolumeSlider.value);
    }

    private void UpdateToggleLabel(bool isFullscreen) =>
        toggleLabel.text = isFullscreen ? Constants.FULLSCREEN : Constants.WINDIW ;
    
}
