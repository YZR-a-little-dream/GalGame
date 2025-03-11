using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypeWriteEffect : MonoBehaviour
{
    public float TYPINGSPEED = Constants.DEFAULT_TYPING_SPEED;

    public TextMeshProUGUI textDisplay;

    private Coroutine typingCoroutine;

    private bool isTyping;

    public void StartTyping(string text)
    {
        if (typingCoroutine!= null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(typeLineCor(text));
    }

    private IEnumerator typeLineCor(string text)  
    {
        isTyping = true;
        textDisplay.text = text;
        textDisplay.maxVisibleCharacters = 0;
        for (int i  = 0; i  <= text.Length; i ++)
        {
            textDisplay.maxVisibleCharacters = i;
            
            yield return new WaitForSeconds(TYPINGSPEED);
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