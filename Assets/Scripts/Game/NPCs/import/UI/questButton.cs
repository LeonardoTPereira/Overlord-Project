using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class questButton : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] int questType;
    [SerializeField] GameObject source;

    private menuController menu;
    private Color color;
    public Button myButton;

    // Obtain button and set variables
    private void Start()
    {
        Button btn = myButton.GetComponent<Button>();
        color = btn.GetComponent<Image>().color;
        menu = source.GetComponent<menuController>();

        btn.onClick.AddListener(fireFunctions);
    }

    public void fireFunctions()
    {
        if(img != null)
        {
            setColor();
        }
        setQuest();

        return;
    }
    
    
    // When clicked, set color
    public void setColor()
    {
        img.color = color;

        return;
    }

    public void setQuest()
    {
        menu.setQuest(questType);

        return;
    }
}
