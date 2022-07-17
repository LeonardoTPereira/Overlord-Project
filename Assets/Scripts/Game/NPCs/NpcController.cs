using System.Linq;
using Fog.Dialogue;
using Game.Dialogues;
using Game.Quests;
using MyBox;
using UnityEditor;
using UnityEngine;
using Util;

namespace Game.NPCs
{
    public class NpcController : MonoBehaviour, IInteractable, IQuestElement {
        [SerializeField] private DialogueController dialogue;
        [SerializeField] private NpcSo npc;
        private bool _isDialogueNull;
        
        private void Start()
        {
            CreateIntroDialogue();
        }

        private void OnEnable()
        {
            QuestController.QuestCompletedEventHandler += CreateQuestCompletedDialogue;
            QuestController.QuestOpenedEventHandler += CreateQuestOpenedDialogue;
        }

        private void OnDisable()
        {
            QuestController.QuestCompletedEventHandler -= CreateQuestCompletedDialogue;
            QuestController.QuestOpenedEventHandler -= CreateQuestOpenedDialogue;
        }

        private void CreateQuestCompletedDialogue(object sender, NewQuestEventArgs eventArgs)
        {
            if (eventArgs.NpcInCharge != npc) return;
            var completedQuest = eventArgs.Quest;
            dialogue.AddDialogue(npc, NpcDialogueGenerator.CreateQuestCloser(completedQuest, npc));
            if (!completedQuest.EndsStoryLine)
            {
                dialogue.AddDialogue(npc, NpcDialogueGenerator.CreateQuestOpener(completedQuest.Next, npc));

            }
        }
        
        private void CreateQuestOpenedDialogue(object sender, NewQuestEventArgs eventArgs)
        {
            if (eventArgs.NpcInCharge != npc) return;
            var openedQuest = eventArgs.Quest;
            dialogue.AddDialogue(npc, NpcDialogueGenerator.CreateQuestOpener(openedQuest, npc));
        }

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
        
        private void CreateIntroDialogue()
        {
            dialogue = ScriptableObject.CreateInstance<DialogueController>();
            _isDialogueNull = dialogue == null;
            dialogue.AddDialogue(npc, NpcDialogueGenerator.CreateGreeting(Npc));
        }

        private bool HasAtLeastOneTrigger()
        {
            return GetComponents<Collider2D>().Any(col => col.isTrigger);
        }

        public void OnInteractAttempt()
        {
            if (_isDialogueNull) return;
            ((IQuestElement)this).OnQuestTaskResolved(this, new QuestTalkEventArgs(npc));
            DialogueHandler.instance.StartDialogue(dialogue);
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