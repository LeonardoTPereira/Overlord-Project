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
        QuestInstance completedQuest = null;

        foreach (var quest in _activeQuests)
        {
            quest.CheckCompletion();
            if (quest.IsCompleted)
            {
                completedQuest = quest;
                break;
            }
        }

        if (completedQuest != null)
            CompleteQuest(completedQuest);
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

        NotifyDialogueCompleted(FindObjectOfType<NaveNPC>());
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
        _activeQuests.Remove(quest);

        if (quest.UI != null)
            Destroy(quest.UI.gameObject);

        RebuildQuestUI();

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

    private void SpawnQuestUI(QuestInstance quest)
    {
        int index = _activeQuests.Count - 1;

        var uiObj = Instantiate(questUIPrefab, questUIParent);
        RectTransform rt = uiObj.GetComponent<RectTransform>();
        RectTransform[] existingRTs = uiObj.GetComponents<RectTransform>();

        for (int i = 0; i < existingRTs.Length; i++)
        {
            existingRTs[i].sizeDelta = new Vector2(
                0,
                -index * rt.sizeDelta.y
            );
            break;
        }

        QuestUI ui = uiObj.GetComponent<QuestUI>();
        ui.SetText(quest.Description);

        quest.UI = ui; // 🔗 vínculo
    }
        private void RebuildQuestUI()
    {
        int index = 0;
        foreach (Transform child in questUIParent)
        {
            RectTransform rt = child.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(
                rt.anchoredPosition.x,
                -index * rt.sizeDelta.y
            );
            index++;
        }
    }
}
