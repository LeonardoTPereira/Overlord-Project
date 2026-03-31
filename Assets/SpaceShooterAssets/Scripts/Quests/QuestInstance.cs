using Overlord.NarrativeGenerator.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInstance
{
    public QuestLine QuestLine;
    public QuestUI UI;
    public QuestCategory Category;
    public int TargetValue;
    public bool IsCompleted;

    public string Description;

    public void CheckCompletion()
    {
        if (IsCompleted) return;

        switch (Category)
        {
            case QuestCategory.Mastery:
                IsCompleted = StatusManager.Instance.EnemiesDefeated >= TargetValue;
                break;

            case QuestCategory.Creativity:
                IsCompleted = StatusManager.Instance.VisitedRooms >= TargetValue;
                break;

            case QuestCategory.Achievement:
                IsCompleted = ScoreManager.Instance.GetCollectibles() >= TargetValue;
                break;

            case QuestCategory.Immersion:
                // Completed by dialogue callback
                break;
        }
    }


}

