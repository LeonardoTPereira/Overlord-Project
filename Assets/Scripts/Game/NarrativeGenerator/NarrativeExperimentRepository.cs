using System.Collections.Generic;
using System.Threading.Tasks;
using Game.EnemyGenerator;
using Game.Events;
using Game.ExperimentControllers;
using Game.LevelGenerator;
using Game.LevelGenerator.LevelSOs;
using Game.LevelSelection;
using Game.Maestro;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using Game.NarrativeGenerator.Quests;
using MyBox;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using Util;
using Overlord.ProfileAnalyst;

namespace Overlord.NarrativeGenerator
{
    public class NarrativeExperimentRepository
    {
        private readonly PlayerProfileToQuestLinesDictionarySo _playerProfileToQuestLines;
        private List<QuestLineList> _questLinesForProfile;
        private IPlayerProfile _playerProfile;

        public NarrativeExperimentRepository(IPlayerProfile playerProfile, PlayerProfileToQuestLinesDictionarySo playerProfileToQuestLines)
        {
            _playerProfile = playerProfile;
            _playerProfileToQuestLines = playerProfileToQuestLines;
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

        public void SetQuestLineListForProfile(QuestLineList questLines)
        {
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
        }
    }
}