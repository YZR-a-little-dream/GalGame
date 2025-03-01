using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ExcelDataReader;
using UnityEngine;

public class ExcelReader : MonoBehaviour
{
    public struct ExcelData
    {
        public string speakerName;
        public string SpeakingContent;
        public string avatorImageFileName;
        public string vocalAudioFileName;
        public string bgImageFileName;  //±≥æ∞Õº∆¨
        public string bgMusicFileName;  //±≥æ∞“Ù¿÷
    }

    public static List<ExcelData> ReadExcel(string filepath)
    {
        List<ExcelData> excelData = new List<ExcelData>();
        //ºÊ»›–‘øº¬«
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using(FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.Read))
        {
            using(var reader = ExcelReaderFactory.CreateReader(stream))
            {
                do
                {
                    while(reader.Read())
                    {
                        ExcelData data = new ExcelData();
                        data.speakerName = reader.IsDBNull(0) ? string.Empty : reader.GetValue(0)?.ToString();
                        data.SpeakingContent = reader.IsDBNull(1) ? string.Empty : reader.GetValue(1)?.ToString();
                        data.avatorImageFileName = reader.IsDBNull(2) ? string.Empty : reader.GetValue(2)?.ToString();
                        data.vocalAudioFileName = reader.IsDBNull(3) ? string.Empty : reader.GetValue(3)?.ToString();
                        data.bgImageFileName = reader.IsDBNull(4) ? string.Empty : reader.GetValue(4)?.ToString();
                        data.bgMusicFileName = reader.IsDBNull(5) ? string.Empty : reader.GetValue(5)?.ToString();
                        excelData.Add(data);
                    }
                }while(reader.NextResult());
            }
        }
        return excelData;
    }
}
