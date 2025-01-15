using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs
{
    public class ExchangeQuestOpener : QuestOpener
    {
        protected override string [] lowSocialOpeners {
            get => new string [] {
            "Ugh, I don’t have time for this request. Make yourself usefull and do it for me. Go trade a {questSo.GetItemString()} with {questSo.GetTargetNpc()}. You might get something out of it.",
            "Look, I don’t feel like dealing with {questSo.GetTargetNpc()} today. You handle the trade, and they’ll reward you. All you need to do is get them an {questSo.GetItemString()}",
            "Why don't you go bother someone else, will ya? Take a {questSo.GetItemString()} over to [NPC Name]. They’ve got something waiting for whoever delivers it—don’t ask me what.",
            "Go trade a {questSo.GetItemString()} with {questSo.GetTargetNpc()}. You’ll get some sort of reward, and I’ll get some peace and quiet.",
            "If you’re so eager for rewards, bring a {questSo.GetItemString()} to {questSo.GetTargetNpc()}. They’ll give you something, I’m sure.",
            "I’m too busy for trade request. Take a {questSo.GetItemString()} to {questSo.GetTargetNpc()}, they’ll reward you, and I won’t have to lift a finger."
            };
        }

        protected override string [] averageSocialOpeners{
            get => new string [] {
             "If you’re up for it, I need you to trade a {questSo.GetItemString()} with {questSo.GetTargetNpc()}. They need it for a new spell they are learning and will reward you handsomely for it.",
            "Would you be willing to make a trade for me? Give a {questSo.GetItemString()} to {questSo.GetTargetNpc()}, and I hear they’ve got a nice reward ready.",
            "I was supposed to get {questSo.GetTargetNpc()} a {questSo.GetItemString()} before dawn for their new spell, but I don't think I'll make it. If you can do it for me, you can get their reward.",
            "Could you take a {questSo.GetItemString()} to {questSo.GetTargetNpc()}? They told me they are looking for it, and they have a reward for anyone that brings it to them."
            };
        }

        protected override string [] highSocialOpeners{
            get => new string [] {
            "Oh, you’re just the person I need! Could you take a {questSo.GetItemString()} over to {questSo.GetTargetNpc()}? I’m sure they’ll reward you greatly!",
            "I’ve heard that {questSo.GetTargetNpc()} is dying to get their hands on a {questSo.GetItemString()}. Take it to them, and they’ll be so thrilled—they always give the best rewards!",
            "I’ve been meaning to trade a {questSo.GetItemString()} with {questSo.GetTargetNpc()}, but I’m just so busy! If you do it for me, I’m sure they’ll give you something incredible. They always have the best treasures!",
            "You’re just the right person for this! Take a {questSo.GetItemString()} to {questSo.GetTargetNpc()}, and they’ll probably give you something better than you imagined. They always surprise me with what they have!",
            "Oh, I know you’re busy, but {questSo.GetTargetNpc()} will really appreciate a trade! They always reward kindness—trust me, it’ll be worth your time! Just get them a {questSo.GetItemString()} and you'll see what I mean!",
            "Heyy, could you trade a {questSo.GetItemString()} with {questSo.GetTargetNpc()}? I can’t wait to see the look on their face when you give them it! They’ll probably reward you with something that will blow your mind. They always have the best stuff!"
            };
        }
    }
}