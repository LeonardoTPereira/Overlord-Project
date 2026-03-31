using System;

[Serializable]
public class RoomProgressData
{
    public int RemainingEnemies;
    public int RemainingCollectibles;
    public int RemainingTools;
    public int RemainingLoreItems;

    public RoomProgressData(
        int enemies,
        int collectibles,
        int tools,
        int lore)
    {
        RemainingEnemies = enemies;
        RemainingCollectibles = collectibles;
        RemainingTools = tools;
        RemainingLoreItems = lore;
    }
}