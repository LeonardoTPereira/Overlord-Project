using System.Linq;
using System.Text;
using Fog.Dialogue;
using Game.Dialogues;
using MyBox;
using UnityEditor;
using UnityEngine;
using Util;

namespace Game.NPCs
{
    public class NpcController : MonoBehaviour, IInteractable {
        [SerializeField] private DialogueController dialogue;
        [SerializeField] private NpcSo npc;

        public void Reset() {
            var nColliders = GetComponents<Collider2D>().Length;
            if (nColliders == 1) {
                GetComponent<Collider2D>().isTrigger = true;
            } else if (nColliders > 0) {
                var hasTrigger = HasAtLeastOneTrigger();
                if (!hasTrigger) {
                    GetComponent<Collider2D>().isTrigger = true;
                }
            }
        }

#if UNITY_EDITOR
        [ButtonMethod]
        public void CreateDialogueAsset()
        {
            dialogue = ScriptableObject.CreateInstance<DialogueController>();
            CreateIntroDialogue();
            var target = "Assets";
            target += Constants.SEPARATOR_CHARACTER + "Resources";
            target += Constants.SEPARATOR_CHARACTER + "Dialogues";
            var newFolder = npc.NpcName;
            if (!AssetDatabase.IsValidFolder(target + Constants.SEPARATOR_CHARACTER + newFolder))
            {
                AssetDatabase.CreateFolder(target, newFolder);
            }
            target += Constants.SEPARATOR_CHARACTER + newFolder;
            var fileName = target + Constants.SEPARATOR_CHARACTER + "Dialogue.asset";
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(fileName);
            Debug.Log(uniquePath);
            AssetDatabase.CreateAsset(dialogue, uniquePath);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
            AssetDatabase.Refresh();
        }
#endif
        
        public void CreateIntroDialogue()
        {
            var dialogueLine = new StringBuilder();
            dialogueLine.Append(NpcDialogueGenerator.CreateGreeting(Npc));
            dialogue.AddDialogue(npc, dialogueLine.ToString());
        }

        private bool HasAtLeastOneTrigger()
        {
            return GetComponents<Collider2D>().Any(col => col.isTrigger);
        }

        public void OnInteractAttempt() {
            if (dialogue != null) {
                DialogueHandler.instance.StartDialogue(dialogue);
            }
        }

        public void OnTriggerEnter2D(Collider2D col) {
            var agent = col.GetComponent<Agent>();
            if (agent) {
                agent.collidingInteractables.Add(this);
            }
        }

        public void OnTriggerExit2D(Collider2D col) {
            var agent = col.GetComponent<Agent>();
            if (agent) {
                agent.collidingInteractables.Remove(this);
            }
        }
        
        public NpcSo Npc
        {
            get => npc;
            set => npc = value;
        }
    }
}