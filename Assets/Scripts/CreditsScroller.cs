using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScroller : MonoBehaviour
{
    public RectTransform creditsText;

    private void Start()
    {
        loadCreditsFromFile();

        creditsText.anchoredPosition = new Vector2(creditsText.anchoredPosition.x, -Screen.height);
    }

    private void Update()
    {
        float speedMultiplier = Constants.DEFAULT_MULTIPLIER;
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.LeftControl))
            speedMultiplier = Constants.DEFAULT_FAST_MULTIPLIER;

        creditsText.anchoredPosition += Vector2.up * (Screen.height / 10)
                                        * speedMultiplier * Time.deltaTime;

        if (creditsText.anchoredPosition.y >= Constants.CREDITS_SCROLL_END_Y
            || Input.GetMouseButton(1))
        {
            SceneManager.LoadScene(Constants.MENU_SCENE);
        }
        
    }

    private void loadCreditsFromFile()
    {
        string path = Path.Combine(Application.streamingAssetsPath,
                                    Constants.CREDITS_PATH,
                                    GameManager.Instance.currentLanguage
                                    + Constants.CREDITS_FILE_EXTENSION);
        if (File.Exists(path))
        {
            string content = File.ReadAllText(path);
            creditsText.GetComponent<TextMeshProUGUI>().text = content;
        }
        else
        {
            Debug.Log(Constants.CREDITS_LOAD_FILED + path);
        }
    }
}
