using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    public Button slotButton;
    public Button deleteButton;
    public RawImage thumbnailImage;
    public TextMeshProUGUI topText;
    public TextMeshProUGUI bottomText;

    private int slotIndex;              //保存槽位索引
    private SaveLoadManager owner;
    private bool hasFile;               //是否有存档文件

    public void Init(SaveLoadManager owner, int slotIndex)
    {
        this.owner = owner;
        this.slotIndex = slotIndex;

        slotButton.onClick.RemoveAllListeners();
        slotButton.onClick.AddListener(OnSlotClick);

        deleteButton.onClick.RemoveAllListeners();
        deleteButton.onClick.AddListener(OnDeleteClick);
    }

    public void Refresh()
    {
        string path = GameManager.Instance.GenetateDataPath(slotIndex);
        hasFile = File.Exists(path);
        bool isLoad = GameManager.Instance.currentSaveLoadMode == GameManager.SaveLoadMode.Load;

        deleteButton.gameObject.SetActive(hasFile);

        slotButton.interactable = hasFile || !isLoad;

        thumbnailImage.texture = null;

        if (!hasFile)
        {
            topText.text = string.Empty;
            bottomText.text = (slotIndex + 1) + Constants.COLON + LM.GLV(Constants.Empty_SLOT);
            return;
        }

        string json = File.ReadAllText(path);
        saveData data = JsonConvert.DeserializeObject<saveData>(json);

        if (data.savedScreenshotData != null)
        {
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(data.savedScreenshotData);
            thumbnailImage.texture = tex;
        }
        if (data.savedHistoryRecords?.Last != null)
        {
            topText.text = data.savedHistoryRecords.Last.Value;
        }
        bottomText.text = File.GetLastWriteTime(path).ToString("G");
    }

    private void OnSlotClick()
    {
        if (hasFile)
            owner.HandleExistingSlot(slotIndex,this);
        else
            owner.HandleEmptySlot(slotIndex,this);
    }

    private void OnDeleteClick()
    {
        owner.RequestDeleteSlot(slotIndex, this);
    }
}
