using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue startDialogue;
    public Dialogue endDialogue;

    public void TriggerDialogue(NaveNPC nave)
    {
        FindObjectOfType<DialogueManager>().StartDialogue(nave, startDialogue, endDialogue);
    }
}
