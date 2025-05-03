using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : SingletonMonoBase<InputManager>
{
    public GameObject inputPanel;
    public TextMeshProUGUI promptText;
    public TMP_InputField nameInputField;
    public Button confirmButton;

    void Start()
    {
        confirmButton.GetComponentInChildren<TextMeshProUGUI>().text = Constants.CONFIRM;
        confirmButton.onClick.AddListener(OnConfirm);
        inputPanel.SetActive(false);
    }

    private void OnConfirm()
    {
        string playerName = nameInputField.text.Trim();
        if(IsInvalidName(playerName))
        {
            Debug.Log("非法字符");
            return;
        }
        PlayerData.Instance.playerName = playerName;
        inputPanel.SetActive(false);
        MenuManager.Instance.StartGame();
    }

    private bool IsInvalidName(string name)
    {
        return string.IsNullOrEmpty(name);
    }

    public void showInputPanel()
    {
        promptText.text = Constants.PROMPT_TEXT;
        nameInputField.text = "";
        inputPanel.SetActive(true);
    }
}
