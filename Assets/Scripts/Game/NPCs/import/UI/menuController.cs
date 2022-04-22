using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class menuController : MonoBehaviour
{
    private int questType, iterationTimes;

    [SerializeField] GameObject source;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] GameObject inputField;
    private GeradorNPCs gerador;
    public Button myButton;

    private void Start()
    {
        Button btn = myButton.GetComponent<Button>();
        gerador = source.GetComponent<GeradorNPCs>();

        btn.onClick.AddListener(fireFunctions);
    }

    public void fireFunctions()
    {
        int amount = 200;

        if(inputField != null)
        {
            amount = getInput();
        }

        setAmount(amount);

        gerador.Generate(iterationTimes, questType);
    }

    public void load(bool value)
    {
        loadingScreen.SetActive(value);
    }

    public void setQuest(int type)
    {
        questType = type;

        return;
    }

    public void setAmount(int amount)
    {
        iterationTimes = amount;

        return;
    }

    public int getInput()
    {
        string text = inputField.GetComponent<TMP_InputField>().text;
        int value;

        bool success = int.TryParse(text, out value);

        if (success)
        {
            if (value > 10000)
            {
                value = 10000;
            } else if (value < 10)
            {
                value = 10;
            }

            return value;
        }
        else
        {
            return 10;
        }
    }

}
