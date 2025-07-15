using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class GameManager : SingletonDontDestory<GameManager>
{
    //FIXME:默认玩家姓名
    public string playerName = "我是玩家";
    public string currentScene;
    public string currentStoryFile;
    public int currentLineIndex;
    public int currentLanguageIndex = Constants.DEFAULT_LANGUAGE_INDEX;
    public string currentLanguage = Constants.DEFAULT_LANGUAGE;
    public string currentBGImg;
    public string currentBGMusic;

    //立绘是否显示 立绘图片 立绘位置  29期 04:38
    /// <summary>
    /// string:立绘名字  string:立绘坐标
    /// </summary>
    //public Dictionary<string, string> curCharacterName_ActionDic;

    //当前立绘数据
    public List<characterSaveData> currentCharacterData = new List<characterSaveData>(); 

    public string WinStoryFileName;             //胜利结局的故事文件名
    public string LoseStoryFileName;            //失败结局的故事文件名
    public bool hasStarted;                     //判断菜单界面的继续按钮是否可用

    //保存已解锁的背景
    public HashSet<string> unlockedBGHashSets = new HashSet<string>();

    //保存历史记录   每次切换语言时，将会从本行开始显示
    public LinkedList<string> historyRecords = new LinkedList<string>();

    //全局存储每个文件的最远行索引 string:fileName, int:maxReachedLineIndex
    public Dictionary<string, int> MaxReachedLineIndicesDict = new Dictionary<string, int>();


    public enum SaveLoadMode { None, Save, Load }
    public SaveLoadMode currentSaveLoadMode{ get; set; } = SaveLoadMode.None;

    public saveData pendingData;                    //待处理数据

    public void Save(int slotIndex)
    {
        string path = GenetateDataPath(slotIndex);
        File.WriteAllText(path, JsonConvert.SerializeObject(pendingData, Formatting.Indented));
    }

    public void Load(int slotIndex)
    {
        string path = GenetateDataPath(slotIndex);
        pendingData = JsonConvert.DeserializeObject<saveData>(File.ReadAllText(path));
    }

    public string GenetateDataPath(int slotIndex)
        => Path.Combine(Application.persistentDataPath, Constants.SAVE_FILE_PATH, slotIndex + Constants.SAVE_FILE_EXTENSION);
}
