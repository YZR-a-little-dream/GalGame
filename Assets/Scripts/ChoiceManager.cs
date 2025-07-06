using System;
using System.Collections;
using System.Collections.Generic;
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

    public void showChoices(List<string> options, List<string> actions, Action<string> onChoiceSelected)
    {
        foreach (Transform child in choiceButtonContainer)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < options.Count; i++)
        {
            Button choiceButton = Instantiate(choiceButtonPrefab, choiceButtonContainer);
            choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = options[i];
            int index = i;
            choiceButton.onClick.RemoveAllListeners();
            choiceButton.onClick.AddListener(() =>
            {
                onChoiceSelected?.Invoke(actions[index]);
                choicePanel.SetActive(false);
            });
        }
        choicePanel.SetActive(true);
    }
}
