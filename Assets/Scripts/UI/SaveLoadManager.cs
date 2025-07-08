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
    public SaveSlot[] slots;        // 存档槽位数组
    public Button prevPageButton;
    public Button nextPageButton;
    public Button backButton;

    //新子界面提示是否删除或覆盖当前存档
    public GameObject confirmPanel;
    public TextMeshProUGUI confirmText;
    public Button confirmButton;
    public Button cancelButton;

    private bool isLoad => GameManager.Instance.currentSaveLoadMode == GameManager.SaveLoadMode.Load;

    private int currentPageIndex = Constants.DEFAULT_START_INDEX;
    private Action<int> currentAction;
    private Action menuAction;

    private void Start()
    {
        panelTitle.text = isLoad ? LM.GLV(Constants.LOAD_GAME) : LM.GLV(Constants.SAVE_GAME);

        prevPageButton.GetComponentInChildren<TextMeshProUGUI>().text = LM.GLV(Constants.PREV_PAGE);
        nextPageButton.GetComponentInChildren<TextMeshProUGUI>().text = LM.GLV(Constants.NEXT_PAGE);
        backButton.GetComponentInChildren<TextMeshProUGUI>().text = LM.GLV(Constants.BACK);


        prevPageButton.onClick.AddListener(OnPrevPageBtnClick);
        nextPageButton.onClick.AddListener(OnNextPageBtnClick);
        backButton.onClick.AddListener(OnBackBtnClick);

        confirmPanel.SetActive(false);

        RefreshPage();
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

    private void RefreshPage()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            int slotIndex = i + currentPageIndex * Constants.SLOTS_PER_PAGE;
            if (slotIndex < Constants.TOTAL_SLOTS)
            {
                slots[i].gameObject.SetActive(true);
                slots[i].Init(this, slotIndex);
                slots[i].Refresh();
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }

    // private void UpdateSaveLoadButtons(Button slotBtn, int slotIndex)
    // {
    //     slotBtn.gameObject.SetActive(true);
    //     slotBtn.interactable = true;

    //     string savePath = GameManager.Instance.GenetateDataPath(slotIndex);
    //     bool fileExists = File.Exists(savePath);

    //     if (isLoad && !fileExists)       //如果是记载游戏并且存档不存在，存档不可互动
    //     {
    //         slotBtn.interactable = false;
    //     }

    //     //显示文本和图片
    //     var textComponents = slotBtn.GetComponentsInChildren<TextMeshProUGUI>();
    //     textComponents[0].text = null;
    //     textComponents[1].text = (slotIndex + 1) + Constants.COLON + Constants.Empty_SLOT;
    //     slotBtn.GetComponentInChildren<RawImage>().texture = null;

    //     slotBtn.onClick.RemoveAllListeners();
    //     slotBtn.onClick.AddListener(() => OnButtonClick(slotBtn, slotIndex));
    // }

    // private void OnButtonClick(Button slotBtn, int slotIndex)
    // {
    //     if (!isLoad)
    //     {
    //         GameManager.Instance.Save(slotIndex);
    //         LoadstorylineAndScreenshots(slotBtn, slotIndex);
    //     }
    //     else
    //     {
    //         GameManager.Instance.Load(slotIndex);
    //         SceneManager.LoadScene(Constants.GAME_SCENE);
    //     }
    // }

    private void OnPrevPageBtnClick()
    {
        currentPageIndex =
        (currentPageIndex - 1 + Constants.TOTAL_SLOTS / Constants.SLOTS_PER_PAGE) % (Constants.TOTAL_SLOTS / Constants.SLOTS_PER_PAGE);
        RefreshPage();

    }

    private void OnNextPageBtnClick()
    {
        currentPageIndex = (currentPageIndex + 1) % (Constants.TOTAL_SLOTS / Constants.SLOTS_PER_PAGE);
        RefreshPage();
    }

    private void OnBackBtnClick()
    {
        string sceneName = GameManager.Instance.currentScene;
        if (sceneName == Constants.GAME_SCENE)
        {
            GameManager.Instance.historyRecords.RemoveLast();
        }
        GameManager.Instance.pendingData = null;

        SceneManager.LoadScene(sceneName);
    }

    // private void LoadstorylineAndScreenshots(Button slotBtn, int slotIndex)
    // {
    //     // load storyline and screenshotss
    //     string savePath = GameManager.Instance.GenetateDataPath(slotIndex);
    //     if (File.Exists(savePath))
    //     {
    //         string json = File.ReadAllText(savePath);
    //         var saveData = JsonConvert.DeserializeObject<GameManager.saveData>(json);
    //         if (saveData.savedScreenshotData != null)
    //         {
    //             Texture2D screenshot = new Texture2D(2, 2);
    //             screenshot.LoadImage(saveData.savedScreenshotData);
    //             slotBtn.GetComponentInChildren<RawImage>().texture = screenshot;
    //         }
    //         if (saveData.savedHistoryRecords.Last != null)
    //         {
    //             //FIXME: save and load 本地化
    //             TextMeshProUGUI[] textComponents = slotBtn.GetComponentsInChildren<TextMeshProUGUI>();
    //             textComponents[0].text = saveData.savedHistoryRecords.Last.Value;
    //             textComponents[1].text = File.GetLastWriteTime(savePath).ToString("G");
    //         }
    //     }
    //     else
    //     {
    //         //Debug.Log("未找到当前存档路径");
    //     }
    // }

    public void HandleEmptySlot(int slotIndex, SaveSlot saveSlot)
    {
        SaveToSlot(slotIndex, saveSlot);
    }

    public void HandleExistingSlot(int slotIndex, SaveSlot saveSlot)
    {
        if (isLoad)
        {
            GameManager.Instance.Load(slotIndex);
            SceneManager.LoadScene(Constants.GAME_SCENE);
        }
        else
        {
            showConfirmPanel(
                LM.GLV(Constants.CONFIRM_COVER_SAVE_FILE),
                () => { SaveToSlot(slotIndex, saveSlot); }
            );
        }
    }

    public void RequestDeleteSlot(int slotIndex, SaveSlot deleteSlot)
    {
        showConfirmPanel(
            LM.GLV(Constants.CONFIRM_DELETE_SAVE_FILE),
            () => { DeleteToSlot(slotIndex, deleteSlot); }
        );
    }

    private void showConfirmPanel(string message, Action onYes)
    {
        confirmText.text = message;
        confirmPanel.SetActive(true);

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() =>
        {
            confirmPanel.SetActive(false);
            onYes?.Invoke();
        }
        );

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() =>
        {
            confirmPanel.SetActive(false);
        }
        );
    }

    private void SaveToSlot(int slotIndex, SaveSlot saveSlot)
    {
        GameManager.Instance.Save(slotIndex);
        saveSlot.Refresh();
    }

    private void DeleteToSlot(int slotIndex, SaveSlot deleteSlot)
    {
        File.Delete(GameManager.Instance.GenetateDataPath(slotIndex));
        deleteSlot.Refresh();
    }
}
