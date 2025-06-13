using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    public string key;

    private TextMeshProUGUI textCompenent;

    private void Start()
    {
        textCompenent = GetComponent<TextMeshProUGUI>();
        LocalizationManager.Instance.LanguageChanged += UpdateText;

        UpdateText();
    }

    private void OnDestroy() {
        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.LanguageChanged -= UpdateText;
        }
    }

    private void UpdateText()
    {
        if (textCompenent != null)
        {
            textCompenent.text = LocalizationManager.Instance.GetLocalizedValue(key);
        }
    }
}
