using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputManager : SingletonMonoBase<InputManager>
{
    public TextMeshProUGUI promptText;
    public TMP_InputField nameInputField;
    public Button confirmButton;

    void Start()
    {
        promptText.text = LM.GLV(Constants.PROMPT_TEXT);
        nameInputField.text = "";
        confirmButton.GetComponentInChildren<TextMeshProUGUI>().text = LM.GLV(Constants.CONFIRM);
        confirmButton.onClick.AddListener(OnConfirm);
    }

    private void OnConfirm()
    {
        string playerName = nameInputField.text.Trim();
        if(IsInvalidName(playerName))
        {
            //TODO: 可以添加错误提示（比如对话框来提示玩家）
            Debug.Log("非法字符");
            return;
        }
        GameManager.Instance.playerName = playerName;

        SceneManager.LoadScene(Constants.GAME_SCENE);
    }

    private bool IsInvalidName(string name)
    {
        return string.IsNullOrEmpty(name);
    }

    // public void showInputPanel()
    // {
    //     promptText.text = Constants.PROMPT_TEXT;
    //     nameInputField.text = "";
    //     inputPanel.SetActive(true);
    // }
}
