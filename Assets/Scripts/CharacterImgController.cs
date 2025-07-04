using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterImgController : MonoBehaviour
{
    [SerializeField] private Image[] characterImgs;

    //立绘名字 立绘位置


    private Image[] GetCharacterImgs()
    {
        if (this != null)
        {
            characterImgs = GetComponentsInChildren<Image>();
            if (characterImgs.Count() != 0)
                return characterImgs;
            else
                return Array.Empty<Image>();
        }
        else
        {
            return Array.Empty<Image>();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public void GetCharcterImgsPositionDic()
    {
        characterImgs = GetCharacterImgs();

        if (characterImgs != null)
        {
            foreach (var img in characterImgs)
            {
                if (img.gameObject.activeSelf)
                {
                    string APPEARAT_ImgLastImgPos = Constants.CHARACTERACTION_APPEARAT + img.rectTransform.anchoredPosition.ToString();
                    GameManager.Instance.curCharacterName_ActionDic[img.mainTexture.name] = APPEARAT_ImgLastImgPos;
                }
            }

        }

    }

    public int GetChracterImgIndexByName(Image[] characterImgs, string ImgName)
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

    public void GetDic(int num)
    {
        foreach (var img in GameManager.Instance.curCharacterName_ActionDic)
        {
            Debug.Log($"{num}{num}{num}" + img.Key + "  " + img.Value);
        }
    }
}
