using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : SingletonMonoBase<GameManager>
{
    //FIXME:玩家姓名
    public string playerName = "我是玩家";
    public string currentScene;
    public string currentStoryFile;
    public int currentLineIndex;
    public int currentLanguageIndex = Constants.DEFAULT_LANGUAGE_INDEX;
    public string currentLanguage = Constants.DEFAULT_LANGUAGE;
    public string currentBGImg;
    public string currentBGMusic;

    //TODO:@1 立绘是否显示 立绘图片 立绘位置  29期 04:38
    /// <summary>
    /// string:立绘名字  string:立绘坐标
    /// </summary>
    public Dictionary<string, string> curCharacterName_ActionDic;

    public bool hasStarted;                     //判断菜单界面的继续按钮是否可用

    //保存已解锁的背景
    public HashSet<string> unlockedBGHashSets = new HashSet<string>();

    //保存历史记录   每次切换语言时，将会从本行开始显示
    public LinkedList<string> historyRecords = new LinkedList<string>();

    //全局存储每个文件的最远行索引 string:fileName, int:maxReachedLineIndex
    public Dictionary<string, int> MaxReachedLineIndicesDict = new Dictionary<string, int>();


    public enum SaveLoadMode { Save, Load }
    public SaveLoadMode currentSaveLoadMode;

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

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
