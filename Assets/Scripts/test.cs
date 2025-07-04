using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class test : MonoBehaviour
{
    private void Start()
    {
        Image[] images = GetComponentsInChildren<Image>(true);
        foreach (var img in images)
        {
            Debug.Log(img);
        }
     }  
}
