using UnityEngine;

[CreateAssetMenu]
public class LevelConfigSO : ScriptableObject
{
    public int enemy, size, linearity;
    public string levelName;
    public string fileName;
}
