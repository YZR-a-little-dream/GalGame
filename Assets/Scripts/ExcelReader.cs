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
        public string bgImageFileName;          //±³¾°Í¼Æ¬
        public string bgMusicFileName;          //±³¾°ÒôÀÖ

        public int characterNum;                //½ÇÉ«±àºÅ
        public string characterAction;          //½ÇÉ«Î»ÖÃ
        public string characterImgFileName;     //½ÇÉ«Á¢»æ
    }

    public static List<ExcelData> ReadExcel(string filepath)
    {
        List<ExcelData> excelData = new List<ExcelData>();
        //¼æÈÝÐÔ¿¼ÂÇ
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using(FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.Read))
        {
            using(var reader = ExcelReaderFactory.CreateReader(stream))
            {
                do
                {
                    while(reader.Read())
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
                        int.TryParse(reader.GetValue(6)?.ToString() , out num) ? num : Constants.DEFAULT_UNEXiST_NUMBER;
                        data.characterAction = reader.IsDBNull(7) ? string.Empty : reader.GetValue(7)?.ToString();
                        data.characterImgFileName = reader.IsDBNull(8) ? string.Empty : reader.GetValue(8)?.ToString();
                        excelData.Add(data);
                    }
                }while(reader.NextResult());
            }
        }
        return excelData;
    }
}
