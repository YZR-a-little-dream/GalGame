using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 实现打字效果
/// </summary>
public class TypeWriteEffect : MonoBehaviour
{
    public float WAITINGSECONDS = Constants.DEFAULT_WAITING_SECONDS;

    public TextMeshProUGUI textDisplay;

    private Coroutine typingCoroutine;

    private bool isTyping;

    public void StartTyping(string text)
    {
        if (typingCoroutine!= null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeLine(text));
    }

    private IEnumerator TypeLine(string text)  
    {
        isTyping = true;
        textDisplay.text = text;
        textDisplay.maxVisibleCharacters = 0;
        for (int i  = 0; i  <= text.Length; i ++)
        {
            textDisplay.maxVisibleCharacters = i;
            yield return new WaitForSeconds(WAITINGSECONDS);
        }

        isTyping = false;
    } 

    public void CompleteLine()
    {
        if(typingCoroutine!= null)
        {
            StopCoroutine(typingCoroutine);
        }

        textDisplay.maxVisibleCharacters = textDisplay.text.Length;
        isTyping = false;
    }

    public bool IsTyping() => isTyping;
}