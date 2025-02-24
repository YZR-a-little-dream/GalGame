using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VNManager : MonoBehaviour
{
    // 文本文件路径
    private string FILEPATH = Constants.STORY_PATH;

    public TextMeshProUGUI speakerName;
    public TextMeshProUGUI speakingContent;

    public TypeWriteEffect typeWriterEffect;

       
    private List<ExcelReader.ExcelData> storyData;

    private int currentLine = 0;

    void Start()
    {
        LoadStoryFromFile(FILEPATH);        
        DisplayNextLine();                  
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            DisplayNextLine();
        }
    }

    private void LoadStoryFromFile(string filepath)
    {
        storyData = ExcelReader.ReadExcel(filepath);
        
        if(storyData == null || storyData.Count == 0)
        {
            Debug.LogError("Story data is null or empty.");
        }
    }
        

    private void DisplayNextLine()
    {
        if(currentLine >= storyData.Count)
        {
            Debug.Log("End of story.");
            return;
        }

        if(typeWriterEffect.IsTyping())
        {
            typeWriterEffect.CompleteLine();
        }
        else
        {
            var data = storyData[currentLine];
            speakerName.text = data.speaker;
            typeWriterEffect.StartTyping(data.content);
            speakingContent.text = data.content;
            currentLine++;
        }
    }
}
