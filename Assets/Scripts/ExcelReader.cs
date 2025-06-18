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
        public string speakingContent;
        public string avatorImageFileName;
        public string vocalAudioFileName;
        public string bgImageFileName;          //背景图片
        public string bgMusicFileName;          //背景音乐

        public int characterNum;                //角色编号
        public string characterAction;          //角色位置
        public string characterImgFileName;     //角色立绘

        public string lastBgImg;
        public string lastBgMusic;
        public string englishName;
        public string englishContent;
        public string japaneseName;
        public string japaneseContent;

    }

    public static List<ExcelData> ReadExcel(string filepath)
    {
        List<ExcelData> excelData = new List<ExcelData>();
        //兼容性考虑
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using(FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.Read))
        {
            using(var reader = ExcelReaderFactory.CreateReader(stream))
            {
                do
                {
                    while (reader.Read())
                    {
                        int num = 0;
                        ExcelData data = new ExcelData();
                        data.speakerName = reader.IsDBNull(0) ? string.Empty : reader.GetValue(0)?.ToString();
                        data.speakingContent = reader.IsDBNull(1) ? string.Empty : reader.GetValue(1)?.ToString();
                        data.avatorImageFileName = reader.IsDBNull(2) ? string.Empty : reader.GetValue(2)?.ToString();
                        data.vocalAudioFileName = reader.IsDBNull(3) ? string.Empty : reader.GetValue(3)?.ToString();
                        data.bgImageFileName = reader.IsDBNull(4) ? string.Empty : reader.GetValue(4)?.ToString();
                        data.bgMusicFileName = reader.IsDBNull(5) ? string.Empty : reader.GetValue(5)?.ToString();
                        data.characterNum = reader.IsDBNull(6) ?
                        0 :
                        int.TryParse(reader.GetValue(6)?.ToString(), out num) ? num : Constants.DEFAULT_UNEXiST_NUMBER;
                        data.characterAction = reader.IsDBNull(7) ? string.Empty : reader.GetValue(7)?.ToString();
                        data.characterImgFileName = reader.IsDBNull(8) ? string.Empty : reader.GetValue(8)?.ToString();
                        data.lastBgImg = reader.IsDBNull(9) ? string.Empty : reader.GetValue(9)?.ToString();
                        data.lastBgMusic = reader.IsDBNull(10) ? string.Empty : reader.GetValue(10)?.ToString();
                        excelData.Add(data);            
                    }
                }while(reader.NextResult());
            }
        }
        return excelData;
    }
}
