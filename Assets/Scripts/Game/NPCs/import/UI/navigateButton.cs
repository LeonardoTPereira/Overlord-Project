using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class navigateButton : MonoBehaviour
{
    [SerializeField] GameObject origin;
    [SerializeField] GameObject destiny;
    public Button myButton;

    private void Start()
    {
        Button btn = myButton.GetComponent<Button>();

        btn.onClick.AddListener(navigate);
    }

    private void navigate()
    {
        origin.SetActive(false);
        destiny.SetActive(true);
    }
}
