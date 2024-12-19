using System.Collections;
using System.Collections.Generic;
using Game.Events;
using Game.LevelGenerator.LevelSOs;
using Game.LevelManager.DungeonLoader;
using Game.LevelSelection;
using Game.Maestro;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;
using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace Game.GameManager
{
    // TODO: Setup do experimento - 50% de chance aleatório e 50% de chance do perfil complemento
    // TODO: Pula tela de level selection e carrega o nível gerado -> ao inves de carregar tela de level select,
    // carrega tela de weapon select -> pode carregar direto a cena sem weapon select


    // TODO: PERFIS
    // Perfil recomendado = pré-teste
    // Outro = complemento do pré-teste


    // TODO: 
    // Questão do loop -> chamar o profileselectedevent 
    // baseado na % de quests completas
    // importante -> vida perdida, quantos inimigos matou, (olhar artigo que o leo mandar)
    // quest de imersão, uso de fechadura, uso de chave, compleção do mapa

    // geração do novo perfil após o pos-teste
    // ponderação em inputs da sala + dados do jogador -> rebalancear para 1, 2, 3, 4

    // fator da perfil -> 1 = .25, 2 = ..., 4 = 1.00
    // fator do perfil - dados do jogador > 0.2 muda, senão mantem
    // clamp no 1 e no 4

    // combinação/ponderação entre (1 - %vida perdida, quantos inimigos matou) => mastery
    // valor de imersão => % de compleção de quests de imerção
    // compleção do mapa, lock used => explorer
    // todos os dados juntos/ponderação ( enemy kill rate+ revist rate+ %items coletados + completude do mapa) => achiever
    // setar limite para taxa de revisitação para 100 (2 -> 100)


    // TODO: testar coleta de tesouro/itens (pode não estar funcionando)


    // TODO: verificar se estão sendo salvos e atualizados os given profiles
    // O given profile vai ser pro nível e não pro jogador

    public class ExperimentController : MonoBehaviour
    {
        public static event ProfileSelectedEvent ProfileSelectedEventHandler;

        [SerializeField, MustBeAssigned]
        private PlayerProfileToQuestLinesDictionarySo playerProfileToQuestLinesDictionarySo;
        private List<QuestLineList> _questLinesListForProfile;

        [SerializeField]
        private DungeonSceneLoader[] dungeonEntrances;

        private void Awake()
        {
            _questLinesListForProfile = null;
        }

        private void OnEnable()
        {
            QuestGeneratorManager.ProfileSelectedEventHandler += LoadDataForExperiment;
            QuestGeneratorManager.FixedLevelProfileEventHandler += LoadDataForExperiment;
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        private void OnDisable()
        {
            QuestGeneratorManager.ProfileSelectedEventHandler -= LoadDataForExperiment;
            QuestGeneratorManager.FixedLevelProfileEventHandler -= LoadDataForExperiment;
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        IEnumerator WaitForProfileToBeLoadedAndSelectNarratives(Scene scene)
        {
            yield return new WaitUntil(() => CanLoadNarrativesToDungeonEntrances(scene));
            SelectNarrativeAndSetDungeonsToEntrances();
        }

        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            StartCoroutine(WaitForProfileToBeLoadedAndSelectNarratives(scene));
        }

        private bool CanLoadNarrativesToDungeonEntrances(Scene scene)
        {
            return scene.name == "Overworld";
        }

        private void SelectNarrativeAndSetDungeonsToEntrances()
        {
            QuestLineList selectedQuestLine = GetAndRemoveRandomQuestLine();
            List<DungeonFileSo> dungeonFileSos = new List<DungeonFileSo>(selectedQuestLine.DungeonFileSos);
            dungeonEntrances = FindObjectsOfType<DungeonSceneLoader>();
            foreach (var dungeonEntrance in dungeonEntrances)
            {
                int selectedIndex = RandomSingleton.GetInstance().Random.Next(dungeonFileSos.Count);
                dungeonEntrance.SelectedDungeon = dungeonFileSos[selectedIndex];
                dungeonEntrance.LevelQuestLines = selectedQuestLine;
                dungeonEntrance.IsLastQuestLine = _questLinesListForProfile.Count == 0;
                dungeonFileSos.RemoveAt(selectedIndex);
            }
        }

        private QuestLineList GetAndRemoveRandomQuestLine()
        {
            var selectedIndex = RandomSingleton.GetInstance().Random.Next(_questLinesListForProfile.Count);
            var questLines = _questLinesListForProfile[selectedIndex];
            _questLinesListForProfile.RemoveAt(selectedIndex);
            return questLines;
        }

        private void SetQuestLinesForProfile(PlayerProfile playerProfile)
        {
            _questLinesListForProfile = new List<QuestLineList>(playerProfileToQuestLinesDictionarySo.QuestLinesForProfile[
                playerProfile.PlayerProfileEnum.ToString()]);
        }

        private void LoadDataForExperiment(object sender, ProfileSelectedEventArgs profileSelectedEventArgs)
        {

            PlayerProfile selectedProfile;
            if ( !UseTrueProfile() )
            {
                selectedProfile = 
            }
            if (sender.GetType() == typeof(RealTimeLevelSelectManager))
            {
                selectedProfile = profileSelectedEventArgs.PlayerProfile;
                SetQuestLinesForProfile(selectedProfile);
            }
            else
            {
                if (RandomSingleton.GetInstance().Random.Next(0, 100) < 50)
                {
                    selectedProfile = profileSelectedEventArgs.PlayerProfile;
                }
                else
                {
                    selectedProfile = new PlayerProfile();
                    do
                    {
                        selectedProfile.PlayerProfileEnum = (PlayerProfile.PlayerProfileCategory)RandomSingleton.GetInstance().Random.Next(0, 4);
                    } while (selectedProfile.PlayerProfileEnum == profileSelectedEventArgs.PlayerProfile.PlayerProfileEnum);
                }
                ProfileSelectedEventHandler?.Invoke(null, new ProfileSelectedEventArgs(selectedProfile));
            }
            SetQuestLinesForProfile(selectedProfile);
            ProfileSelectedEventHandler?.Invoke(null, new ProfileSelectedEventArgs(selectedProfile));
        }

        private static bool UseTrueProfile()
        {
            return RandomSingleton.GetInstance().Random.Next(0, 100) < 50;
        }
    }
}