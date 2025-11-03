using Game.ExperimentControllers;
using Game.Maestro;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;
using Overlord.ProfileAnalyst;
using System.Collections.Generic;
using UnityEditor;

namespace Overlord.NarrativeGenerator
{
    public class NarrativeExperimentRepository
    {
        private readonly PlayerProfileToQuestLinesDictionarySo _playerProfileToQuestLines;
        private List<QuestLineList> _questLinesForProfile;
        private IPlayerProfile _playerProfile;
        private GeneratorSettings _generatorSettings;

        public NarrativeExperimentRepository(IPlayerProfile playerProfile, 
            PlayerProfileToQuestLinesDictionarySo playerProfileToQuestLines, GeneratorSettings generatorSettings)
        {
            _playerProfile = playerProfile;
            _playerProfileToQuestLines = playerProfileToQuestLines;
            _generatorSettings = generatorSettings;
        }

        public void Save(QuestLineList questLines, string profileName)
        {
            SetQuestLineListForProfile(questLines);

            string target = "Assets/Resources/Experiment";
            string questLineFile = $"{target}/{profileName}";

            questLines.SaveAsset(questLineFile);

            EditorUtility.SetDirty(questLines);
            AssetDatabase.SaveAssetIfDirty(questLines);

            EditorUtility.SetDirty(_playerProfileToQuestLines);
            AssetDatabase.SaveAssetIfDirty(_playerProfileToQuestLines);
        }

        private void SetQuestLineListForProfile(QuestLineList questLines)
        {
            _questLinesForProfile = new List<QuestLineList> { Selector.CreateMissions(_generatorSettings) };
            /*
            if (_playerProfile is YeePlayerProfile playerProfile)
            {
                if (_playerProfileToQuestLines.QuestLinesForProfile.TryGetValue(
                        playerProfile.PlayerProfileEnum.ToString(), out var questLinesForProfile))
                {
                    _questLinesForProfile = questLinesForProfile;
                }
                else
                {
                    _questLinesForProfile = new List<QuestLineList>();
                    _questLinesForProfile.Add(questLines);
                    _playerProfileToQuestLines.QuestLinesForProfile.Add(playerProfile.PlayerProfileEnum.ToString(), _questLinesForProfile);
                }
            }
            */
        }
    }
}