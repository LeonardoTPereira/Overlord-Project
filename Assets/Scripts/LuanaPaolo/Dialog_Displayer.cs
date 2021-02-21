using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Displayer : MonoBehaviour
{
    public Dialog_Box dialog_box;
    public Text text_controller;

    // Update is called once per frame
    void Update()
    {
        text_controller.text = dialog_box.dialog; 
    }
}
