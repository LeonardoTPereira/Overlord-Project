using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class listArray : MonoBehaviour
{
    string filename = "Assets/FNPC";
    int count;

    [SerializeField] private NPC_SO[] population = new NPC_SO[0];
    [SerializeField] public GameObject buttonPrefab;
    [SerializeField] public GameObject buttonParent;

    private void Start()
    {
        Button btn = GetComponent<Button>();

        btn.onClick.AddListener(fireFunctions);
    }

    private void fireFunctions()
    {
        destroyButtons();
        loadArray();
        spawnButtons();
    }

#if UNITY_EDITOR
    private void loadArray()
    {
        string[] guids = AssetDatabase.FindAssets("t:NPC_SO", new[] { filename });
        count = guids.Length;

        population = new NPC_SO[count];

        for (int i = 0; i < count; i++)
        {
            var path = AssetDatabase.GUIDToAssetPath(guids[i]);
            population[i] = AssetDatabase.LoadAssetAtPath<NPC_SO>(path);
        }

        return;
    }
#endif

    private void spawnButtons()
    {
        for (int i = 0; i < count; i++)
        {
            TMP_Text text = buttonPrefab.GetComponentInChildren<TMP_Text>();
            text.text = population[i].age.ToString();
            text.fontSize = 100;
            GameObject button = (GameObject)Instantiate(buttonPrefab);
            button.transform.SetParent(buttonParent.transform);

            //button.GetComponent<Button>().onClick.AddListener();
        }
    }

    private void destroyButtons()
    {
        int i = 0;
        GameObject[] allChildren = new GameObject[buttonParent.transform.childCount];
        foreach (Transform child in buttonParent.transform)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }
        foreach (GameObject child in allChildren)
        {
            DestroyImmediate(child.gameObject);
        }

    }
}
