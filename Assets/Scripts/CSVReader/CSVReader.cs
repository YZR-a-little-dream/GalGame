using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class CSVReader : MonoBehaviour
{
    void Start()
    {
        
        // 获取文件路径（StreamingAssets 示例）
        string filePath = Path.Combine(Application.streamingAssetsPath, "Dialogue.csv");
        
        // 读取 CSV 内容
        List<Dictionary<string, string>> data = ParseCSV(filePath);
        
        // 示例：访问第一行数据
        if (data.Count > 0)
        {   
            
            Debug.Log("对话：" + data[0]["character"]);
            Debug.Log("内容：" + data[0]["text"]);
        }
    }

    List<Dictionary<string, string>> ParseCSV(string filePath)
    {
        List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
        
        // 处理不同平台的路径差异
        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
            www.SendWebRequest();
            while (!www.isDone) { }
            ParseCSVContent(www.downloadHandler.text, result);
        }
        else
        {
            using (StreamReader reader = new StreamReader(filePath,Encoding.UTF8))
            {
                ParseCSVContent(reader.ReadToEnd(), result);
            }
        }
        return result;
    }

    void ParseCSVContent(string content, List<Dictionary<string, string>> data)
    {
        // 去除 UTF-8 BOM 头（如果有）
        content = content.Trim('\uFEFF');
    
        // 拆分行
        string[] lines = content.Split('\n');
        
        if (lines.Length < 2) return;

        // 解析表头
        string[] headers = lines[0].Trim().Split(',');

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Trim().Split(',');
            
            if (values.Length != headers.Length) continue;

            Dictionary<string, string> entry = new Dictionary<string, string>();
            for (int j = 0; j < headers.Length; j++)
            {
                entry[headers[j]] = values[j];
            }
            data.Add(entry);
        }
    }
}