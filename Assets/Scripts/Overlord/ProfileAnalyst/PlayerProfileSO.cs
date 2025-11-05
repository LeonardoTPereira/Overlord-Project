using MyBox;
using Overlord.RulesGenerator.EnemyGeneration;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerProfileSO", menuName = "Overlord-Project/Profile-Analyst/PlayerProfileSO")]
public class PlayerProfileSO : ScriptableObject
{
    public bool GetRandomProfile = false;

    [Header("Profile Attributes (1–100)")]
    [Range(1, 100)]
    public int Achievement = 50;
    [Range(1, 100)]
    public int Creativity = 50;
    [Range(1, 100)]
    public int Immersion = 50;
    [Range(1, 100)]
    public int Mastery = 50;
}