using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs
{
    public static class NpcDialogueGenerator
    {
        public static string CreateGreeting(NpcSo speaker)
        {
            return DialogueGreetings.CreateGreeting( speaker );
        }

        public static string CreateQuestOpener(QuestSo openedQuest, NpcSo speaker)
        {
            return DialogueQuestOpener.CreateQuestOpener( openedQuest, speaker );
        }

        public static string CreateQuestCloser(QuestSo closedQuest, NpcSo speaker)
        {
            return DialogueQuestCloser.CreateQuestCloser( closedQuest, speaker );
        }
        
        public static string CreateExchangeDialogue(ExchangeQuestSo quest, NpcSo npc)
        {
            var questExchangeDialogue = new StringBuilder();
            questExchangeDialogue.Append("You really got all the items.");
            var spriteString = quest.ExchangeData.ReceivedItem.GetToolSpriteString();
            questExchangeDialogue.Append($"Take this {quest.ExchangeData.ReceivedItem.ItemName} {spriteString} as a reward!");
            questExchangeDialogue.Append($"<trade={npc.NpcName}, {quest.Id}>");
            return questExchangeDialogue.ToString();
        }
        
        public static string CreateGiveDialogue(GiveQuestSo quest, NpcSo npc)
        {
            var questGiveDialogue = new StringBuilder();
            var spriteString = quest.GiveQuestData.ItemToGive.GetToolSpriteString();
            questGiveDialogue.Append($"You really got the {quest.GiveQuestData.ItemToGive.ItemName} {spriteString}.");
            questGiveDialogue.Append($"<give={npc.NpcName}, {quest.Id}>");
            return questGiveDialogue.ToString();
        }
#if UNITY_EDITOR
        [ButtonMethod]
        public static string CreateMockGoToQuest()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("I need you to go to the room <goto=10,12>");
            return stringBuilder.ToString();
        }
#endif

    }
}