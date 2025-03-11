using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tessttt : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(testCor());
    }

    private IEnumerator testCor()
    {
        for (int i = 0; i < 10; i++)
        {
            Debug.Log(i);
            yield return new WaitForSeconds(10f);
        }
        Debug.Log("testCor end");
    }
}
