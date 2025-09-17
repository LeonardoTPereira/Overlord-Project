using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs
{
    public class DamageQuestOpener : QuestDialogue
    {
        protected override string [] lowSocialDialogues {
            get => new string [] {
            "..."
            };
        }

        protected override string [] averageSocialDialogues {
            get => new string [] {
            
            };
        }

        protected override string [] highSocialDialogues {
            get => new string [] {
            
            };
        }
    }
}