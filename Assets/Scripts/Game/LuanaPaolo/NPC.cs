using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    Color[] colors = { Color.red, Color.green, Color.blue };
    string[] dialogues = { "Thank you for saving me!", "May the Godesses bless your help!", "Finally!" };
    [SerializeField]
    protected GameObject canvas, dialogue;

    public static event EventHandler DialogueOpenEventHandler;
    public static event EventHandler DialogueCloseEventHandler;

    // Start is called before the first frame update
    public void Start()
    {
        GetComponent<SpriteRenderer>().color = colors[UnityEngine.Random.Range(0, colors.Length)];
        dialogue.GetComponent<TextMeshProUGUI>().text = dialogues[UnityEngine.Random.Range(0, 3)];
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
