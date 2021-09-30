using Game.NarrativeGenerator.Quests;
using UnityEngine;
using static Enums;

namespace Game.NarrativeGenerator
{
    [CreateAssetMenu(menuName = "NarrativeComponents/Dungeons")]
    public class ParametersDungeon : ScriptableObject
    {
        public int size = 0;
        public int nKeys = 0;
        public int nEnemies = -1;
        public float linearity;
        private int linearityMetric = 0;
        private int linearityEnum;

        public int Linearity
        {
            get => linearityMetric;
            set => linearityMetric = value;
        }

        public int LinearityEnum
        {
            get => linearityEnum;
            set
            {
                linearityEnum = value;
                linearity = getLinearity();
            }
        }

        public override string ToString()
        {
            return "Size=" + size + "_Keys=" + nKeys + "_lin=" + getLinearity() + "_NEnemies=" + nEnemies;
        }

        public float getLinearity()
        {
            return DungeonLinearityConverter.ToFloat((DungeonLinearity)LinearityEnum);
        }

        private void conversorDungeon(ParametersDungeon pD, QuestList quests)
        {
            for (int i = 0; i < quests.graph.Count; i++)
            {
                if (quests.graph[i].Tipo == 1 || quests.graph[i].Tipo == 3 || quests.graph[i].Tipo == 4 || quests.graph[i].Tipo == 6) pD.size++;
                if (quests.graph[i].Tipo == 0 || quests.graph[i].Tipo == 1 || quests.graph[i].Tipo == 4) pD.Linearity++;
            }

            if (pD.size < 3) pD.size = (int)DungeonSize.VerySmall;
            else if (pD.size >= 3 && pD.size < 7) pD.size = (int)DungeonSize.Medium;
            else pD.size = (int)DungeonSize.VeryLarge;

            if (pD.Linearity < 3) pD.LinearityEnum = (int)DungeonLinearity.VeryLinear;
            else if (pD.Linearity >= 3 && pD.Linearity < 7) pD.LinearityEnum = (int)DungeonLinearity.Medium;
            else pD.LinearityEnum = (int)DungeonLinearity.VeryBranched;

            if (pD.nKeys < 3) pD.nKeys = (int)DungeonKeys.AFewKeys;
            else if (pD.nKeys >= 3 && pD.nKeys < 7) pD.nKeys = (int)DungeonKeys.SeveralKeys;
            else pD.nKeys = (int)DungeonKeys.LotsOfKeys;

            pD.nEnemies = UnityEngine.Random.Range(1, 5);
        }
    }    
}
