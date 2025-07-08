using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    public Button clickButton;
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI countdownText;

    private int targetClicks = 10; // 目标点击次数
    private float remainingTime = 5f;
    private bool isGameActive;

    private void Start()
    {
        clickButton.onClick.AddListener(OnButtonClick);
        isGameActive = true;

        buttonText.text = $"点击按钮({targetClicks})";
        countdownText.text = $"剩余时间: {remainingTime:F1}秒";
    }

    private void Update()
    {
        if (isGameActive)
        {
            remainingTime -= Time.deltaTime;
            countdownText.text = $"剩余时间: {remainingTime:F1}秒";

            if (remainingTime <= 0)
            {
                EndGame();
            }
        }
    }

    private void OnButtonClick()
    {
        if (isGameActive)
        {
            targetClicks--;
            buttonText.text = $"点击按钮({targetClicks})";
            if (targetClicks <= 0)
            {
                EndGame();
            }
        }
    }

    private void EndGame()
    {
        isGameActive = false;
        if (targetClicks <= 0) //win
        {
            GameManager.Instance.pendingData.savedStoryFileName = GameManager.Instance.WinStoryFileName;
        }
        else
        {
            GameManager.Instance.pendingData.savedStoryFileName = GameManager.Instance.LoseStoryFileName;
        }
        SceneManager.LoadScene(Constants.GAME_SCENE);
    }
}
