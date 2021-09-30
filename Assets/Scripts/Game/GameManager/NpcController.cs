using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private NpcDialogueSO dialogues;
    [SerializeField] private GameObject canvas, npcName, dialogue;
    [SerializeField] private NpcSO npcSo;
    
    public static event EventHandler DialogueOpenEventHandler;
    public static event EventHandler DialogueCloseEventHandler;

    // Start is called before the first frame update
    public void Start()
    {
        dialogue.GetComponent<TextMeshProUGUI>().text = dialogues.Dialogues[UnityEngine.Random.Range(0, 3)];
        npcName.GetComponent<TextMeshProUGUI>().text = npcSo.NpcName;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            canvas.SetActive(true);
            DialogueOpenEventHandler?.Invoke(null, EventArgs.Empty);
        }
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            canvas.SetActive(false);
            DialogueCloseEventHandler?.Invoke(null, EventArgs.Empty);
        }
    }

}
