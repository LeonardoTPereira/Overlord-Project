using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;
using Game.NPCs;
using ScriptableObjects;

namespace Game.LevelManager
{
    public static class NpcDispenser
    {
        public static void DistributeNpcsInDungeon(Map map, QuestLine questLine)
        {
            var npcQueue = new Queue<NpcSo>(questLine.NpcParametersForQuestLine.NpcsBySo.Keys);
            foreach (var dungeonPart in map.DungeonPartByCoordinates )
            {
                if (!(dungeonPart.Value is DungeonRoom dungeonRoom) || dungeonRoom.IsStartRoom() ||
                    !dungeonRoom.HasItemPreference) continue;
                dungeonRoom.Npcs = new List<NpcSo> {npcQueue.Dequeue()};
                if (npcQueue.Count <= 0)
                {
                    break;
                }
            }

            if (npcQueue.Count <= 0) return;
            
            foreach (var dungeonPart in map.DungeonPartByCoordinates)
            {
                if (!(dungeonPart.Value is DungeonRoom dungeonRoom) || dungeonRoom.IsStartRoom()) continue;
                dungeonRoom.Npcs = new List<NpcSo> {npcQueue.Dequeue()};
                if (npcQueue.Count <= 0)
                {
                    break;
                }
            }
        }
    }
}