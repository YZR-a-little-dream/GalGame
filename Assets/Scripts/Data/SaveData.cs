using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class characterSaveData
{
    public string characterID;          //角色ID
    public float positionX;            //角色位置X
    public string characterEmotion;     //角色表情
}

public class saveData
{
    public string savedStoryFileName;        //当前保存的故事文件名
    public int savedLine;                   //当前保存的行索引
    public byte[] savedScreenshotData;      //截图数据
    public LinkedList<string> savedHistoryRecords;              //历史存档
    public string savedBGImg;
    public string savedBGMusic;
    //立绘名字 立绘坐标
    public List<characterSaveData> savedCharacters;
    public string savedPlayerName;

    //角色ID，好感度
    public Dictionary<string, int> savedAffections;
}