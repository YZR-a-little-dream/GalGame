using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveLoadManager : SingletonMonoBase<SaveLoadManager>
{

    public TextMeshProUGUI panelTitle;
    public Button[] saveLoadButtons;
    public Button prevPageButton;
    public Button nextPageButton;
    public Button backButton;

    private bool isLoad => GameManager.Instance.currentSaveLoadMode == GameManager.SaveLoadMode.Load;

    private int currentPageIndex = Constants.DEFAULT_START_INDEX;
    private Action<int> currentAction;
    private Action menuAction;

    private void Start()
    {
        prevPageButton.GetComponentInChildren<TextMeshProUGUI>().text = LM.GLV(Constants.PREV_PAGE);
        nextPageButton.GetComponentInChildren<TextMeshProUGUI>().text = LM.GLV(Constants.NEXT_PAGE);
        backButton.GetComponentInChildren<TextMeshProUGUI>().text = LM.GLV(Constants.BACK);


        prevPageButton.onClick.AddListener(OnPrevPageBtnClick);
        nextPageButton.onClick.AddListener(OnNextPageBtnClick);
        backButton.onClick.AddListener(OnBackBtnClick);

        panelTitle.text = isLoad ? LM.GLV(Constants.LOAD_GAME) : LM.GLV(Constants.SAVE_GAME);
        UpdateUI();

    }

    // public void ShowSaveLoadUI(bool isSave)
    // {
    //     this.isSave = isSave;
    //     panelTitle.text = isSave ? Constants.SAVE_GAME : Constants.LOAD_GAME;
    //     UpdateSaveLoadUI();
    //     saveLoadPanel.SetActive(true);
    //     LoadstorylineAndScreenshots();
    // }

    // public void showSavePanel(Action<int> action)
    // {
    //     this.isSave = true;
    //     panelTitle.text = Constants.SAVE_GAME;
    //     currentAction = action;
    //     UpdateSaveLoadUI();
    //     saveLoadPanel.SetActive(true);
    // }

    // public void showLoadPanel(Action<int> action,System.Action menuAction)
    // {
    //     this.isSave = false;
    //     panelTitle.text = Constants.LOAD_GAME;
    //     currentAction = action;
    //     this.menuAction = menuAction;
    //     UpdateSaveLoadUI();
    //     saveLoadPanel.SetActive(true);
    // }

    private void UpdateUI()
    {
        for (int i = 0; i < Constants.SLOTS_PER_PAGE; i++)
        {
            int slotIndex = i + currentPageIndex * Constants.SLOTS_PER_PAGE;
            if (slotIndex < Constants.TOTAL_SLOTS)
            {
                UpdateSaveLoadButtons(saveLoadButtons[i], slotIndex);
                LoadstorylineAndScreenshots(saveLoadButtons[i], slotIndex);
            }
            else
            {
                saveLoadButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void UpdateSaveLoadButtons(Button slotBtn, int slotIndex)
    {
        slotBtn.gameObject.SetActive(true);
        slotBtn.interactable = true;

        string savePath = GenetateDataPath(slotIndex);
        bool fileExists = File.Exists(savePath);

        if (isLoad && !fileExists)       //如果是记载游戏并且存档不存在，存档不可互动
        {
            slotBtn.interactable = false;
        }

        //显示文本和图片
        var textComponents = slotBtn.GetComponentsInChildren<TextMeshProUGUI>();
        textComponents[0].text = null;
        textComponents[1].text = (slotIndex + 1) + Constants.COLON + Constants.Empty_SLOT;
        slotBtn.GetComponentInChildren<RawImage>().texture = null;

        slotBtn.onClick.RemoveAllListeners();
        slotBtn.onClick.AddListener(() => OnButtonClick(slotBtn, slotIndex));
    }

    private void OnButtonClick(Button slotBtn, int slotIndex)
    {
        //TODO: 暂定
    }

    private void OnPrevPageBtnClick()
    {
        currentPageIndex =
        (currentPageIndex - 1 + Constants.TOTAL_SLOTS / Constants.SLOTS_PER_PAGE) % (Constants.TOTAL_SLOTS / Constants.SLOTS_PER_PAGE);
        UpdateUI();

    }

    private void OnNextPageBtnClick()
    {
        currentPageIndex = (currentPageIndex + 1) % (Constants.TOTAL_SLOTS / Constants.SLOTS_PER_PAGE);
        UpdateUI();
    }

    private void OnBackBtnClick()
    {
        string sceneName = GameManager.Instance.currentScene;
        if (sceneName == Constants.GAME_SCENE)
        {
            GameManager.Instance.historyRecords.RemoveLast();
        }
        SceneManager.LoadScene(sceneName);
    }

    private void LoadstorylineAndScreenshots(Button slotBtn, int slotIndex)
    {
        // load storyline and screenshotss
        string savePath = GenetateDataPath(slotIndex);
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            var saveData = JsonConvert.DeserializeObject<GameManager.saveData>(json);
            if (saveData.savedScreenshotData != null)
            {
                Texture2D screenshot = new Texture2D(2, 2);
                screenshot.LoadImage(saveData.savedScreenshotData);
                slotBtn.GetComponentInChildren<RawImage>().texture = screenshot;
            }
            if (saveData.savedHistoryRecords.Last != null)
            {
                //FIXME: save and load
                TextMeshProUGUI[] textComponents = slotBtn.GetComponentsInChildren<TextMeshProUGUI>();
                textComponents[0].text = saveData.currentSpeekingContent;
                textComponents[1].text = File.GetLastWriteTime(savePath).ToString("G");
            }

        }
    }

    private string GenetateDataPath(int slotIndex)
        => Path.Combine(Application.persistentDataPath, Constants.SAVE_FILE_PATH, slotIndex + Constants.SAVE_FILE_EXTENSION);
}
