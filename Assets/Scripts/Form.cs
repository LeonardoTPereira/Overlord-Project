using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Form : MonoBehaviour {

    public void GoToForm()
    {
        //TODO add correct link
        Time.timeScale = 0f;
        Debug.Log("Open Form in Browser");
        Application.OpenURL("http://unity3d.com/");
    }
    public void Continue()
    {

        Debug.Log("Load New Batch");
        
        GameManager gm = GameManager.instance;
        gm.LoadNewBatch();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
