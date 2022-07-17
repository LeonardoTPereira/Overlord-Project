using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;
using Game.NPCs;
using UnityEngine;

namespace Game.LevelManager.DungeonLoader
{
    public static class NpcDispenser
    {
        public static void DistributeNpcsInDungeon(Map map, QuestLine questLine)
        {
            List<NpcSo> Npcs = questLine.NpcParametersForQuestLine.GetNpcs();
            var npcQueue = new Queue<NpcSo>(Npcs);
            foreach (var dungeonPart in map.DungeonPartByCoordinates )
            {
                if (!(dungeonPart.Value is DungeonRoom dungeonRoom) || dungeonRoom.IsStartRoom() ||
                    !dungeonRoom.HasItemPreference) continue;
                dungeonRoom.Npcs = new List<NpcSo> {npcQueue.Dequeue()};
                if (npcQueue.Count > 0) continue;
                break;
            }

            if (npcQueue.Count <= 0) return;
            
            foreach (var dungeonPart in map.DungeonPartByCoordinates)
            {
                if (!(dungeonPart.Value is DungeonRoom dungeonRoom) || dungeonRoom.IsStartRoom()) continue;
                dungeonRoom.Npcs = new List<NpcSo> {npcQueue.Dequeue()};
                if (npcQueue.Count > 0) continue;
                break;
            }
        }
    }
}