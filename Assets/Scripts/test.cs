using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable() {
        SaveLoadManager.Instance.ShowSaveLoadUI(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
