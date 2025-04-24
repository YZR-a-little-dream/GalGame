using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
     private void Start() {
        testHandler(test1);
    }

    public void testHandler(Action<string> a)
    {
        a("1");
        
    }

    private void test1(string a)
    {
        print("I'm test");
    }    
}
