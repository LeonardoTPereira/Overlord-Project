using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Fog.Dialogue;
using Game.Events;
using Game.GameManager;
using Game.Quests;
using UnityEngine;

namespace Game.Dialogues
{
    public class TaggedDialogueHandler : DialogueHandler, IQuestElement
    {
        public static event MarkRoomOnMiniMapEvent MarkRoomOnMiniMapEventHandler;
        public static event StartExchangeEvent StartExchangeEventHandler;
        public static event StartGiveEvent StartGiveEventHandler;
        
        private string[] _tags;

        private void OnEnable()
        {
            MinimapController.EndedMiniMapEvent += ReactivateInputPostEvent;
        }

        private void OnDisable()
        {
            MinimapController.EndedMiniMapEvent -= ReactivateInputPostEvent;
        }

        private void ReactivateInputPostEvent(object sender, EventArgs e)
        {
            IsActive = true;
        }

        protected override IEnumerator TypeDialogueTextCoroutine() {
            stringBuilder.Clear();
            stringBuilder.Append(dialogueText.text);
            stringBuilder.Append(ExtractAndSaveTags());
            var finalText = stringBuilder.ToString();
            stringBuilder.Clear();
            foreach (var character in finalText) {
                stringBuilder.Append(character);
                dialogueText.text = stringBuilder.ToString();
                dialogueBox.ScrollToEnd();
                yield return WaitForFrames(framesBetweenCharacters);
            }
            ProcessTags();
        }

        protected override void FillDialogueText() {
            stringBuilder.Clear();
            stringBuilder.Append((dialogueText == titleText) ? currentTitle : "");
            stringBuilder.Append(ExtractAndSaveTags());
            dialogueText.text = stringBuilder.ToString();
            ProcessTags();
        }

        private void ProcessTags()
        {
            foreach (var textTag in _tags)
            {
                Debug.Log("Process Tag: "+textTag);
                EvaluateTag(textTag);
            }
        }

        private string ExtractAndSaveTags()
        {
            // split the whole text into parts based off the <> tags 
            // even numbers in the array are text, odd numbers are tags
            var oldText = currentLine.Text;
            var newTextBuilder = new StringBuilder();
            char[] separators = {'<', '>'};
            var subTexts = oldText.Split(separators);
            var customTags = new List<string>();
            for (var i = 0; i < subTexts.Length; i++)
            {
                Debug.Log("SubText: "+subTexts[i]);
                if (i % 2 != 1)
                {
                    newTextBuilder.Append(subTexts[i]);
                    continue;
                }

                if (!IsCustomTag(subTexts[i].Replace(" ", "")))
                {
                    newTextBuilder.Append("<"+subTexts[i]+">");
                    continue;
                }
                customTags.Add($"{subTexts[i]}");
            }

            _tags = customTags.ToArray();
            return newTextBuilder.ToString();
        }

        private static bool IsCustomTag(string tag)
        {
            return tag.StartsWith("goto=") || tag.StartsWith("complete=") || tag.StartsWith("trade=") || tag.StartsWith("give=");
        }
        
        private void EvaluateTag(string textTag)
        {
            if (textTag.Length <= 0) return;
            if (textTag.StartsWith("goto="))
            {
                var room = textTag.Split('=')[1];
                MarkRoomOnMiniMapEventHandler?.Invoke(this, new MarkRoomOnMinimapEventArgs(room));
                IsActive = false;
            }
            else if (textTag.StartsWith("complete="))
            {
                var questId = int.Parse(textTag.Split('=')[1]);
                ((IQuestElement)this).OnQuestCompleted(this, new QuestElementEventArgs(questId));
            }
            else if (textTag.StartsWith("trade="))
            {
                var npcName = textTag.Split('=')[1];
                var questId = int.Parse(textTag.Split(',')[1]);
                StartExchangeEventHandler?.Invoke(this, new StartExchangeEventArgs(questId));
                ((IQuestElement)this).OnQuestTaskResolved(this, new QuestExchangeDialogueEventArgs(npcName, questId)); 
            }
            else if (textTag.StartsWith("give="))
            {
                var npcName = textTag.Split('=')[1];
                var questId = int.Parse(textTag.Split(',')[1]);
                StartGiveEventHandler?.Invoke(this, new StartGiveEventArgs(questId));
                ((IQuestElement)this).OnQuestTaskResolved(this, new QuestGiveDialogueEventArgs(npcName, questId)); 
            }
        }
    }
}