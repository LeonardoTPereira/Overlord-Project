using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs
{
    public class ReportQuestOpener : QuestOpener
    {
        protected override string [] lowSocialOpeners {
            get => new string [] {
            "I hate being the bearer of bad news... Or even news at all... Be of some use and send this envelope to {questSo.GetTargetNpc()}.",
            "The thing {questSo.GetTargetNpc()} asked me to do is gonna be late. If you see them around, do tell. I'm not getting out of my way to do it.",
            "Take this news to {questSo.GetTargetNpc()}, would you? I don’t have time to deal with it myself.",
            "Ugh, I’m not running errands today. Go tell {questSo.GetTargetNpc()} what’s happened and leave me out of it.",
            "Fine. If you’re so eager to help, go deliver this news to {questSo.GetTargetNpc()}. And don’t mess it up.",
            "Look, I’m too busy for this. Go find {questSo.GetTargetNpc()} and give them the update. Got it?",
            "Why do I have to do everything around here? Go tell {questSo.GetTargetNpc()} what they need to know.",
            "This news is important, but I’m not wasting my time delivering it. You handle it—{questSo.GetTargetNpc()} is waiting.",
            "Here’s the deal: you take this message to {questSo.GetTargetNpc()}, and I can finally get some peace and quiet.",
            "You want to be useful? Go tell {questSo.GetTargetNpc()} what’s going on. And don’t expect me to thank you.",
            "I’m not in the mood to play messenger. You go report the news to {questSo.GetTargetNpc()} instead.",
            "If you’re standing here, you’re not helping. Go deliver this news to {questSo.GetTargetNpc()} before I lose my patience."
        };
        }

        protected override string [] averageSocialOpeners {
            get => new string [] {
            "I need someone to tell {questSo.GetTargetNpc()} that I won't be able to get their book back as soon as I thought. Could you do me a favor and tell them?",
            "{questSo.GetTargetNpc()} borrowed me a magical book but I think I'll need a couple more weeks to learn these spells. If you see them around, could you ask them if that's okay?",
            "I need you to take this message to {questSo.GetTargetNpc()}. They need to hear this as soon as possible.",
            "Could you deliver this news to {questSo.GetTargetNpc()}? They’re the one who needs to know right now.",
            "I’ve got some critical information that needs to reach {questSo.GetTargetNpc()}. Can you take care of that for me?",
            "This news is too important to wait. Please report it to {questSo.GetTargetNpc()} immediately.",
            "Would you mind finding {questSo.GetTargetNpc()} and passing this along? It’s urgent.",
            "Someone needs to inform {questSo.GetTargetNpc()} about this. Can I trust you to do that?",
            "This is big. Go to {questSo.GetTargetNpc()} and make sure they’re up to speed.",
            "I’d go myself, but I’m tied up here. Could you deliver this update to {questSo.GetTargetNpc()} for me?",
            "Can you report this to {questSo.GetTargetNpc()}? It’s crucial that they know about it.",
            "We need to make sure {questSo.GetTargetNpc()} is informed. Will you take the news to them?"
        };
        }

        protected override string [] highSocialOpeners {
            get => new string [] {
            "STOP THE WORLD, THE BIGGEST GOSSIP JUST DROPPED! I beg of you, could you please tell {questSo.GetTargetNpc()} about this??",
            "Oh, you’re just the person I need! I have this vital information, and I really need you to take it to {questSo.GetTargetNpc()}. They’ll be so grateful to hear it from you!",
            "I’ve been meaning to get this news to {questSo.GetTargetNpc()}, but you know, time just slips away! Could you go find them and tell them about what’s happened? It’s pretty important!",
            "Okay, so I don’t want to overwhelm you, but I need you to take this news to {questSo.GetTargetNpc()}—and, trust me, they’ll be so glad you did! It’s kind of a big deal, you know?",
            "Alright, here’s the thing. I’ve got all this juicy info, and I really need you to pass it along to {questSo.GetTargetNpc()}. They’re gonna want to know, you’ll see!",
            "Listen, I could go myself, but I’m kind of tied up. Plus, I’m sure {questSo.GetTargetNpc()} would rather hear it from you, right? It’s about time we get them caught up!",
            "Okay, I know you’ve got a lot going on, but this is really important. Can you take this to {questSo.GetTargetNpc()}? They’ve been waiting for the update for ages!",
            "So, here’s the scoop: {questSo.GetTargetNpc()} really needs to hear what just happened. Do you think you could deliver the message for me? I’d do it myself, but... well, you know.",
            "Alright, I don’t want to burden you, but could you do me a huge favor? Take this info to {questSo.GetTargetNpc()}, will you? I’d do it, but I’m in the middle of three things right now!",
            "I’ve been trying to catch up with {questSo.GetTargetNpc()} all day, but you know how they are—always running off! Could you go find them and tell them what’s going on?",
            "You’re the perfect person for this job! I need you to take this important news to {questSo.GetTargetNpc()}. They’ve been waiting for this info, and I’m sure they’ll appreciate you bringing it to them!"
        };
        }
    }
}