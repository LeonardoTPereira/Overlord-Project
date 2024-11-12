using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs
{
    public static class ExchangeQuestOpener
    {

        public static string CreateQuestOpener(QuestSo openedQuest, NpcSo speaker)
        {
            int text = Random.Range(0, 3);
            switch (text)
            {
                //TODO: replace someone for the npcs name
                case 0:
                    return "I need you to trade:\n";
                case 1:
                    return "I have heard there is someone nearby with some good trades! Why don't you go see for yourself? Trade ";
                default:
                    return "Have you heard there's someone trying out the merchant career. Go give them some support! Trade ";
            }
        }
    }
}