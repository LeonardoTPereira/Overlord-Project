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
        [SerializeField] private NpcSo npc;
        private bool _isDialogueNull;
        private Queue<int> _assignedQuestsQueue;
        public int QuestId
        {
            get =>  _assignedQuestsQueue.Dequeue();
            set => _assignedQuestsQueue.Enqueue(value);
        }

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
            QuestController.QuestOpenedEventHandler += CreateQuestOpenedDialogue;
            QuestController.QuestOpenedEventHandler += CheckIfNpcIsTarget;
        }

        private void OnDisable()
        {
            QuestLine.QuestCompletedEventHandler -= CreateQuestCompletedDialogue;
            QuestController.QuestOpenedEventHandler -= CreateQuestOpenedDialogue;
            QuestController.QuestOpenedEventHandler -= CheckIfNpcIsTarget;
        }

        private void CheckIfNpcIsTarget(object sender, NewQuestEventArgs eventArgs)
        {
            NpcSo questNpc = null;
            if (eventArgs.Quest is not ImmersionQuestSo immersionQuestSo) return;
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
                QuestId = immersionQuestSo.Id;
            }
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
        }
        
        private void CreateQuestOpenedDialogue(object sender, NewQuestEventArgs eventArgs)
        {
            if (eventArgs.NpcInCharge != npc) return;
            var openedQuest = eventArgs.Quest;
            var openerLine = NpcDialogueGenerator.CreateQuestOpener(openedQuest, npc);
            var questId = openedQuest.Id;
            dialogue.AddDialogue(npc, openerLine, true, questId);
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
            var newFolder = npc.NpcName;
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
            dialogue.AddDialogue(npc, greetingDialogue, true, 0);
        }

        private bool HasAtLeastOneTrigger()
        {
            return GetComponents<Collider2D>().Any(col => col.isTrigger);
        }

        public void OnInteractAttempt()
        {
            if (_isDialogueNull) return;
            if (_assignedQuestsQueue.Count > 0)
            {
                ((IQuestElement)this).OnQuestTaskResolved(this, new QuestTalkEventArgs(npc, QuestId));
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
        
        public NpcSo Npc
        {
            get => npc;
            set => npc = value;
        }

    }
}