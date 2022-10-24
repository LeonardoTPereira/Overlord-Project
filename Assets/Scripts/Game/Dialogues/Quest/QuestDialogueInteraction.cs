using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fog.Dialogue;
using Game.Dialogues;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.Quests;
using UnityEngine;
using ScriptableObjects;

#if UNITY_EDITOR
using MyBox;
using UnityEditor;
using Util;
#endif

namespace Game
{

    public class QuestDialogueInteraction : MonoBehaviour, IInteractable, IQuestElement
    {
        [field: SerializeField] public IDialogueObjSo DialogueObj { get; set; }

        [SerializeField] protected DialogueController dialogue;
        public string DialogueLine;
        
        protected bool _isDialogueNull;
        protected bool _wasTaskResolved = false;

        protected Queue<QuestSo> _assignedQuestsQueue;
        public int QuestId { get; set; }
        
        protected virtual void Awake()
        {
            _assignedQuestsQueue = new Queue<QuestSo>();
        }

        protected void Start()
        {
            CreateIntroDialogue();
        }

        protected virtual void OnEnable()
        {
            QuestLine.QuestOpenedEventHandler += OpenQuest;
        }

        protected virtual void OnDisable()
        {
            QuestLine.QuestOpenedEventHandler -= OpenQuest;
        }

        protected virtual void OpenQuest(object sender, NewQuestEventArgs eventArgs)
        {
            var quest = eventArgs.Quest;
            AddQuestToQueueIfIsTarget(quest);
        }

        protected void AddQuestToQueueIfIsTarget(QuestSo questSo)
        {
            if ( IsTarget(questSo) )
                _assignedQuestsQueue.Enqueue(questSo);
        }

        protected virtual bool IsTarget (QuestSo questSo)
        {
            return true;
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
        
        protected virtual void CreateIntroDialogue()
        {
            dialogue = ScriptableObject.CreateInstance<DialogueController>();
            _isDialogueNull = dialogue == null;
            dialogue.AddDialogue( DialogueObj.DialogueData, DialogueLine, true, 0);
        }

        protected bool HasAtLeastOneTrigger()
        {
            return GetComponents<Collider2D>().Any(col => col.isTrigger);
        }

        public virtual void OnInteractAttempt()
        {
            if (_isDialogueNull)
                return;

            if ( !_wasTaskResolved )
            {
                ((IQuestElement)this).OnQuestTaskResolved(this, new QuestReadEventArgs(DialogueObj as ItemSo, QuestId));
                ((IQuestElement)this).OnQuestCompleted(this, new QuestReadEventArgs(DialogueObj as ItemSo, QuestId));
                _wasTaskResolved = true;
            }

            DialogueHandler.instance.StartDialogue(dialogue);
        }

        public void OnTriggerEnter2D(Collider2D col)
        {
            var agent = col.GetComponent<Agent>();
            if (agent) {
                agent.collidingInteractables.Add(this);
            }
        }

        public void OnTriggerExit2D(Collider2D col)
        {
            var agent = col.GetComponent<Agent>();
            if (agent) {
                agent.collidingInteractables.Remove(this);
                if ( _wasTaskResolved )
                {
                    Destroy(this.gameObject);
                }
            }
        }        
    }
}