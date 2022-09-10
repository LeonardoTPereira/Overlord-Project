using System.Collections;
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
        private Queue<QuestSo> _assignedQuestsQueue;
        [field: SerializeField] public NpcSo Npc { get; set; }

        public int QuestId { get; set; }
        
        private void Awake()
        {
            _assignedQuestsQueue = new Queue<QuestSo>();
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
            var questNpc = quest switch
            {
                ImmersionQuestSo immersionQuestSo => CheckIfNpcIsTargetOfImmersionQuest(immersionQuestSo),
                AchievementQuestSo achievementQuestSo => CheckIfNpcIsTargetOfAchievementQuest(achievementQuestSo),
                _ => null
            };
            if (questNpc == null) return;
            AddQuestToQueueIfNpcIsTarget(questNpc, quest);
        }

        private static NpcSo CheckIfNpcIsTargetOfAchievementQuest(AchievementQuestSo achievementQuestSo)
        {
            var questNpc = achievementQuestSo switch
            {
                ExchangeQuestSo exchangeQuestSo => exchangeQuestSo.Npc,
                _ => null
            };

            return questNpc;
        }

        private static NpcSo CheckIfNpcIsTargetOfImmersionQuest(ImmersionQuestSo immersionQuestSo)
        {
            var questNpc = immersionQuestSo switch
            {
                ListenQuestSo listenQuestSo => listenQuestSo.Npc,
                GiveQuestSo giveQuestSo => giveQuestSo.GiveQuestData.NpcToReceive,
                ReportQuestSo reportQuestSo => reportQuestSo.Npc,
                _ => null
            };
            return questNpc;
        }

        private void AddQuestToQueueIfNpcIsTarget(NpcSo questNpc, QuestSo questSo)
        {
            if (questNpc.NpcName == Npc.NpcName)
            {
                _assignedQuestsQueue.Enqueue(questSo);
            }
        }

        private void CreateQuestCompletedDialogue(object sender, NewQuestEventArgs eventArgs)
        {
            if (eventArgs.NpcInCharge != Npc) return;
            var questId = eventArgs.Quest.Id;
            dialogue.StopDialogueFromQuest(questId);
            var closerLine = NpcDialogueGenerator.CreateQuestCloser(eventArgs.Quest, Npc);
            dialogue.AddDialogue(Npc.DialogueData, closerLine, false, questId, true);
        }
        
        private void CreateQuestOpenedDialogue(QuestSo quest, NpcSo npcInCharge)
        {
            if (npcInCharge != Npc) return;
            var openerLine = NpcDialogueGenerator.CreateQuestOpener(quest, Npc);
            var questId = quest.Id;
            dialogue.AddDialogue(Npc.DialogueData, openerLine, true, questId);
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
            dialogue.AddDialogue(Npc.DialogueData, greetingDialogue, true, 0);
        }

        private bool HasAtLeastOneTrigger()
        {
            return GetComponents<Collider2D>().Any(col => col.isTrigger);
        }

        public void OnInteractAttempt()
        {
            if (_isDialogueNull) return;
            var incompleteQuestQueue = new Queue<QuestSo>();
            while (_assignedQuestsQueue.Count > 0)
            {
                var quest = _assignedQuestsQueue.Dequeue();
                switch (quest)
                {
                    case ExchangeQuestSo exchangeQuest when !quest.IsCompleted:
                        incompleteQuestQueue.Enqueue(quest);
                        continue;
                    case ExchangeQuestSo exchangeQuest:
                        exchangeQuest.TradeItems();
                        break;
                    case GiveQuestSo giveQuest when !quest.IsCompleted:
                        incompleteQuestQueue.Enqueue(quest);
                        continue;
                    case GiveQuestSo giveQuest:
                        giveQuest.GiveItems();
                        break;
                }
                ((IQuestElement)this).OnQuestTaskResolved(this, new QuestTalkEventArgs(Npc, quest.Id));
            }
            _assignedQuestsQueue = incompleteQuestQueue;

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