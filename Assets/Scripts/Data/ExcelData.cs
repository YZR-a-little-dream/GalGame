using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AffectionChange
{
    public string characterID;
    public int delta;
}

public class AffectionCondition
{
    public string characterID;
    public int minValue;
    public int maxValue;
}

public class ChoiceOption
{
    public string text;
    public string nextStoryFileName;
    public List<AffectionChange> changes;
    public List<AffectionCondition> conditions;
}


// 角色指令类
// 用于存储角色的动作、位置和表情等信息
public class CharacterCommand
{
    public string characterID;          //角色ID
    public string characterAction;      //角色动作
    public float positionX;             //角色位置
    public string characterEmotion;     //角色表情
}

public class ExcelData
    {
        public string speakerName;
        public string speakingContent;
        public string avatorImageFileName;
        public string vocalAudioFileName;
        public string bgImageFileName;          //背景图片
        public string bgMusicFileName;          //背景音乐

        public List<CharacterCommand> characterCommands = new(); //角色指令列表

        public string englishName;
        public string englishContent;
        public string japaneseName;
        public string japaneseContent;

    }