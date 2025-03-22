using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypeWriteEffect : MonoBehaviour
{   
    private float typingSpeed;
    public TextMeshProUGUI textDisplay;

    private Coroutine typingCoroutine;

    private bool isTyping;

    public void StartTyping(string text,float speed)
    {
        typingSpeed = speed;
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
            
            yield return new WaitForSeconds(typingSpeed);
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