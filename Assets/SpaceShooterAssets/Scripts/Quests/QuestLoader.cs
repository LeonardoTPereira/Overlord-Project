using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLoader : MonoBehaviour
{
    public static QuestLoader Instance;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }
    /*
    //Mastery
    public const string KillQuest = "kill";
    public const string DamageQuest = "damage";
    //Immersion
    public const string ListenQuest = "listen";
    public const string ReadQuest = "read";
    public const string ReportQuest = "report";
    public const string GiveQuest = "give";
    //Creativity
    public const string ExploreQuest = "explore";
    public const string GotoQuest = "goto";
    //Achievement
    public const string GatherQuest = "gather";
    public const string ExchangeQuest = "exchange";
    */

    public string[] GetQuestSentence(string questType)
    {
        switch (questType)
        {
            case "kill":
            case "damage":
                return new string[] { "Eliminate the target.", "Defeat the enemy.", "Take down the foe." };

            case "listen":
            case "read":
            case "report":
            case "give":
                return new string[] { "Receive the message.", "Obtain the information.", "Get the briefing." };

            case "explore":
            case "goto":
                return new string[] { "Investigate the area.", "Scout the location.", "Survey the surroundings." };

            case "gather":
            case "exchange":
                return new string[] { "Collect the items.", "Assemble the resources.", "Gather the materials." };

            default:
                return new string[] { "Complete the quest objective." };

        }
    }
}
