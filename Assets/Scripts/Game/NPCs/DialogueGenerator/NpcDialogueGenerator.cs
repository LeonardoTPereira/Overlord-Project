using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using Overlord.NarrativeGenerator.Quests;

namespace Game.NPCs
{
    public static class NpcDialogueGenerator
    {
        public static string CreateGreeting(NpcSo speaker)
        {
            return DialogueGreetings.CreateGreeting( speaker );
        }

        public static string CreateMainQuestLineOpener(QuestLine openedQuestLine, NpcSo speaker )
        {
            return "I have a key that you might be interested in, but I'll only give it to you if you help me out with a couple of things";
        }

        public static string CreateMainQuestLineCloser( QuestLine closedQuestLine, NpcSo speaker )
        {
            var questCloserDialogue = new StringBuilder();
            questCloserDialogue.Append( "Okay, you've earned it. Here is the key I told you about..." );
            if ( closedQuestLine.IsMainQuest )
            {
                questCloserDialogue.Append($"<completequestline={speaker.NpcName}, {closedQuestLine.RewardKeys[0]}>");
            }
            return questCloserDialogue.ToString();
        }

        public static string CreateQuestOpener(QuestSo openedQuest, NpcSo speaker)
        {
            return DialogueQuestOpener.CreateQuestOpener( openedQuest, speaker );
        }

        public static string CreateQuestCloser(QuestSo closedQuest, NpcSo speaker)
        {
            return DialogueQuestCloser.CreateQuestCloser( closedQuest, speaker );
        }

        public static string CreateQuestTargetDialogueCheckPoint(QuestSo checkPointQuest, NpcSo speaker)
        {
            var questDialogue = new StringBuilder();
            questDialogue.Append(DialogueQuestCheckPoint.CreateQuestCheckPoint(checkPointQuest, speaker));
            questDialogue.Append($"<checkpoint={checkPointQuest.NpcInCharge.NpcName}, {checkPointQuest.Id}>");
            return questDialogue.ToString();
        }
        
        public static string CreateExchangeDialogue(ExchangeQuestSo quest, NpcSo npc)
        {
            var questExchangeDialogue = new StringBuilder();

            questExchangeDialogue.Append($"{quest.NpcInCharge.NpcName} sent you to deliver me this {quest.GetItemString()}? ");
            var spriteString = quest.ExchangeData.ReceivedItem.GetToolSpriteString();
            questExchangeDialogue.Append($"Take this {quest.ExchangeData.ReceivedItem.ItemName} {spriteString} for your troubles.");
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