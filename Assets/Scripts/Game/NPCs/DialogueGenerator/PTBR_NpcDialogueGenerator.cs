using System.Text;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using Overlord.NarrativeGenerator.NPCs;
using Overlord.NarrativeGenerator.Quests;
using Overlord.NarrativeGenerator.Quests.QuestGrammarTerminals;

namespace Game.NPCs.PTBR
{
    public static class PTBR_NpcDialogueGenerator
    {
        public static string CreateGreeting(NpcSo speaker)
        {
            return DialogueGreetings.CreateGreeting( speaker );
        }

        public static string CreateMainQuestLineOpener(QuestLine openedQuestLine, NpcSo speaker )
        {
            return "Eu tenho uma chave que pode ser interessante para você, mas só te darei se você me ajudar com algumas coisas...";
        }

        public static string CreateMainQuestLineCloser( QuestLine closedQuestLine, NpcSo speaker )
        {
            var questCloserDialogue = new StringBuilder();
            questCloserDialogue.Append( "Okay, você mereceu, aqui está a chave que te falei..." );
            if ( closedQuestLine.IsMainQuest )
            {
                questCloserDialogue.Append($"<completequestline={speaker.NpcName}, {closedQuestLine.RewardKeys[0]}>");
            }
            return questCloserDialogue.ToString();
        }
        
        public static string CreateExchangeDialogue(ExchangeQuestSo quest, NpcSo npc)
        {
            var questExchangeDialogue = new StringBuilder();

            questExchangeDialogue.Append($"{quest.NpcInCharge.NpcName} te enviou para me entregar esse/a(s) {quest.GetItemString()}? ");
            var spriteString = quest.ExchangeData.ReceivedItem.GetToolSpriteString();
            questExchangeDialogue.Append($"Pegue esse/a {quest.ExchangeData.ReceivedItem.ItemName} {spriteString} como recompensa do seu trabalho.");
            questExchangeDialogue.Append($"<trade={npc.NpcName}, {quest.Id}>");
            return questExchangeDialogue.ToString();
        }
        
        public static string CreateGiveDialogue(GiveQuestSo quest, NpcSo npc)
        {
            var questGiveDialogue = new StringBuilder();
            var spriteString = quest.GiveQuestData.ItemToGive.GetToolSpriteString();
            questGiveDialogue.Append($"Você pegou todos os {quest.GiveQuestData.ItemToGive.ItemName} {spriteString}.");
            questGiveDialogue.Append($"<give={npc.NpcName}, {quest.Id}>");
            return questGiveDialogue.ToString();
        }
#if UNITY_EDITOR
        [ButtonMethod]
        public static string CreateMockGoToQuest()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Eu preciso que você vá até aqui <goto=10,12>");
            return stringBuilder.ToString();
        }
#endif

    }
}