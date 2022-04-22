using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class buttonSpawner : MonoBehaviour
{

    [SerializeField] public GameObject buttonPrefab;
    [SerializeField] public GameObject buttonParent;

    private void spawnButtons(NPC_SO[] population, int count)
    {
        for (int i = 0; i < count; i++)
        {
            TMP_Text text = buttonPrefab.GetComponentInChildren<TMP_Text>();
            text.text = population[i].identity;
            text.fontSize = 17.1f;
            GameObject button = (GameObject)Instantiate(buttonPrefab);
            button.transform.SetParent(buttonParent.transform);

            //button.GetComponent<Button>().onClick.AddListener();
        }
    }
}
