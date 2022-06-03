using System;
using System.Collections.Generic;
using System.Linq;
using Game.DataCollection;
using Game.Events;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarNonterminals;
using Game.NPCs;
using ScriptableObjects;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using Util;
using Enums = Util.Enums;

namespace Game.NarrativeGenerator
{
    public class QuestsToJson
    {
        public static QuestJson questJson;
        public static List<List<GeneratedQuests>> generatedQuests = new List<List<GeneratedQuests>>();

        [System.Serializable]
        public class QuestJson
        {
            [SerializeField] public List<ProfileScores> profiles = new List<ProfileScores>();
            [SerializeField] public List<GeneratedQuestList> generatedQuestLists = new List<GeneratedQuestList>();

            public QuestJson ( ProfileScores[] _profiles )
            {
                profiles.AddRange( _profiles );
            }

            public void AddGeneratedQuests ( List<GeneratedQuests> _generatedQuests )
            {
                GeneratedQuestList newQuestList = new GeneratedQuestList();
                newQuestList.Quests =  _generatedQuests;
                generatedQuestLists.Add(newQuestList);
            }
        }

        [System.Serializable]
        public class ProfileScores
        {
            [SerializeField] public string profile;
            [SerializeField] public float score;
        }

        public class GeneratedQuestList
        {
            [SerializeField] public List<GeneratedQuests> genQuests;
            public GeneratedQuestList()
            {
                genQuests = new List<GeneratedQuests>();
            }
            [SerializeField]  public List<GeneratedQuests> Quests
            {
                get => genQuests;
                set => genQuests = value;
            }
        }

        [System.Serializable]
        public class GeneratedQuests
        {
            [SerializeField] public string name;
            [SerializeField] public int total = 1;
        }

        public static void Init ( PlayerProfile profile )
        {
            List<ProfileScores> profiles = new List<ProfileScores>();
            ProfileScores mastery = new ProfileScores();
            mastery.profile = "Mastery";
            mastery.score = profile.MasteryPreference;
            profiles.Add( mastery );
            ProfileScores achievement = new ProfileScores();
            achievement.profile = "Achievement";
            achievement.score = profile.AchievementPreference;
            profiles.Add( achievement );
            ProfileScores creativity = new ProfileScores();
            creativity.profile = "Creativity";
            creativity.score = profile.CreativityPreference;
            profiles.Add( creativity ); 
            ProfileScores immersion = new ProfileScores();
            immersion.profile = "Immersion";
            immersion.score = profile.ImmersionPreference;
            profiles.Add( immersion ); 
            questJson = new QuestJson( profiles.ToArray());
        }

        public static void AddGeneratedQuests( int index, List<QuestSO> questSos )
        {
            List<GeneratedQuests> genQuests = new List<GeneratedQuests>();
            foreach (QuestSO item in questSos)
            {
                string symbol = item.symbolType.ToString();
                if ( genQuests.Find( x => x.name == symbol ) != null )
                {
                    genQuests.Find( x => x.name == symbol ).total += 1;
                }
                else
                {
                    GeneratedQuests newQuest = new GeneratedQuests();
                    newQuest.name = item.symbolType.ToString();
                    genQuests.Add( newQuest );
                }
            }
            Debug.Log("adding generated quests");
            questJson.AddGeneratedQuests( genQuests );
        }

        public static void CreateJson()
        {
            // foreach( List<GeneratedQuests> genQ in questJson.generatedQuests )
            // {
            //     foreach (GeneratedQuests g in genQ)
            //     {
            //         Debug.Log(g.name);
            //     }
            // }
            Debug.Log(JsonUtility.ToJson(questJson));
            // var folder = "folder"+ Constants.SEPARATOR_CHARACTER + QuestJson.selectedProfile;
            // if (!Directory.Exists(folder))
            // {
            //     Directory.CreateDirectory(folder);
            // }
            // var file = folder + Constants.SEPARATOR_CHARACTER + QuestJson.selectedProfile;
            // var fileEnding = ".json";
            // var fileCounter = 0;
            // if (File.Exists(file))
            // {
            //     fileCounter++;
            //     while (File.Exists(file + fileCounter + fileEnding))
            //     {
            //         fileCounter++;
            //     }
            //     file += fileCounter + fileEnding;
            // }
            // else
            // {
            //     file += fileEnding;
            // }

            // using (var fileStream = new FileStream(file, FileMode.OpenOrCreate))
            // {
            //     using (var sw = new StreamWriter(fileStream))
            //     {
            //         sw.Write(JsonUtility.ToJson(questJson));
            //     }
            // }
            // Debug.Log("Writing Json file..");
        }
    }
}