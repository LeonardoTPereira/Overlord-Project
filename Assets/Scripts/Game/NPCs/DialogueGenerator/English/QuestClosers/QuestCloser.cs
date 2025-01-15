using System.Linq;
using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs
{
    public class QuestCloser
    {
        protected virtual string [] lowSocialClosers {
            get { return new string[0]; }
            }

        protected virtual string [] averageSocialClosers {
            get { return new string[0]; }
            }

        protected virtual string [] highSocialClosers {
            get { return new string[0]; }
            }

        public string CreateQuestCloser(QuestSo closedQuest, NpcSo speaker)
        {
            switch (speaker.SocialFactor)
            {
                case < 3:
                    return GetQuestCloser( lowSocialClosers, closedQuest, speaker );
                case < 5:
                    return GetQuestCloser( averageSocialClosers, closedQuest, speaker );
                default:
                    return GetQuestCloser( highSocialClosers, closedQuest, speaker );
            }
        }

        protected string GetQuestCloser( string[] closers, QuestSo closedQuest, NpcSo speaker )
        {
            var opener = new StringBuilder();
            int randomOpener = Random.Range( 0, closers.Length );

            if ( closers.Length == 0 )
            {
                Debug.LogError("Not enough closers for quest type "+this);
                return "";
            }
            
            opener.Append( 
                closers[randomOpener]
                    .Replace( "{speaker.NpcName}", speaker.NpcName )
                    .Replace( "{speaker.Job}", speaker.Job.ToString() )
                    .Replace( "{questSo.GetTargetNpc()}", closedQuest.GetTargetNpc() )
                );
            return opener.ToString();
        }
    }
}