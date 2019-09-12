using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FormQuestionBHV : MonoBehaviour {

    public int value;
    public Toggle[] toggles;
    public Text questionText;
    public Text descriptionText;

    private FormQuestionData questionData;



    void Awake()
    {
        toggles = GetComponentsInChildren<Toggle>().ToArray<Toggle>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeValue(Toggle selected)
    {
        if (!selected.isOn)
        {
            //Debug.Log("IsOff:"+ int.Parse(selected.GetComponentInChildren<Text>().text));
            value = 0;
            return;
        }
        if (selected.isOn)
        {
            foreach (Toggle t in toggles)
            {
                if (t != selected)
                {
                    //Debug.Log("NotSelected:"+ int.Parse(t.GetComponentInChildren<Text>().text));
                    t.isOn = false;
                    //Debug.Log("After Falsing");
                }
            }
            value = int.Parse(selected.GetComponentInChildren<Text>().text);
        }
    }

    public void LoadData(FormQuestionData q)
    {
        questionData = q;
        questionText.text = q.question;
        descriptionText.text = q.description;
    }

}
