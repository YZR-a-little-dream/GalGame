using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterManager : SingletonMonoBase<CharacterManager>
{
    public RectTransform container;             //角色立绘容器
    public CharacterDisplay displayPrefab;      //角色立绘预制体
    public CharacterData[] allCharacters;       //角色数据列表

    private List<CharacterDisplay> pool = new List<CharacterDisplay>();     //对象池

    /// <summary>
    /// 角色所有激活立绘显示。Key 为角色ID（string），Value 为对应的 CharacterDisplay 预制体实例。
    /// </summary>
    private Dictionary<string, CharacterDisplay> activeDisplays = new Dictionary<string, CharacterDisplay>(); //角色立绘字典


    public void showCharacter(string characterID, Vector2? position = null, string emotionName = null)
    {
        var data = allCharacters.FirstOrDefault(character => character.CharacterID == characterID);
        if (data == null)
        {
            Debug.Log($"Character with ID {characterID} not found.");
            return;
        }

        Sprite spriteToUse = data.CharacterSprite;
        if (emotionName != null && data.emotionsDic.ContainsKey(emotionName))
        {
            spriteToUse = data.emotionsDic[emotionName];
        }
        //更新立绘的表情
        if (activeDisplays.TryGetValue(characterID, out CharacterDisplay display))
        {
            display.Setup(spriteToUse, position ?? data.defaultPosition, data.defaultScale);
            return;
        }

        CharacterDisplay availableCharacterSprite = pool.FirstOrDefault(_d => !_d.gameObject.activeSelf);
        //如果对象池中没有可用的立绘
        if (availableCharacterSprite == null)
        {
            availableCharacterSprite = Instantiate(displayPrefab, container);
            pool.Add(availableCharacterSprite);
        }

        availableCharacterSprite.Setup(spriteToUse, position ?? data.defaultPosition, data.defaultScale);
        activeDisplays[characterID] = availableCharacterSprite;
    }

    public void HideCharacter(string characterID)
    {
        if (activeDisplays.TryGetValue(characterID, out CharacterDisplay display))
        {
            display.gameObject.SetActive(false);
            activeDisplays.Remove(characterID);
        }
    }

    public void ClrarAll()
    {
        foreach (KeyValuePair<string, CharacterDisplay> _activeDisplay in activeDisplays)
        {
            _activeDisplay.Value.gameObject.SetActive(false);
        }
        activeDisplays.Clear();
    }
}
