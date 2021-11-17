using UnityEngine;
using System.Collections.Generic;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests.QuestTerminals;

namespace Game.NarrativeGenerator.Quests
{
    public enum SymbolType
    {
        // Non-terminals
        Start,
        Kill,
        Talk,
        Get,
        Explore,
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
                case SymbolType.Kill.ToString():
                    this.symbol = new Kill();
                break;
                case SymbolType.Talk.ToString():
                    this.symbol = new Talk();
                break;
                case SymbolType.Get.ToString():
                    this.symbol = new Get();
                break;
                case SymbolType.Explore.ToString():
                    this.symbol = new Explore();
                break;
                case SymbolType.kill.ToString():
                    this.symbol = ScriptableObject.CreateInstance<KillQuestSO>();
                break;
                case SymbolType.talk.ToString():
                    this.symbol = ScriptableObject.CreateInstance<TalkQuestSO>();
                break;
                case SymbolType.empty.ToString():
                    this.symbol = ScriptableObject.CreateInstance<EmptyQuestSO>();
                break;
                case SymbolType.get.ToString():
                    this.symbol = ScriptableObject.CreateInstance<GetQuestSO>();
                break;
                case SymbolType.drop.ToString():
                    this.symbol = ScriptableObject.CreateInstance<DropQuestSO>();
                break;
                case SymbolType.item.ToString():
                    this.symbol = ScriptableObject.CreateInstance<ItemQuestSO>();
                break;
                case SymbolType.secret.ToString():
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