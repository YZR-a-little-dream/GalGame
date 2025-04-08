using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HistoryManager : SingletonMonoBase<HistoryManager>
{
    public Transform historyContent;                // ScrollView的Content对象
    public GameObject historyItemPrefab;            // 用于显示历史记录的文本预制件
    public GameObject historyScrollView;            // ScrollView对象
    public Button closeButton;                      // 关闭按钮

    private LinkedList<string> historyRecords;      // 保存的历史记录

    private void Start() {
        historyScrollView.SetActive(false);
        closeButton.onClick.AddListener(CloseHistory);
    }

    public void ShowHistory(LinkedList<string> records)
    {
        //清空现有历史记录
        foreach (Transform child in historyContent)
        {
            Destroy(child.gameObject);
        }
        //显示新的历史记录
        historyRecords = records;
        LinkedListNode<string> currentNode = historyRecords.Last;
        while (currentNode!= null)
        {
            AddHistoryItem(currentNode.Value);
            currentNode = currentNode.Previous;
        }
        historyContent.GetComponent<RectTransform>().localPosition = Vector3.zero;
        historyScrollView.SetActive(true);
    }
    
    private void AddHistoryItem(string text)
    {
        GameObject historyItem = Instantiate(historyItemPrefab, historyContent);
        historyItem.GetComponentInChildren<TextMeshProUGUI>().text = text;
        historyItem.transform.SetAsFirstSibling();          // 将新创建的历史记录显示在最上方
    }

    private void CloseHistory() =>historyScrollView.SetActive(false);
}
