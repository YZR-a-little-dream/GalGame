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
        public string speaker;
        public string content;
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
                        ExcelData data = new ExcelData();
                        data.speaker = reader.GetString(0);
                        data.content = reader.GetString(1);
                        excelData.Add(data);
                    }
                }while(reader.NextResult());
            }
        }
        return excelData;
    }
}
