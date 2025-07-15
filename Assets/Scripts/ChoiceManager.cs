using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ChoiceManager : SingletonMonoBase<ChoiceManager>
{
    public GameObject choicePanel;
    public Button choiceButtonPrefab;
    public Transform choiceButtonContainer;
    
    private void Start()
    {
        choicePanel.SetActive(false);
    }

    public void showChoices(List<ChoiceOption> options, Action<string> onChoiceSelected)
    {
        var available = options.Where(
            opt => opt.conditions.All(
                c =>
                {
                    int affection = CharacterStateManager.Instance.GetAffection(c.characterID);
                    return affection >= c.minValue && affection <= c.maxValue;
                }
            )
        ).ToList();

        if (available.Count == 0)
        {
            Debug.LogWarning("No available choices");
            return;
        }

        //当满足选项条件的只有一个，直接不展示并选择该选项
        if (available.Count == 1)
        {
            foreach (AffectionChange affectionChange in available[0].changes)
            {
                CharacterStateManager.Instance.ChangeAffection
                    (affectionChange.characterID, affectionChange.delta);
            }
            onChoiceSelected?.Invoke(available[0].nextStoryFileName);
            return;
        }


        foreach (Transform child in choiceButtonContainer)
        {
            Destroy(child.gameObject);
        }

        foreach(ChoiceOption opt in available)
        {
            Button choiceButton = Instantiate(choiceButtonPrefab, choiceButtonContainer);
            choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = opt.text;
            
            choiceButton.onClick.RemoveAllListeners();
            choiceButton.onClick.AddListener(() =>
            {
                //先改好感度
                foreach (AffectionChange _affectionChange in opt.changes)
                {
                    CharacterStateManager.Instance.ChangeAffection
                        (_affectionChange.characterID, _affectionChange.delta);
                }
                
                onChoiceSelected?.Invoke(opt.nextStoryFileName);
                choicePanel.SetActive(false);
            });
        }

        choicePanel.SetActive(true);
    }
}
