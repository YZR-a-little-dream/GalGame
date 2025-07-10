using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[CreateAssetMenu(menuName = "vn/Character Data", fileName = "CharacterSpriteData", order = 1)]
public class CharacterData : ScriptableObject
{
    public string CharacterID;                  //人物编号
    public Sprite CharacterSprite;              //人物立绘
    public Vector2 defaultPosition;             //人物立绘默认位置
    public Vector2 defaultScale;                //人物立绘默认缩放

    [System.Serializable]
    public class CharacterEmotion
    {
        public string emotionName;              //表情名称
        public Sprite emotionSprite;            //表情图片
    }

    //表情列表
    public List<CharacterEmotion> emotionsList = new List<CharacterEmotion>();
    public Dictionary<string, Sprite> emotionsDic;

    private void OnEnable()
    {
        emotionsDic = new Dictionary<string, Sprite>();
        foreach (CharacterEmotion emotion in emotionsList)
        {
            if(!emotionsDic.ContainsKey(emotion.emotionName))
            {
                emotionsDic.Add(emotion.emotionName, emotion.emotionSprite);
            }
            else
            {
                Debug.LogWarning($"Duplicate emotion name: {emotion.emotionName} in {CharacterID}");
            }
        }
    } 
}
