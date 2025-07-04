using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : SingletonMonoBase<AudioManager>
{
    public AudioMixer audioMixer;
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup voiceGroup;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource voiceSource;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.outputAudioMixerGroup = musicGroup;
        musicSource.loop = true;

        voiceSource = gameObject.AddComponent<AudioSource>();
        voiceSource.outputAudioMixerGroup = voiceGroup;
        voiceSource.loop = false;

        LoadVolumeSettings();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == Constants.MENU_SCENE)
        {
            PlayBackground(Constants.MENU_MUSIC_FILE_NAME);
        }
        else if (scene.name == Constants.GAME_SCENE)
        {
            string lastMusic = GameManager.Instance.currentBGMusic;
            if (!string.IsNullOrEmpty(lastMusic))
                PlayBackground(lastMusic);
        }
        else if (scene.name == Constants.CREDITS_SCENE)
        {
            PlayBackground(Constants.CREDITS_MUSIC_FILE_NAME);
        }
    }

    public void PlayBackground(string musicFileName)
    {
        AudioClip clip = Resources.Load<AudioClip>(Constants.MUSIC_PATH + musicFileName);
        if (clip == null)
        {
            Debug.Log(Constants.AUDIO_LOAD_FAILED + musicFileName);
            return;
        }
        if (musicSource.clip == clip)
            return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlayVoice(string musicFileName)
    {
        AudioClip clip = Resources.Load<AudioClip>(Constants.VOCAL_PATH + musicFileName);
        if (clip == null)
        {
            Debug.Log(Constants.AUDIO_LOAD_FAILED + musicFileName);
            return;
        }
        voiceSource.clip = clip;
        voiceSource.Play();

    }

    private void LoadVolumeSettings()
    {
        float _masterVolume = PlayerPrefs.GetFloat(Constants.MASTER_VOLUME, Constants.DEFAULT_VOLUME);
        float _musicVolume = PlayerPrefs.GetFloat(Constants.MUSIC_VOLUME, Constants.DEFAULT_VOLUME);
        float _voiceVolume = PlayerPrefs.GetFloat(Constants.VOICE_VOLUME, Constants.DEFAULT_VOLUME);

        audioMixer.SetFloat(Constants.MASTER_VOLUME, SliderValueToDecibel(_masterVolume));
        audioMixer.SetFloat(Constants.MUSIC_VOLUME, SliderValueToDecibel(_musicVolume));
        audioMixer.SetFloat(Constants.VOICE_VOLUME, SliderValueToDecibel(_voiceVolume));
    }

    //将Slider 数值（0~1) 转换成分贝值（对数曲线），当数值为0时设为 -80dB
    private float SliderValueToDecibel(float value)
    {
        //计算分贝公式 以10为底取对数 在乘以20 
        return value > 0.0001f ? Mathf.Log10(value) * 20f : -80f;
    }
}
