using Overlord.NarrativeGenerator;
using Overlord.NarrativeGenerator.Quests;
using System.Text;
using Topdown.Overlord.NarrativeGenerator;
using UnityEngine;
using static Util.Enums;

namespace Game.NPCs
{
    public class QuestDialogue : MonoBehaviour
    {
        protected Language _language;

        private void Awake()
        {
            var questGeneratorManager = FindObjectOfType<QuestGeneratorManager>();
            if (questGeneratorManager == null)
                questGeneratorManager = FindObjectOfType<TopdownQuestGeneratorManager>();
            _language = questGeneratorManager.language;
        }

        protected virtual string [] lowSocialDialogues {
            get { return new string[0]; }
            }

        protected virtual string [] averageSocialDialogues {
            get { return new string[0]; }
            }

        protected virtual string [] highSocialDialogues {
            get { return new string[0]; }
            }

        public string CreateQuestDialogue(QuestSo closedQuest, NpcSo speaker)
        {
            switch (speaker.SocialFactor)
            {
                case < 3:
                    return GetQuestDialogue( lowSocialDialogues, closedQuest, speaker );
                case < 5:
                    return GetQuestDialogue( averageSocialDialogues, closedQuest, speaker );
                default:
                    return GetQuestDialogue( highSocialDialogues, closedQuest, speaker );
            }
        }

        protected string GetQuestDialogue( string[] dialogues, QuestSo quest, NpcSo speaker )
        {
            var createdDialogue = new StringBuilder();
            int randomDialogue = UnityEngine.Random.Range( 0, dialogues.Length );

            if ( dialogues.Length == 0 )
            {
                Debug.LogError("Not enough Dialogues for quest type "+this);
                return "";
            }

            createdDialogue.Append(
                dialogues[randomDialogue]
                    .Replace("{speaker.NpcName}", speaker.NpcName)
                    .Replace("{speaker.Job}", speaker.Job.ToString())
                    .Replace("{questSo.GetTargetNpc()}", quest.GetTargetNpc())
                    .Replace("{questSo.GetItemAmountString()}", quest.GetItemAmountString(_language))
                    .Replace("{questSo.GetItemString()}", quest.GetItemString())
                    .Replace("{questSo.GetRoomAmount()}", quest.GetRoomAmount())
                    .Replace("{questSo.GetRoomCoordinates()}", quest.GetRoomCoordinates())
                    .Replace("{questSo.GetEnemyAmountString()}", quest.GetEnemyAmountString(_language))
                    .Replace("{questSo.GetEnemyString()}", quest.GetEnemyString(_language))
                    .Replace("{questSo.GetOwnerNpc()}",quest.GetOwnerNpc())
                );
            return createdDialogue.ToString();
        }
    }
}