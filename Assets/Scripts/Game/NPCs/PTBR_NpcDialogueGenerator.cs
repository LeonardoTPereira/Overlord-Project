﻿using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs
{
    public static class PTBR_NpcDialogueGenerator
    {
        public static string CreateGreeting(NpcSo speaker)
        {
            var greeting = new StringBuilder();
            switch (speaker.SocialFactor)
            {
                case < 3:
                    greeting.Append("...");
                    break;
                case < 5:
                    greeting.Append("Ei,");
                    break;
                case < 8:
                    greeting.Append("Olá,");
                    break;
                default:
                    greeting.Append(speaker.ViolenceFactor < 8 ? "Que prazer," : "O que você fazer comigo?");
                    break;
            }
            //greeting.Append(" Eu sou "+speaker.NpcName+", o "+speaker.Job+".");
            greeting.Append(" Eu sou " + speaker.NpcName + " " + speaker.Job + ".");
            return greeting.ToString();
        }

        public static string CreateQuestOpener(QuestSo openedQuest, NpcSo speaker)
        {
            var questOpener = new StringBuilder();
            questOpener.Append("Saudações aventureiro! Eu estava esperando por você!");
            questOpener.Append(CreateOpener(openedQuest, speaker));
            questOpener.Append(openedQuest.ToString());
            return questOpener.ToString();
        }

        private static string CreateOpener(QuestSo openedQuest, NpcSo speaker)
        {
            switch (openedQuest)
            {
                case ExchangeQuestSo:
                    return ExchangeQuestOpener();
                case GatherQuestSo:
                    return GatherQuestOpener();
                case KillQuestSo:
                    return KillQuestOpener();
                case DamageQuestSo:
                    return DamageQuestOpener();
                case GiveQuestSo:
                    return GiveQuestOpener();
                case ListenQuestSo:
                    return ListenQuestOpener();
                case ReadQuestSo:
                    return ReadQuestOpener();
                case ReportQuestSo:
                    return ReportQuestOpener();
                case ExploreQuestSo:
                    return ExploreQuestOpener();
                case GotoQuestSo:
                    return GotoQuestOpener();
                default:
                    Debug.LogError($"No quest type for this quest {openedQuest.GetType()} " +
                                   "was found to create dialogue");
                    return null;
            }
        }

        #region QuestOpeners
        private static string ExchangeQuestOpener()
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

        private static string GatherQuestOpener()
        {
            int text = UnityEngine.Random.Range(0, 3);
            switch (text)
            {
                case 0:
                    return "I need you to collect:\n";
                case 1:
                    return "Why are there so many things laying on the floor!? Please, can't you do something about this? Collect ";
                default:
                    return "There's a lot of treasures around this dungeon. Don't be shy, feel free to collect it all! In fact, I think you reall should collect ";
            }
        }

        private static string KillQuestOpener()
        {
            int text = UnityEngine.Random.Range(0, 3);
            switch (text)
            {
                case 0:
                    return "I need you to kill some monsters for me:\n";
                case 1:
                    return "Nasty monsters! They are EVERYWHERE!! Please don't tell anyone, but I'm a bit scared of them... Could you please kill ";
                default:
                    return "The monsters in this dungeon sure are annoying! Do us a favor and kill ";
            }
        }

        private static string DamageQuestOpener()
        {
            return "...";
        }

        private static string GiveQuestOpener()
        {
            int text = UnityEngine.Random.Range(0, 3);
            switch (text)
            {
                case 0:
                    return "I need you to give:\n";
                case 1:
                    return "Someone's birthday is comming up! Can you give ";
                default:
                    return "I was thinking on giving someone a little present... Can you help me with that? Please, give ";
            }
        }

        private static string ListenQuestOpener()
        {
            return "I need you to listen carefully to the message from:\n";
        }

        private static string ReadQuestOpener()
        {
            return "I need you to read the message in:\n";
        }

        private static string ReportQuestOpener()
        {
            return "I need you to report this to:\n";
        }

        private static string ExploreQuestOpener()
        {
            return "I need you to explore this dungeon:\n";
        }

        private static string GotoQuestOpener()
        {
            return "I need you to go to a special place for me:\n";
        }
        #endregion

        public static string CreateQuestCloser(QuestSo closedQuest, NpcSo speaker)
        {
            var questCloser = new StringBuilder();
            questCloser.Append("Oh meu Deus! ");
            switch (closedQuest)
            {
                case ExchangeQuestSo:
                    questCloser.Append("Você trocou tudo o que foi pedido!\n");
                    break;
                case GatherQuestSo:
                    questCloser.Append("Você coletou tudo!\n");
                    break;
                case KillQuestSo:
                    questCloser.Append("Você acabou com todos eles!\n");
                    break;
                case DamageQuestSo:
                    questCloser.Append("Você causou sérios danos a eles!\n");
                    break;
                case GiveQuestSo:
                    questCloser.Append("VocÊ entregou tudo o que era preciso!\n");
                    break;
                case ListenQuestSo:
                    questCloser.Append("Obrigado por ouvir a mensagem!\n");
                    break;
                case ReadQuestSo:
                    questCloser.Append("Você leu a mensagem!\n");
                    break;
                case ReportQuestSo:
                    questCloser.Append("Você reportou a informação!\n");
                    break;
                case ExploreQuestSo:
                    questCloser.Append("Você explorou o calabouço suficientemente!\n");
                    break;
                case GotoQuestSo:
                    questCloser.Append("Você já foi na sala pedida!\n");
                    break;
                default:
                    Debug.LogError($"No quest type for this quest {closedQuest.GetType()} " +
                                   "was found to create dialogue");
                    return null;
            }

            questCloser.Append("Muito obrigado por isso!");
            questCloser.Append($"<complete={closedQuest.Id}>");
            return questCloser.ToString();
        }
        
        public static string CreateExchangeDialogue(ExchangeQuestSo quest, NpcSo npc)
        {
            var questExchangeDialogue = new StringBuilder();
            questExchangeDialogue.Append("Você realmente coletou todos estes itens.");
            var spriteString = quest.ExchangeData.ReceivedItem.GetToolSpriteString();
            questExchangeDialogue.Append($"Pegue isso: {quest.ExchangeData.ReceivedItem.ItemName} {spriteString} como recompensa!");
            questExchangeDialogue.Append($"<trade={npc.NpcName}, {quest.Id}>");
            return questExchangeDialogue.ToString();
        }
        
        public static string CreateGiveDialogue(GiveQuestSo quest, NpcSo npc)
        {
            var questGiveDialogue = new StringBuilder();
            var spriteString = quest.GiveQuestData.ItemToGive.GetToolSpriteString();
            questGiveDialogue.Append($"Você realmente encontrou {quest.GiveQuestData.ItemToGive.ItemName} {spriteString}.");
            questGiveDialogue.Append($"<give={npc.NpcName}, {quest.Id}>");
            return questGiveDialogue.ToString();
        }
#if UNITY_EDITOR
        [ButtonMethod]
        public static string CreateMockGoToQuest()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Eu preciso que você vá para a sala <goto=10,12>");
            return stringBuilder.ToString();
        }
#endif

    }
}