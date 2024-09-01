using System.Collections.Generic;
using Game.Events;
using TMPro;
using UnityEngine;

namespace Game.MenuManager
{
    public class LevelDescriptionDialogueBhv : MonoBehaviour
    {
        private TextMeshProUGUI levelDialogue;
        private LevelSelectData levelSelectData;
        public TMP_ColorGradient easyGradient, mediumGradient, hardGradient;
        private static Dictionary<int, TextStruct> DUNGEONSETTINGSMAP;

        private void Awake()
        {
            DUNGEONSETTINGSMAP = new Dictionary<int, TextStruct>
            {
                { LevelConfiguration.EASYENEMY, new TextStruct("Fácil", easyGradient) },
                { LevelConfiguration.MEDIUMENEMY, new TextStruct("Médio", mediumGradient) },
                { LevelConfiguration.HARDENEMY, new TextStruct("Difícil", hardGradient) },
                { LevelConfiguration.SMALLDUNGEON, new TextStruct("Pequeno", easyGradient) },
                { LevelConfiguration.MEDIUMDUNGEON, new TextStruct("Médio", mediumGradient) },
                { LevelConfiguration.LARGEDUNGEON, new TextStruct("Grande", hardGradient) },
                { LevelConfiguration.LINEARDUNGEON, new TextStruct("Linear", easyGradient) },
                { LevelConfiguration.MAZEDUNGEON, new TextStruct("Labirinto", hardGradient) }
            };
        }

        void Start()
        {
            levelDialogue = GetComponent<TextMeshProUGUI>();
        }

        protected void OnEnable()
        {
            LevelSelectButtonBhv.SelectLevelButtonEventHandler += ShowLevelInfo;
        }

        protected void OnDisable()
        {
            LevelSelectButtonBhv.SelectLevelButtonEventHandler -= ShowLevelInfo;
        }

        protected void ShowLevelInfo(object sender, LevelSelectEventArgs args)
        {
            //TODO Fix this configuration
            //levelSelectData = new LevelSelectData((int)levelConfigSO.enemyDifficultyInDungeon, (int)levelConfigSO.dungeonSize, (int)levelConfigSO.dungeonLinearity);
            levelDialogue.text = LevelToString();
        }

        protected string LevelToString()
        {
            string str = "";
            str += "Inimigos: " + "<gradient=\"" + levelSelectData.enemyDifficulty.gradient.name + "\">" + levelSelectData.enemyDifficulty.text + "</gradient>\n";
            str += "Calabouço: " + "<gradient=\"" + levelSelectData.dungeonSize.gradient.name + "\">" + levelSelectData.dungeonSize.text + "</gradient>\n";
            str += "Exploração: " + "<gradient=\"" + levelSelectData.dungeonLinearity.gradient.name + "\">" + levelSelectData.dungeonLinearity.text + "</gradient>\n";
            return str;
        }

        protected struct TextStruct
        {
            public TextStruct(string text, TMP_ColorGradient gradient)
            {
                this.text = text;
                this.gradient = gradient;
            }
            public readonly string text;
            public readonly TMP_ColorGradient gradient;
        };

        protected struct LevelSelectData
        {
            public LevelSelectData(int enemyDifficulty, int dungeonSize, int dungeonLinearity)
            {
                TextStruct aux = DUNGEONSETTINGSMAP[enemyDifficulty];
                this.enemyDifficulty = new TextStruct(aux.text, aux.gradient);
                aux = DUNGEONSETTINGSMAP[dungeonSize];
                this.dungeonSize = new TextStruct(aux.text, aux.gradient);
                aux = DUNGEONSETTINGSMAP[dungeonLinearity];
                this.dungeonLinearity = new TextStruct(aux.text, aux.gradient);
            }
            public TextStruct enemyDifficulty;
            public TextStruct dungeonSize;
            public TextStruct dungeonLinearity;
        }

        public static class LevelConfiguration
        {
            public static readonly int EASYENEMY = 0;
            public static readonly int MEDIUMENEMY = 1;
            public static readonly int HARDENEMY = 2;
            public static readonly int SMALLDUNGEON = 100;
            public static readonly int MEDIUMDUNGEON = 101;
            public static readonly int LARGEDUNGEON = 102;
            public static readonly int LINEARDUNGEON = 200;
            public static readonly int MAZEDUNGEON = 201;
        }
    }
}
