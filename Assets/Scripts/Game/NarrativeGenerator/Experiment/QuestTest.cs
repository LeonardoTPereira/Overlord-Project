using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.NarrativeGenerator
{
    public class QuestTest : MonoBehaviour
    {
        [System.Serializable]
        public class QuestWeightsbyType
        {
            public string[] questTypes;
            public int[] questWeights;
        }

        [SerializeField] private NarrativeGenerator narrativeGenerator;
        [SerializeField] private QuestsToJson _questsToJson;
        [SerializeField] private List<QuestWeightsbyType> questWeights = new List<QuestWeightsbyType>();
        private Dictionary<string,int> _questWeightsbyType = new Dictionary<string, int>();

        private Dictionary<string,List<int>> _sameQuestDistance = new Dictionary<string, List<int>>();
        private Dictionary<string,int> _averageSameQuestDistance = new Dictionary<string, int>();
        private Dictionary<string,List<int>> _sameTypeQuestDistance = new Dictionary<string, List<int>>();
        private Dictionary<string,int> _averageSameTypeQuestDistance = new Dictionary<string, int>();


        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            foreach ( QuestWeightsbyType weightbyType in questWeights)
            {
                _questWeightsbyType.Clear();
                for (int i = 0; i < weightbyType.questWeights.Length; i++)
                {
                    _questWeightsbyType.Add( weightbyType.questTypes[i], weightbyType.questWeights[i]);
                }
                TestPlayerProfile(_questWeightsbyType);
            }

            TestRandomGeneration();
        }

        public void TestPlayerProfile (Dictionary<string,int> questWeightsbyType)
        {
            narrativeGenerator.SelectQuestWeights(questWeightsbyType);
        }

        public void TestRandomGeneration()
        {
            QuestsToJson.Init(null);
            List<string> possibleQuests = new List<string>{"experiment","gather","repair","take","use","exchange","give","listen","read","report","escort","goto","spy","stealth","capture","damage","defend","kill"};
            for(int i = 0; i < 200; i++)
            {
                bool containsKill = false, containsExplore = false, containsTalk = false, containsGet = false;
                PopulateDictionary();
                List<string> generatedQuests = new List<string>();
                do
                {
                    int questIndex = Random.Range(0, possibleQuests.Count);
                    UpdateListContents(possibleQuests[questIndex], ref containsKill,ref containsTalk, ref containsGet, ref containsExplore );
                    generatedQuests.Add( possibleQuests[questIndex] );
                } while( !containsKill || !containsTalk || !containsGet || !containsExplore);
                GetAverageQuesDistances();
                QuestsToJson.AddGeneratedQuests( i, generatedQuests, _averageSameQuestDistance, _averageSameTypeQuestDistance );
            }
            QuestsToJson.CreateJson();
        }

        private void UpdateListContents ( string lastQuest, ref bool containsKill ,ref bool containsTalk ,ref bool containsGet ,ref bool containsExplore )
        {
            string questType = "";
            switch ( lastQuest )
            {
                case "experiment":
                case "gather":
                case "repair":
                case "take":
                case "use":
                    questType = "Achievement";
                    containsGet = true;
                    break;
                case "exchange":
                case "give":
                case "listen":
                case "report":
                case "read":
                    questType = "Immersion";
                    containsTalk = true;
                    break;
                case "capture":
                case "damage":
                case "defend":
                case "kill":
                    questType = "Mastery";
                    containsKill = true;
                    break;
                case "escort":
                case "goto":
                case "spy":
                case "stealth":
                    questType = "Creativity";
                    containsExplore = true;
                    break;
            }

            List<string> keys = new List<string>(_sameQuestDistance.Keys);
            foreach (string item in keys)
            {
                if ( item != lastQuest )
                {
                    _sameQuestDistance[item][_sameQuestDistance[item].Count-1] += 1;
                }
                else
                {
                    _sameQuestDistance[item].Add(0);
                }
            }

            List<string> keys2 = new List<string>(_sameTypeQuestDistance.Keys);
            foreach (string item in keys2)
            {
                if ( item != questType )
                {
                    _sameTypeQuestDistance[item][_sameTypeQuestDistance[item].Count-1] += 1;
                }
                else
                {
                    _sameTypeQuestDistance[item].Add(0);
                }
            }
        }

        private void PopulateDictionary()
        {
            string[] questNames = {"experiment", "gather", "repair", "take", "use", "exchange", "give", "listen", "report", "read",
                "capture", "damage", "defend", "kill", "escort", "goto", "spy", "stealth"};

            string[] questTypes = {"Achievement", "Immersion", "Mastery", "Creativity"};

            _sameQuestDistance.Clear();
            _sameTypeQuestDistance.Clear();
            
            foreach (var questName in questNames)
            {
                _sameQuestDistance.Add( questName, new List<int>());
                _sameQuestDistance[questName].Add(0);
            }

            foreach (var questType in questTypes)
            {
                _sameTypeQuestDistance.Add( questType, new List<int>());
                _sameTypeQuestDistance[questType].Add(0);
            }
        }

        private void GetAverageQuesDistances()
        {
            _averageSameQuestDistance.Clear();
            _averageSameTypeQuestDistance.Clear();
            foreach (KeyValuePair<string,List<int>> pair in _sameQuestDistance)
            {
                _averageSameQuestDistance.Add( pair.Key, pair.Value.Sum()/pair.Value.Count );
            }
            foreach (KeyValuePair<string,List<int>> pair in _sameTypeQuestDistance)
            {
                _averageSameTypeQuestDistance.Add( pair.Key, pair.Value.Sum()/pair.Value.Count );
            }
        }
    }
}