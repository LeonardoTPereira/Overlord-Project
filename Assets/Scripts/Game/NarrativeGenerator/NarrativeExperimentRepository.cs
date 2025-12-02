using Game.ExperimentControllers;
using Game.Maestro;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;
using Overlord.ProfileAnalyst;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Util;

namespace Overlord.NarrativeGenerator
{
    public class NarrativeExperimentRepository
    {
        private readonly PlayerProfileToQuestLinesDictionarySo _playerProfileToQuestLines;
        private List<QuestLineList> _questLinesForProfile;
        private IPlayerProfile _playerProfile;
        private GeneratorSettings _generatorSettings;
        private Enums.Language _language;

        public NarrativeExperimentRepository(IPlayerProfile playerProfile, 
            PlayerProfileToQuestLinesDictionarySo playerProfileToQuestLines, GeneratorSettings generatorSettings,
            Enums.Language language)
        {
            _playerProfile = playerProfile;
            _playerProfileToQuestLines = playerProfileToQuestLines;
            _generatorSettings = generatorSettings;
            _language = language;
        }

        public void Save(QuestLineList questLines, string profileName)
        {
            SetQuestLineListForProfile(questLines);

#if UNITY_EDITOR
            string target = "Assets/Resources/Experiment";
            string questLineFile = $"{target}/{profileName}";

            questLines.SaveAsset(questLineFile);

            EditorUtility.SetDirty(questLines);
            AssetDatabase.SaveAssetIfDirty(questLines);

            EditorUtility.SetDirty(_playerProfileToQuestLines);
            AssetDatabase.SaveAssetIfDirty(_playerProfileToQuestLines);
#endif
        }

        private void SetQuestLineListForProfile(QuestLineList questLines)
        {
            _questLinesForProfile = new List<QuestLineList> { TopdownQuestSelector.CreateMissions(_generatorSettings, _language) };
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