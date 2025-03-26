using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterImgController : MonoBehaviour
{
    private Image[] GetCharacterImgs()
    {
        Image[] CharacterImgs = GetComponentsInChildren<Image>();
        return CharacterImgs;
    }

    public Dictionary<string,string> GetCharcterImgsPositionDic()
    {
        Image[] characterImgs = GetCharacterImgs();
        Dictionary<string,string> characterImgDicts = new Dictionary<string, string>();
        foreach (var img in characterImgs)
        {
            if (img.gameObject.activeSelf)
            {
                string APPEARAT_ImgLastImgPos = Constants.CHARACTERACTION_APPEARAT + (img.rectTransform.anchoredPosition).ToString();
                characterImgDicts.Add(img.mainTexture.name, APPEARAT_ImgLastImgPos);
            }
        }

        return characterImgDicts;
    }

    public int GetChracterImgIndexByName(Image[] characterImgs,string ImgName)
    {
        //实现根据名字获取图片的功能
        for (int i = 0; i < characterImgs.Length; i++)
        {
            if (characterImgs[i].mainTexture.name == ImgName)
            {
                return i;
            }
        }
            return Constants.DEFAULT_UNEXiST_NUMBER;
    }
}
