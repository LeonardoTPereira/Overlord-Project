using UnityEngine;

[CreateAssetMenu(fileName = "YeePlayerProfileSO", menuName = "Overlord-Project/Profile-Analyst/YeePlayerProfileSO")]
public class YeePlayerProfileSO : PlayerProfileSO
{
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