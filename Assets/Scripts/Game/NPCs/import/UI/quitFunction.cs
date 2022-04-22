using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class quitFunction : MonoBehaviour
{
    //public Button myButton;

    private void Start()
    {
        Button btn = GetComponent<Button>();

        btn.onClick.AddListener(quit);
    }

    private void quit()
    {
        Application.Quit();
    }
}
