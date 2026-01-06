using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveNPC : MonoBehaviour
{
    [SerializeField] private DialogueTrigger _dialogueTrigger;
    private DialogueManager _dialogueManager;
    private bool _hasTriggered = false;

    private void Start()
    {
        _dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {     
        if (collision.CompareTag("PlayerProjectile") && !_hasTriggered)
        {
            _hasTriggered = true;
            _dialogueTrigger.TriggerDialogue(this);
        }
        else if (collision.CompareTag("PlayerProjectile") && _hasTriggered)
        {
            _dialogueManager.DisplayNextSentence();
        }
    }

    public void ResetTrigger()
    {
        _hasTriggered = false;
    }
}
