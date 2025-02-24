using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class CSVReader : MonoBehaviour
{
    void Start()
    {
        
        // ��ȡ�ļ�·����StreamingAssets ʾ����
        string filePath = Path.Combine(Application.streamingAssetsPath, "Dialogue.csv");
        
        // ��ȡ CSV ����
        List<Dictionary<string, string>> data = ParseCSV(filePath);
        
        // ʾ�������ʵ�һ������
        if (data.Count > 0)
        {   
            
            Debug.Log("�Ի���" + data[0]["character"]);
            Debug.Log("���ݣ�" + data[0]["text"]);
        }
    }

    List<Dictionary<string, string>> ParseCSV(string filePath)
    {
        List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
        
        // ����ͬƽ̨��·������
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
        // ȥ�� UTF-8 BOM ͷ������У�
        content = content.Trim('\uFEFF');
    
        // �����
        string[] lines = content.Split('\n');
        
        if (lines.Length < 2) return;

        // ������ͷ
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