using System;
using System.Collections;
using System.Collections.Generic;
using Fog.Dialogue;
using Game.Events;
using Game.GameManager;
using UnityEngine;

namespace Game.Dialogues
{
    public class TaggedDialogueHandler : DialogueHandler
    {
        public static event MarkRoomOnMiniMapEvent MarkRoomOnMiniMapEventHandler;


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
            var tags = ExtractTags(currentLine.Text);
            foreach (var character in currentLine.Text) {
                stringBuilder.Append(character);
                dialogueText.text = stringBuilder.ToString();
                dialogueBox.ScrollToEnd();
                yield return WaitForFrames(framesBetweenCharacters);
            }

            ProcessTags(tags);
        }

        protected override void FillDialogueText() {
            stringBuilder.Clear();
            stringBuilder.Append((dialogueText == titleText) ? currentTitle : "");
            var tags = ExtractTags(currentLine.Text);
            stringBuilder.Append(currentLine.Text);
            dialogueText.text = stringBuilder.ToString();
            ProcessTags(tags);
        }

        private void ProcessTags(string[] tags)
        {
            foreach (var textTag in tags)
            {
                Debug.Log("Process Tag: "+textTag);
                EvaluateTag(textTag);
            }
        }

        private static string[] ExtractTags(string newText)
        {
            // split the whole text into parts based off the <> tags 
            // even numbers in the array are text, odd numbers are tags
            char[] separators = {'<', '>'};
            var subTexts = newText.Split(separators);
            var customTags = new List<string>();
            for (var i = 0; i < subTexts.Length; i++)
            {
                Debug.Log("SubText: "+subTexts[i]);
                if (i % 2 != 1) continue;
                if (!IsCustomTag(subTexts[i].Replace(" ", ""))) continue;
                customTags.Add($"{subTexts[i]}");
            }

            return customTags.ToArray();
        }

        private static bool IsCustomTag(string tag)
        {
            return tag.StartsWith("goto=") || tag.StartsWith("complete=") || tag.StartsWith("give=") || tag.StartsWith("exchange");
        }
        
        private void EvaluateTag(string textTag)
        {
            if (textTag.Length <= 0) return;
            if (textTag.StartsWith("goto="))
            {
                Debug.Log("Found Go To Tag");
                var room = textTag.Split('=')[1];
                MarkRoomOnMiniMapEventHandler?.Invoke(this, new MarkRoomOnMinimapEventArgs(room));
                IsActive = false;
            }
            else if (textTag.StartsWith("complete="))
            {
                var quest = textTag.Split('=')[1];
            }
            else if (textTag.StartsWith("give="))
            {
                var item = textTag.Split('=')[1];
            }
            else if (textTag.StartsWith("exchange="))
            {
                var room = textTag.Split('=')[1];
            }
        }
    }
}