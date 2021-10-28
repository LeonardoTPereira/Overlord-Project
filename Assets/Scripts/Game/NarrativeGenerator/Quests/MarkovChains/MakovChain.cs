using UnityEngine;
using System.Collections.Generic;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests.QuestTerminals;

namespace Game.NarrativeGenerator.Quests
{
    public class MarkovChain
    {
        public Symbol symbol;
        public SymbolType symbolType;

        public MarkovChain ()
        {
            symbol = new NonTerminalQuest();
            symbolType = SymbolType.Start;
        }

        public void SetSymbol ( SymbolType _symbol )
        {
            symbolType = _symbol;
            switch ( _symbol )
            {
                case SymbolType.Kill:
                    this.symbol = new Kill();
                break;
                case SymbolType.Talk:
                    this.symbol = new Talk();
                break;
                case SymbolType.Get:
                    this.symbol = new Get();
                break;
                case SymbolType.Explore:
                    this.symbol = new Explore();
                break;
                case SymbolType.kill:
                    this.symbol = new KillQuestSO();
                break;
                case SymbolType.talk:
                    this.symbol = new TalkQuestSO();
                break;
                case SymbolType.empty:
                    this.symbol = new EmptyQuestSO();
                break;
                case SymbolType.get:
                    this.symbol = new GetQuestSO();
                break;
                case SymbolType.drop:
                    this.symbol = new DropQuestSO();
                break;
                case SymbolType.item:
                    // this.symbol = new ItemQuestSO();
                break;
                case SymbolType.secret:
                    this.symbol = new SecretRoomQuestSO();
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