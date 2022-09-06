using System.Collections.Generic;
using System.Linq;
using Fog.Dialogue;
using Game.Dialogues;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
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
        private bool _isDialogueNull;
        private Queue<int> _assignedQuestsQueue;
        [field: SerializeField] public NpcSo Npc { get; set; }

        public int QuestId { get; set; }
        
        private void Awake()
        {
            _assignedQuestsQueue = new Queue<int>();
        }

        private void Start()
        {
            CreateIntroDialogue();
        }

        private void OnEnable()
        {
            QuestLine.QuestCompletedEventHandler += CreateQuestCompletedDialogue;
            QuestLine.QuestOpenedEventHandler += OpenQuest;
        }

        private void OnDisable()
        {
            QuestLine.QuestCompletedEventHandler -= CreateQuestCompletedDialogue;
            QuestLine.QuestOpenedEventHandler -= OpenQuest;
        }

        private void OpenQuest(object sender, NewQuestEventArgs eventArgs)
        {
            var quest = eventArgs.Quest;
            var npcInCharge = eventArgs.NpcInCharge;
            CheckIfNpcIsTarget(quest);
            CreateQuestOpenedDialogue(quest, npcInCharge);
        }

        private void CheckIfNpcIsTarget(QuestSo quest)
        {
            NpcSo questNpc = null;
            if (quest is not ImmersionQuestSo immersionQuestSo) return;
            switch (immersionQuestSo)
            {
                case ListenQuestSo listenQuestSo:
                    questNpc = listenQuestSo.Npc;
                    break;
                case GiveQuestSo giveQuestSo:
                    questNpc = giveQuestSo.GiveQuestData.NpcToReceive;
                    break;
                case ReportQuestSo reportQuestSo:
                    questNpc = reportQuestSo.Npc;
                    break;
            }

            if (questNpc != null && questNpc.NpcName == Npc.NpcName)
            {
                _assignedQuestsQueue.Enqueue(immersionQuestSo.Id);
            }
        }

        private void CreateQuestCompletedDialogue(object sender, NewQuestEventArgs eventArgs)
        {
            if (eventArgs.NpcInCharge != Npc) return;
            var questId = eventArgs.Quest.Id;
            dialogue.StopDialogueFromQuest(questId);
            var closerLine = NpcDialogueGenerator.CreateQuestCloser(eventArgs.Quest, Npc);
            dialogue.AddDialogue(Npc, closerLine, false, questId, true);
        }
        
        private void CreateQuestOpenedDialogue(QuestSo quest, NpcSo npcInCharge)
        {
            if (npcInCharge != Npc) return;
            var openerLine = NpcDialogueGenerator.CreateQuestOpener(quest, Npc);
            var questId = quest.Id;
            dialogue.AddDialogue(Npc, openerLine, true, questId);
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
            target += Constants.SeparatorCharacter + "Resources";
            target += Constants.SeparatorCharacter + "Dialogues";
            var newFolder = Npc.NpcName;
            if (!AssetDatabase.IsValidFolder(target + Constants.SeparatorCharacter + newFolder))
            {
                AssetDatabase.CreateFolder(target, newFolder);
            }
            target += Constants.SeparatorCharacter + newFolder;
            var fileName = target + Constants.SeparatorCharacter + "Dialogue.asset";
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
            var greetingDialogue = NpcDialogueGenerator.CreateGreeting(Npc);
            dialogue.AddDialogue(Npc, greetingDialogue, true, 0);
        }

        private bool HasAtLeastOneTrigger()
        {
            return GetComponents<Collider2D>().Any(col => col.isTrigger);
        }

        public void OnInteractAttempt()
        {
            if (_isDialogueNull) return;
            while (_assignedQuestsQueue.Count > 0)
            {
                var questId = _assignedQuestsQueue.Dequeue();
                ((IQuestElement)this).OnQuestTaskResolved(this, new QuestTalkEventArgs(Npc, questId));
            }

            var questsInDialogue = dialogue.GetQuestCloserDialogueIds();
            foreach (var questIds in questsInDialogue)
            {
                ((IQuestElement)this).OnQuestCompleted(this, new QuestElementEventArgs(questIds));
            }
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
        
        
    }
}