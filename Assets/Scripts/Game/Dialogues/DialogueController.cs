using System;
using Fog.Dialogue;
using UnityEngine;

namespace Game.Dialogues
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/DialogueController")]
    public class DialogueController : Dialogue
    {
        public static event EventHandler DialogueOpenEventHandler;
        public static event EventHandler DialogueCloseEventHandler;
        public override void BeforeDialogue(){
            DialogueOpenEventHandler?.Invoke(null, EventArgs.Empty);
            base.BeforeDialogue();
            DialogueHandler.instance.OnDialogueStart -= BeforeDialogue;
        }

        public override void AfterDialogue(){
            DialogueCloseEventHandler?.Invoke(null, EventArgs.Empty);
            base.AfterDialogue();
            DialogueHandler.instance.OnDialogueStart -= AfterDialogue;
        }
    }
}