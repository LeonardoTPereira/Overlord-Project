using UnityEngine;
using System.Collections.Generic;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests.QuestTerminals;
using Util;

namespace Game.NarrativeGenerator.Quests
{
    public enum SymbolType
    {
        // Non-terminals
        Start,
        // Terminals
        explore,
        kill,
        talk,
        empty,
        get,
        drop,
        item,
        secret
    }
    public class MarkovChain
    {
        public Symbol symbol;
        public string symbolType;
        public int symbolNumber = 0;

        public MarkovChain ()
        {
            symbol = new NonTerminalQuest();
            symbolType = SymbolType.Start.ToString();
            symbolNumber = 0;
        }

        public void SetSymbol ( string _symbol )
        {
            symbolType = _symbol;
            symbolNumber++;
            switch ( _symbol )
            {
                case Constants.KILL_QUEST:
                    this.symbol = new Kill();
                break;
                case Constants.TALK_QUEST:
                    this.symbol = new Talk();
                break;
                case Constants.GET_QUEST:
                    this.symbol = new Get();
                break;
                case Constants.EXPLORE_QUEST:
                    this.symbol = new Explore();
                break;
                case Constants.KILL_TERMINAL:
                    this.symbol = ScriptableObject.CreateInstance<KillQuestSO>();
                break;
                case Constants.TALK_TERMINAL:
                    this.symbol = ScriptableObject.CreateInstance<TalkQuestSO>();
                break;
                case Constants.EMPTY_TERMINAL:
                    this.symbol = ScriptableObject.CreateInstance<EmptyQuestSO>();
                break;
                case Constants.GET_TERMINAL:
                    this.symbol = ScriptableObject.CreateInstance<GetQuestSO>();
                break;
                case Constants.DROP_TERMINAL:
                    this.symbol = ScriptableObject.CreateInstance<DropQuestSO>();
                break;
                case Constants.ITEM_TERMINAL:
                    this.symbol = ScriptableObject.CreateInstance<ItemQuestSO>();
                break;
                case Constants.SECRET_TERMINAL:
                    this.symbol = ScriptableObject.CreateInstance<SecretRoomQuestSO>();
                break;
                default:
                    Debug.LogError("Symbol type not found!");
                break;
            }
        }

        public Symbol GetSymbol ()
        {
            return this.symbol;
        }
    }
}