using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CharacterStateManager : MonoBehaviour
{
    //所有角色数组
    public CharacterData[] allCharacterDatas;
    private Dictionary<string, CharacterState> states;           //角色ID和角色好感度

    public static CharacterStateManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        states = new Dictionary<string, CharacterState>();
        foreach (var CharacterData in allCharacterDatas)
        {
            if (!states.ContainsKey(CharacterData.CharacterID))
            {
                states[CharacterData.CharacterID] = new CharacterState(CharacterData.CharacterID);
            }
        }
    }

    /// <summary>
    /// 根据ID查好感度
    /// </summary>
    /// <param name="characterID">角色ID</param>
    /// <returns></returns>
    public int GetAffection(string characterID)
    {
        return states.TryGetValue
            (characterID, out var _characterState) ?
            _characterState.affection : Constants.DEFAULT_UNEXiST_NUMBER;
    }

    public void ChangeAffection(string characterID, int delta)
    {
        if (!states.TryGetValue(characterID, out var characterState))
        {
            Debug.Log($"无效角色 ID:{characterID}");
        }
        characterState.affection += delta;
    }

    public void ResetAffectin()
    {
        foreach (var _state in states)
        {
            _state.Value.affection = 0;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="map">角色ID 好感</param>
    public void LoadStates(Dictionary<string, int> map)
    {
        foreach (var item in map)
        {
            if (states.ContainsKey(item.Key))
            {
                states[item.Key].affection = item.Value;
            }
        }
    }

    public Dictionary<string, int> Dumpstates()
    {
        var _map = new Dictionary<string, int>();
        foreach (var item in states)
        {
            _map[item.Key] = item.Value.affection;
        }
        return _map;
    }
}
