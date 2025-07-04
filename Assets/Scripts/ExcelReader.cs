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

    }

    public static List<ExcelData> ReadExcel(string filepath)
    {
        List<ExcelData> excelData = new List<ExcelData>();
        //兼容性考虑
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using (FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                do
                {
                    while (reader.Read())
                    {
                        ExcelData data = new ExcelData();
                        data.speakerName = GetCellString(reader, 0);
                        data.speakingContent = GetCellString(reader, 1);
                        data.avatorImageFileName = GetCellString(reader, 2);
                        data.vocalAudioFileName = GetCellString(reader, 3);
                        data.bgImageFileName = GetCellString(reader, 4);
                        data.bgMusicFileName = GetCellString(reader, 5);
                        // data.characterNum = reader.IsDBNull(6) ?
                        // 0 :
                        // int.TryParse(reader.GetValue(6)?.ToString(), out num) ? num : Constants.DEFAULT_UNEXiST_NUMBER;
                        data.characterNum = GetCellInt(reader, 6);
                        data.characterAction = GetCellString(reader, 7);
                        data.characterImgFileName = GetCellString(reader, 8);

                        excelData.Add(data);
                    }
                } while (reader.NextResult());
            }
        }
        return excelData;
    }

    private static string GetCellString(IExcelDataReader reader, int index)
    {
        return reader.IsDBNull(index) ? string.Empty : reader.GetValue(index)?.ToString();
    }

    private static int GetCellInt(IExcelDataReader reader, int index)
    {
        int num = 0;
        return reader.IsDBNull(index) ? Constants.DEFAULT_UNEXiST_NUMBER :
        int.TryParse(reader.GetValue(index)?.ToString(), out num) ? num : Constants.DEFAULT_UNEXiST_NUMBER;
    }
}

