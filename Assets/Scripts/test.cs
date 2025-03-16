using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public static test instance;

    int a = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Task());
    }

    // Update is called once per frame
    void Update()
    {
        print("update");
    }

    public IEnumerator Task()
    {
        print("1");
        yield return new WaitForEndOfFrame();
        print("2");
    }
}
