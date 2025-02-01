using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs.PTBR
{
    public class QuestOpener
    {
        protected virtual string [] lowSocialOpeners {
            get { return new string[0]; }
            }

        protected virtual string [] averageSocialOpeners {
            get { return new string[0]; }
            }

        protected virtual string [] highSocialOpeners {
            get { return new string[0]; }
            }

        public string CreateQuestOpener(QuestSo openedQuest, NpcSo speaker)
        {
            switch (speaker.SocialFactor)
            {
                case < 3:
                    return GetQuestOpener( lowSocialOpeners, openedQuest, speaker );
                case < 5:
                    return GetQuestOpener( averageSocialOpeners, openedQuest, speaker );
                default:
                    return GetQuestOpener( highSocialOpeners, openedQuest, speaker );
            }
        }

        protected string GetQuestOpener( string[] openers, QuestSo openedQuest, NpcSo speaker )
        {
            var opener = new StringBuilder();
            int randomOpener = Random.Range( 0, openers.Length );

            opener.Append( 
                openers[randomOpener]
                    .Replace("{speaker.Job}", speaker.Job.ToString() )
                    .Replace("{questSo.GetTargetNpc()}", openedQuest.GetTargetNpc() )
                    .Replace("{questSo.GetItemAmountString()}", openedQuest.GetItemAmountString())
                    .Replace("{questSo.GetItemString()}", openedQuest.GetItemString())
                    .Replace("{questSo.GetRoomAmount()}", openedQuest.GetRoomAmount())
                    .Replace("{questSo.GetRoomCoordinates()}", openedQuest.GetRoomCoordinates() )
                    .Replace("{questSo.GetEnemyAmountString()}", openedQuest.GetEnemyAmountString())
                    .Replace("{questSo.GetEnemyString()}", openedQuest.GetEnemyString())
                    .Replace($"{speaker.NpcName}", "eu")
                    .Replace("{speaker.NpcName}", speaker.NpcName )
                );
            return opener.ToString();
        }
    }
}