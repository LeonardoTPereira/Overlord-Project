using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject DialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    private Queue<string> _sentences;
    private NaveNPC _npc;

    public static DialogueManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    void Start()
    {
        DialoguePanel.SetActive(false);
        _sentences = new Queue<string>();
    }

    public void StartDialogue(NaveNPC naveNPC, Dialogue startDialogue, Dialogue endDialogue)
    {
        DialoguePanel.SetActive(true);
        _npc = naveNPC;
        nameText.text = startDialogue.name;

        _sentences.Clear();
        AddDialogueInSentences(startDialogue);
        if (endDialogue != null) 
            AddDialogueInSentences(endDialogue);
        DisplayNextSentence();
    }

    public void AddDialogueInSentences(Dialogue dialogue)
    {
        foreach (string sentence in dialogue.sentences)
        {
            _sentences.Enqueue(sentence);
        }
    }

    public void DisplayNextSentence()
    {
        if (_sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = _sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void EndDialogue()
    {
        _npc.ResetTrigger();
        DialoguePanel.SetActive(false);
    }
}
