using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GalleryManager : SingletonMonoBase<GalleryManager>,IPointerClickHandler
{
    public GameObject galleryPanel;
    public TextMeshProUGUI panelTitle;
    public Button[] galleryButtons;
    public Button prevPageButton;
    public Button nextPageButton;
    public Button backButton;

    
    public GameObject bigImagePanel;        //全屏大图面板
    public Image bigImage;                  //显示大图的Image

    private int currentPageIndex = Constants.DEFAULT_START_INDEX;
    private readonly int slotsPerPage = Constants.GALLERY_SLOTS_PER_PAGE;
    private readonly int totalSlots = Constants.ALL_BACKGROUNDS.Length;

    protected void Start() {
        prevPageButton.onClick.AddListener(OnPrevPageButtonClick);
        nextPageButton.onClick.AddListener(OnNextPageButtonClick);
        backButton.onClick.AddListener(OnBackButtonClick);

        galleryPanel.SetActive(false);
        panelTitle.text = Constants.GALLERY;

        bigImagePanel.SetActive(false);

        Button bigImageButton = bigImagePanel.GetComponent<Button>();
        if(bigImageButton != null)
        {
            bigImageButton.onClick.AddListener(HideImage);
        }else{
            Debug.Log("BigImagePanel上没有Button？");
        }
    }

    

    public void ShowGalleryPanel()
    {
        UpdateUI();
        galleryPanel.SetActive(true);
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slotsPerPage; i++)
        {
            int slotIndex = currentPageIndex * slotsPerPage + i;
            if(slotIndex < totalSlots)
            {
                UpdateGalleryButtons(galleryButtons[i],slotIndex);
            }else{
                galleryButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void UpdateGalleryButtons(Button button, int slotIndex)
    {
        button.gameObject.SetActive(true);
        string bgName = Constants.ALL_BACKGROUNDS[slotIndex];
        bool isUnlocked = VNManager.Instance.unlockedBGHashSets.Contains(bgName);

        string imagePath = Constants.THUMBNATL_PATH + (isUnlocked ? bgName :Constants.GALLERY_PACEHOLDER);
        Sprite sprite = Resources.Load<Sprite>(imagePath);
        if(sprite != null)
        {
            button.GetComponent<Image>().sprite = sprite;
        }else{
            Debug.Log(Constants.IMAGE_LOAD_FAILED + imagePath);
        }
        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onButtonClick(button,slotIndex));
    }

    private void onButtonClick(Button button, int slotIndex)
    {
        string bgName = Constants.ALL_BACKGROUNDS[slotIndex];
        bool isUnlocked = VNManager.Instance.unlockedBGHashSets.Contains(bgName); 

        if(!isUnlocked)
            return;
        
        string imagePath = Constants.BACKGROUND_PATH + bgName;
        Sprite sprite = Resources.Load<Sprite>(imagePath);

        if(sprite != null)
        {
            bigImage.sprite = sprite;
            bigImagePanel.SetActive(true);
        }else{
            Debug.Log(Constants.BIG_IMAGE_LOAD_FAILED);
        }
    }

    private void HideImage()
    {
        bigImagePanel.SetActive(false);
    }

    private void OnBackButtonClick()
    {
        galleryPanel.SetActive(false);
    }

    private void OnNextPageButtonClick()
    {
        // if(currentPageIndex > 0 )
        // {
        //     currentPageIndex--;
        //     UpdateUI();
        // }

        currentPageIndex = ((currentPageIndex + 1) % (int)(Math.Ceiling(
            (double)Constants.ALL_BACKGROUNDS.Length / Constants.GALLERY_SLOTS_PER_PAGE)));
        UpdateUI();
    }

    private void OnPrevPageButtonClick()
    {
        if(currentPageIndex <= 0 )
            currentPageIndex = (int)(Math.Ceiling(
            (double)Constants.ALL_BACKGROUNDS.Length / Constants.GALLERY_SLOTS_PER_PAGE)) - 1;
        else 
            currentPageIndex--;
        UpdateUI();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
            galleryPanel.SetActive(false);
    }
}
