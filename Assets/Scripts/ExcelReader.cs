using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ExcelDataReader;
using Unity.VisualScripting;
using UnityEngine;

public class ExcelReader : MonoBehaviour
{
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
                    while (reader.Read()) // 读取每一行
                    {
                        if(reader.IsDBNull(0) && reader.IsDBNull(1))
                        continue; // 如果第一列和第二列都为空，则跳过该行

                        var data = new ExcelData
                        {
                            speakerName = GetCellString(reader, 0),
                            speakingContent = GetCellString(reader, 1),
                            avatorImageFileName = GetCellString(reader, 2),
                            vocalAudioFileName = GetCellString(reader, 3),
                            bgImageFileName = GetCellString(reader, 4),
                            bgMusicFileName = GetCellString(reader, 5),
                            // data.characterNum = reader.IsDBNull(6) ?
                            // 0 :
                            // int.TryParse(reader.GetValue(6)?.ToString(), out num) ? num : Constants.DEFAULT_UNEXiST_NUMBER;
                            characterCommands = new List<CharacterCommand>(),
                            englishName = GetCellString(reader, 7),
                            englishContent = GetCellString(reader, 8),
                            japaneseName = GetCellString(reader, 9),
                            japaneseContent = GetCellString(reader,10)
                        };
                        var raw = GetCellString(reader, 6);
                        if (!string.IsNullOrEmpty(raw))
                        {
                            var parts = raw.Split(';', System.StringSplitOptions.RemoveEmptyEntries);
                            foreach (var _command in parts)
                            {
                                var block = _command.Trim();
                                if (string.IsNullOrEmpty(block))
                                {
                                    Debug.Log("Empty command block found, skipping.");
                                    continue; // 跳过空的指令块
                                }
                                var fields = block.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
                                if (fields[0].Trim().StartsWith(Constants.CHARACTERCOMMANDS))
                                {
                                    continue;
                                }
                                if (fields.Length < 2)
                                    {
                                        Debug.Log("Invalid command block format, skipping: " + block);
                                        continue; // 跳过格式不正确的指令块
                                    }
                                var cmd = new CharacterCommand
                                {
                                    characterID = fields[0].Trim(),
                                    characterAction = fields[1].Trim()
                                };
                                if (cmd.characterAction != Constants.CHARACTERACTION_DISAPPEAR)
                                {
                                    if (fields.Length < 4)
                                    {
                                        Debug.Log("Incomplete command block, missing position or emotion: " + block);
                                        continue; // 跳过不完整的指令块
                                    }
                                    var third = fields[2].Trim();
                                    var fourth = fields[3].Trim();

                                    if (float.TryParse(third, out float positionX))
                                    {
                                        cmd.positionX = positionX;
                                        cmd.characterEmotion = string.IsNullOrWhiteSpace(fourth) ? null : fourth;
                                    }
                                    else if (float.TryParse(fourth, out positionX))
                                    {
                                        cmd.characterEmotion = string.IsNullOrWhiteSpace(third) ? null : third;
                                        cmd.positionX = positionX;
                                    }
                                    else
                                    {
                                        cmd.characterEmotion = null;
                                        cmd.positionX = 0f;
                                    }
                                }
                                data.characterCommands.Add(cmd);

                            }
                        }
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

