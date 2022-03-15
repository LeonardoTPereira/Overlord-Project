using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fog.Dialogue.Samples {
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "FoG/DialogueModule/Sample/DialogueSample")]
    public class DialogueBlockMovement : Dialogue {
        public override void BeforeDialogue(){
            SimpleMove.instance.BlockMovement();

            base.BeforeDialogue();
            DialogueHandler.instance.OnDialogueStart -= BeforeDialogue;
        }

        public override void AfterDialogue(){
            SimpleMove.instance.AllowMovement();

            base.AfterDialogue();
            DialogueHandler.instance.OnDialogueStart -= AfterDialogue;
        }
    }
}
