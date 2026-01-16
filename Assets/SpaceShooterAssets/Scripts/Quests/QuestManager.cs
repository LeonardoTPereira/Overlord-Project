using Overlord.NarrativeGenerator.NPCs;
using Overlord.NarrativeGenerator.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public Transform questUIParent;
    public GameObject questUIPrefab;

    private readonly List<QuestInstance> _activeQuests = new();
    private GameObject _npcPrefab;

    private const int MAX_ACTIVE_QUESTS = 3;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        foreach (var quest in _activeQuests)  // TODO: Mudar para um sistema de eventos, pois isso é muito pesado computacionalmente...
        {
            quest.CheckCompletion();
            if (quest.IsCompleted)
                CompleteQuest(quest);
        }
    }

    public void TryStartQuest(QuestLine questLine)
    {
        if (_activeQuests.Count >= MAX_ACTIVE_QUESTS)
            return;

        var questSo = questLine.GetCurrentQuest();
        if (questSo == null) return;

        var category = QuestCategoryMapper.FromSymbol(questSo.SymbolType);

        var instance = CreateQuestInstance(questLine, category);
        _activeQuests.Add(instance);

        SpawnQuestUI(instance);
    }


    public void ConfigureQuest(QuestLineList questlineList, NpcSo npc, GameObject npcPrefab)
    {
        _npcPrefab = npcPrefab;

        TryStartQuest(
            questlineList.QuestLines.Find(q =>
                q.NpcInCharge != null &&
                q.NpcInCharge.NpcName == npc.NpcName));

        ConfigureQuestDialogue(questlineList, npc);
    }

    private void ConfigureQuestDialogue(QuestLineList questlineList, NpcSo npc)
    {
        QuestLine questLine = questlineList.QuestLines.Find(q =>
            q.NpcInCharge != null &&
            q.NpcInCharge.NpcName == npc.NpcName);

        if (questLine == null)
            return;

        string[] questEndSentence =
            QuestLoader.Instance.GetQuestSentence(
                questLine.GetCurrentQuest().SymbolType);

        _npcPrefab
            .GetComponent<DialogueTrigger>()
            .SetEndDialogue(questEndSentence);

        QuestManager.Instance.NotifyDialogueCompleted(FindObjectOfType<NaveNPC>());
    }

    private QuestInstance CreateQuestInstance(QuestLine questLine, QuestCategory category)
    {
        var instance = new QuestInstance
        {
            QuestLine = questLine,
            Category = category
        };

        switch (category)
        {
            case QuestCategory.Mastery:
                instance.TargetValue = Random.Range(
                    10,
                    DungeonRoomStateManager.TotalEnemiesInLevel
                );
                instance.Description = $"Derrote {instance.TargetValue} inimigos.";
                break;

            case QuestCategory.Creativity:
                instance.TargetValue = Random.Range(
                    4,
                    DungeonRoomStateManager.TotalRoomsInLevel - 1
                );
                instance.Description = $"Explore {instance.TargetValue} cyber-espaços.";
                break;

            case QuestCategory.Achievement:
                instance.TargetValue = 1; // instance.TargetValue = Random.Range(1, DungeonRoomStateManager.TotalCollectiblesInLevel);
                instance.Description = $"Colete {instance.TargetValue} PugoPoints.";
                break;

            case QuestCategory.Immersion:
                instance.Description = "Converse com o NPC até o fim...";
                break;
        }

        return instance;
    }


    private void CompleteQuest(QuestInstance quest)
    {
        quest.IsCompleted = true;
        _activeQuests.Remove(quest);

        StatusManager.Instance.AddCompletedQuest();

        bool isLastQuest =
            quest.QuestLine.CurrentQuestIndex + 1 >= quest.QuestLine.Quests.Count;

        StatusManager.Instance.RewardPlayer(isLastQuest ? 2 : 1);

        quest.QuestLine.CloseCurrentQuest();

        NaveNPC npc = FindObjectOfType<NaveNPC>();

        if (npc == null)
        {
            Debug.LogError("No NaveNPC found in the scene.");
            return;
        }

        Dialogue endQuestDialogue = new Dialogue
        {
            name = npc.name,
            sentences = isLastQuest
                ? new string[] { "Você concluiu todas as missões, parabéns!", "Tome, essa recompensa te deixará mais poderoso!"}
                : new string[] { "Muito bom! Você completou minha missão.", "Tome essas modificações para a sua nave", "Mas ainda tenho outras tarefas para você." }
        };

        DialogueManager.Instance.StartDialogue(
            npc,
            endQuestDialogue,
            null
        );

        TryStartQuest(quest.QuestLine);
    }

    public void NotifyDialogueCompleted(NaveNPC npc)
    {
        string npcName = DialogueManager.Instance.nameText.text;
        foreach (var quest in _activeQuests)
        {

            if (quest.Category == QuestCategory.Immersion &&
                quest.QuestLine.NpcInCharge.NpcName == npcName)
            {
                quest.IsCompleted = true;
            }
        }
    }

    private float questUISpacing = 80f;
    private void SpawnQuestUI(QuestInstance quest)
    {
        var ui = Instantiate(questUIPrefab, questUIParent);

        RectTransform rt = ui.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(
            0,
           //-_activeQuests.Count * rt.sizeDelta.y
           -_activeQuests.Count * questUISpacing
        );

        ui.GetComponent<QuestUI>().SetText(quest.Description);
    }
}
