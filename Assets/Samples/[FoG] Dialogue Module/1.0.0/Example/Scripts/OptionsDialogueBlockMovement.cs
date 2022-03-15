using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fog.Dialogue.Samples {
    [CreateAssetMenu(fileName = "NewOptionsDialogue", menuName = "FoG/DialogueModule/Sample/OptionsDialogueSample")]
    public class OptionsDialogueBlockMovement : OptionsDialogue {
        public override void BeforeDialogue(){
            SimpleMove.instance.BlockMovement();

            base.BeforeDialogue();
            DialogueHandler.instance.OnDialogueStart -= BeforeDialogue;
        }
    }
}
