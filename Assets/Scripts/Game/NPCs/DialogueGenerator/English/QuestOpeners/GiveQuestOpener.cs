using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs
{
    public class GiveQuestOpener : QuestOpener
    {
        protected override string [] lowSocialOpeners{
            get => new string [] {
            "Take {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()}. I’d do it myself, but I’ve got better things to do.",
            "Ugh, fine. You’re here, so make yourself useful and give {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()}. Don’t lose it.",
            "I don’t have time for this nonsense. Take {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()} and be quick about it.",
            "{questSo.GetTargetNpc()} needs {questSo.GetItemAmountString()}, and I’m not in the mood to deal with them. You handle it.",
            "Deliver {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()} for me, will you? I’ve got enough on my plate as it is.",
            "Why am I always stuck doing this? Whatever, you’re doing it now. Give {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()}.",
            "Take a {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()}. And don’t ask me why—it’s none of your business.",
            "Look, I don’t trust anyone else with this, so you’re up. Give {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()}. Try not to mess it up.",
            "You’re heading that way anyway, right? Good. Hand {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()} for me while you’re at it.",
            "I can’t stand dealing with {questSo.GetTargetNpc()} today. Take {questSo.GetItemAmountString()} to them for me, and don’t make me regret asking you."
        };
        }

        protected override string [] averageSocialOpeners{
            get => new string [] {
            "Could you take {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()} for me? They’ve been waiting for it, and I’d appreciate the help!",
            "Here’s an item that {questSo.GetTargetNpc()} needs. Can you deliver it to them? I trust you’ll get it there safely.",
            "I’ve been meaning to give {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()}, but I’m swamped. Would you mind taking it to them for me?",
            "Oh, perfect timing! I need someone to deliver {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()}. Think you can handle that?",
            "I’d really appreciate it if you could take {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()}. It’s something they’ve been asking for.",
            "Could you run {questSo.GetItemAmountString()} over to {questSo.GetTargetNpc()}? It’s important they get it soon, and I know I can count on you."
        };
        }

        protected override string [] highSocialOpeners{
            get => new string [] {
            "Oh, hey! Just the person I was hoping to see! So, {questSo.GetTargetNpc()} told me a while back that they need {questSo.GetItemAmountString()}. But, you know me—I’d get distracted and forget halfway there. Can you give them it for me? Pretty please?",
            "Oh my gosh, I’ve been meaning to get {questSo.GetItemAmountString()}  to {questSo.GetTargetNpc()} for ages! Well, okay, maybe not ages, but it feels like it. Anyway, could you deliver it? You’re so much better at this kind of thing!",
            "So, funny thing—I promised {questSo.GetTargetNpc()} I’d give them {questSo.GetItemAmountString()} , but, well, something always comes up! Could you help me out and take it to them? You’re a lifesaver!",
            "Oh, this is perfect! {questSo.GetTargetNpc()} needs {questSo.GetItemAmountString()}, and you’re just the person to deliver it! You don’t mind, right? I mean, you’re heading that way anyway... right?",
            "Okay, so here’s the deal—I need to get {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()}, but let’s be real, I’d probably drop it or lose it or something silly like that. But you? You’ve got this in the bag!",
            "So, I’ve been meaning to drop {questSo.GetItemAmountString()}  off with {questSo.GetTargetNpc()}, but, you know, life gets in the way! Would you mind taking it to them? I’ll owe you one. Or two!"
        };
        }
    }
}