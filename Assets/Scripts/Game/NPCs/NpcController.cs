using System.Linq;
using Fog.Dialogue;
using Game.Dialogues;
using Game.Quests;
using UnityEngine;

#if UNITY_EDITOR
using MyBox;
using UnityEditor;
using Util;
#endif

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
            var questId = completedQuest.Id;
            dialogue.StopDialogueFromQuest(questId);
            var closerLine = NpcDialogueGenerator.CreateQuestCloser(completedQuest, npc);
            dialogue.AddDialogue(npc, closerLine, false, questId);
            if (completedQuest.EndsStoryLine) return;
            var nextQuestId = completedQuest.Next.Id;
            var newOpenerLine = NpcDialogueGenerator.CreateQuestOpener(completedQuest.Next, npc);
            dialogue.AddDialogue(npc, newOpenerLine, true, nextQuestId);
            Debug.Log("Opened new quest: " + nextQuestId);
        }
        
        private void CreateQuestOpenedDialogue(object sender, NewQuestEventArgs eventArgs)
        {
            if (eventArgs.NpcInCharge != npc) return;
            var openedQuest = eventArgs.Quest;
            var openerLine = NpcDialogueGenerator.CreateQuestOpener(openedQuest, npc);
            var questId = openedQuest.Id;
            dialogue.AddDialogue(npc, openerLine, true, questId);
            Debug.Log("Opened quest: " + questId);
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
        
        public void CreateIntroDialogue()
        {
            dialogue = ScriptableObject.CreateInstance<DialogueController>();
            _isDialogueNull = dialogue == null;
            var greetingDialogue = NpcDialogueGenerator.CreateGreeting(Npc);
            dialogue.AddDialogue(npc, greetingDialogue, true, 0);
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