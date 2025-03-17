using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadManager : SingletonMonoBase<SaveLoadManager>
{
    public GameObject saveLoadPanel;
    public TextMeshProUGUI panelTitle;
    public Button[] saveLoadButtons;
    public Button prevPageButton;
    public Button nextPageButton;
    public Button backButton;

    private bool isSave;

    private int currentPageIndex = Constants.DEFAULT_START_INDEX;

    private void Start()
    {
        prevPageButton.onClick.AddListener(OnPrevPageButtonClick);
        nextPageButton.onClick.AddListener(OnNextPageButtonClick);
        backButton.onClick.AddListener(OnBackButtonClick);
        saveLoadPanel.SetActive(false);
    }

    public void ShowSaveLoadUI(bool isSave)
    {
        this.isSave = isSave;
        panelTitle.text = isSave ? Constants.SAVE_GAME : Constants.LOAD_GAME;
        UpdateSaveLoadUI();
        saveLoadPanel.SetActive(true);
        LoadstorylineAndScreenshots();
    }

    private void UpdateSaveLoadUI()
    {
        for (int i = 0; i < Constants.SLOTS_PER_PAGE; i++)
        {
            int slotIndex = i + currentPageIndex * Constants.SLOTS_PER_PAGE;
            if(slotIndex < Constants.TOTAL_SLOTS)
            {
                saveLoadButtons[i].gameObject.SetActive(true);
                saveLoadButtons[i].interactable = true;

                //显示文本和图片
                var slotText = (slotIndex + 1) + Constants.COLON + Constants.Empty_SLOT;
                var textComponents = saveLoadButtons[i].GetComponentsInChildren<TextMeshProUGUI>();
                textComponents[0].text = null;
                textComponents[1].text = slotText;
                saveLoadButtons[i].GetComponentInChildren<RawImage>().texture = null;
            }
            else
            {
                saveLoadButtons[i].gameObject.SetActive(false);
            }
        }    
    }

    private void OnPrevPageButtonClick()
    {
        currentPageIndex = 
        (currentPageIndex - 1 + Constants.TOTAL_SLOTS / Constants.SLOTS_PER_PAGE) % (Constants.TOTAL_SLOTS / Constants.SLOTS_PER_PAGE);
        UpdateSaveLoadUI();
        LoadstorylineAndScreenshots();
    }

    private void OnNextPageButtonClick()
    {
        currentPageIndex = (currentPageIndex + 1) % (Constants.TOTAL_SLOTS / Constants.SLOTS_PER_PAGE);
        UpdateSaveLoadUI();
        LoadstorylineAndScreenshots();
    }

    private void OnBackButtonClick()
    {
        saveLoadPanel.SetActive(false);
    }

    private void LoadstorylineAndScreenshots()
    {
        //TODO: load storyline and screenshots
    }
}
