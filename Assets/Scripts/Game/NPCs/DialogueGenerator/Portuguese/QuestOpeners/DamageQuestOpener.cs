using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs.PTBR
{
    public class DamageQuestOpener : QuestOpener
    {
        protected override string [] lowSocialOpeners {
            get => new string [] {
            "..."
            };
        }

        protected override string [] averageSocialOpeners {
            get => new string [] {
            
            };
        }

        protected override string [] highSocialOpeners {
            get => new string [] {
            
            };
        }
    }
}