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
        public static List<List<string>> generatedQuests = new List<List<string>>();

        [System.Serializable]
        public class QuestJson
        {
            [SerializeField] public List<ProfileScores> profiles = new List<ProfileScores>();
            [SerializeField] public List<GeneratedQuestList> generatedQuestLists = new List<GeneratedQuestList>();

            public QuestJson ( ProfileScores[] _profiles )
            {
                profiles.AddRange( _profiles );
            }

            public QuestJson (){}


            public void AddGeneratedQuests ( List<NameByTotal> _generatedQuests,  Dictionary<string,int> sameQuestDistance, Dictionary<string,int> sameTypeDistance )
            {
                GeneratedQuestList newQuestList = new GeneratedQuestList();
                newQuestList.Quests =  _generatedQuests;

                foreach (KeyValuePair<string,int> item in sameQuestDistance)
                {
                    newQuestList.sameQuestDistance.Add( new NameByTotal(item.Key, item.Value) );
                }

                foreach (KeyValuePair<string,int> item in sameTypeDistance)
                {
                    newQuestList.sameTypeDistance.Add( new NameByTotal(item.Key, item.Value) );
                }

                generatedQuestLists.Add(newQuestList);
            }
        }

        [System.Serializable]
        public class ProfileScores
        {
            [SerializeField] public string profile;
            [SerializeField] public float score;
        }

        [System.Serializable]
        public class GeneratedQuestList
        {
            [SerializeField] public List<NameByTotal> genQuests;
            [SerializeField] public List<NameByTotal> sameQuestDistance;
            [SerializeField] public List<NameByTotal> sameTypeDistance;
            public GeneratedQuestList()
            {
                genQuests = new List<NameByTotal>();
                sameQuestDistance = new List<NameByTotal>();
                sameTypeDistance = new List<NameByTotal>();
            }
            [SerializeField]  public List<NameByTotal> Quests
            {
                get => genQuests;
                set => genQuests = value;
            }
        }

        [System.Serializable]
        public class NameByTotal
        {
            [SerializeField] public string name;
            [SerializeField] public int total = 1;
            public NameByTotal(string _name, int _total = 1)
            {
                name = _name;
                total = _total;
            }
        }

        public static void Init ( PlayerProfile profile )
        {
            if ( profile == null )
            {
                questJson = new QuestJson();
                return;
            }
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

        public static void AddGeneratedQuests( int index, List<QuestSO> questSos, Dictionary<string,int> questDistance, Dictionary<string,int> typeDistance )
        {
            List<NameByTotal> genQuests = new List<NameByTotal>();
            foreach (QuestSO item in questSos)
            {
                string symbol = item.symbolType.ToString();
                if ( genQuests.Find( x => x.name == symbol ) != null )
                {
                    genQuests.Find( x => x.name == symbol ).total += 1;
                }
                else
                {
                    NameByTotal newQuest = new NameByTotal(item.symbolType.ToString() );
                    genQuests.Add( newQuest );
                }
            }
            questJson.AddGeneratedQuests( genQuests, questDistance, typeDistance );
        }

        public static void AddGeneratedQuests( int index, List<string> quests, Dictionary<string,int> questDistance, Dictionary<string,int> typeDistance )
        {
            List<NameByTotal> genQuests = new List<NameByTotal>();
            foreach (string item in quests)
            {
                string symbol = item;
                if ( genQuests.Find( x => x.name == symbol ) != null )
                {
                    genQuests.Find( x => x.name == symbol ).total += 1;
                }
                else
                {
                    NameByTotal newQuest = new NameByTotal(item.ToString());
                    genQuests.Add( newQuest );
                }
            }
            questJson.AddGeneratedQuests( genQuests, questDistance, typeDistance );
        }


        // public static void AddGeneratedQuests( int index, List<string> questNames )
        // {
        //     questJson.AddGeneratedQuests( questNames );
        // }

        public static void CreateJson()
        {
            //TODO: PRINT DISTANCES
            // foreach( GeneratedQuestList genQ in questJson.generatedQuestLists )
            // {
            //     foreach (NameByTotal q in genQ.genQuests)
            //     {
            //         Debug.Log(q.name);
            //     }
            // }
            Debug.Log(JsonUtility.ToJson(questJson));
            var folder = Application.persistentDataPath+ Constants.SEPARATOR_CHARACTER + "Results";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var file = folder + Constants.SEPARATOR_CHARACTER + Time.realtimeSinceStartup;
            var fileEnding = ".json";
            var fileCounter = 0;
            if (File.Exists(file))
            {
                fileCounter++;
                while (File.Exists(file + fileCounter + fileEnding))
                {
                    fileCounter++;
                }
                file += fileCounter + fileEnding;
            }
            else
            {
                file += fileEnding;
            }

            using (var fileStream = new FileStream(file, FileMode.OpenOrCreate))
            {
                using (var sw = new StreamWriter(fileStream))
                {
                    sw.Write(JsonUtility.ToJson(questJson));
                }
            }
            Debug.Log("Writing Json file..");
        }
    }
}