using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs
{
    public class ListenQuestOpener : QuestOpener
    {
        protected override string [] lowSocialOpeners {
            get => new string [] {
            "Enough questions! {questSo.GetTargetNpc()} has the answers. Go get an earful from them.",
            "Ugh, just go listen to {questSo.GetTargetNpc()}, will you? They won't stop talking until someone does.",
            "If you want answers, go bother {questSo.GetTargetNpc()}. I've got enough on my plate.",
            "Listen, I don’t have time to explain everything. Go hear it from {questSo.GetTargetNpc()}.",
            "If you’re so curious, go talk to {questSo.GetTargetNpc()}. They love to ramble on.",
            "Go on, then. {questSo.GetTargetNpc()} has all the answers you’re looking for. Don’t make me repeat myself.",
            "You want information? Go listen to {questSo.GetTargetNpc()} instead of pestering me.",
            "Just go. {questSo.GetTargetNpc()} can tell you everything. I don’t have the patience for this.",
            "Look, if you really need to know, go listen to {questSo.GetTargetNpc()}. They’re dying to chat.",
            "I'm busy. {questSo.GetTargetNpc()} has all the details. Go waste their time, not mine.",
            "Fine, go listen to {questSo.GetTargetNpc()}. I’m sure they’ll tell you everything... twice.",
            "If you're not gonna leave, at least go bother {questSo.GetTargetNpc()}. They love talking.",
            "If you're that desperate for info, go talk to {questSo.GetTargetNpc()}. They’re probably itching to yap.",
            "Ugh, for the last time—go listen to {questSo.GetTargetNpc()}. They'll tell you more than you ever wanted."
            };
        }

        protected override string [] averageSocialOpeners {
            get => new string [] {
            "Have you been wondering about the origins of this dungeon? Go listen to {questSo.GetTargetNpc()} — they’ve seen things that could help you understand more.",
            "It might be helpful to listen to {questSo.GetTargetNpc()}. They’ve got knowledge that may aid you.",
            "I know you’re busy, but hearing what {questSo.GetTargetNpc()} has to say could make a difference.",
            "Trust me, it’ll be worth it to listen to {questSo.GetTargetNpc()}. They’re full of valuable information.",
            "Listen carefully to what {questSo.GetTargetNpc()} says. They’ve been through a lot and know secrets.",
            "You should take the time to hear out {questSo.GetTargetNpc()}. They may have knowledge you can use."
            };
        }

        protected override string [] highSocialOpeners {
            get => new string [] {
            "I think you’d find it helpful to listen to {questSo.GetTargetNpc()}. They have a gentle wisdom about them.",
            "Please, take some time to listen to {questSo.GetTargetNpc()}. I think you'll find it truly worthwhile.",
            "I'm sure it would mean a lot to {questSo.GetTargetNpc()} if you listened to what they have to say. They have a kind heart.",
            "I believe {questSo.GetTargetNpc()} could offer you valuable guidance. Could you take a moment to hear them out?",
            "I think it might help you to hear from {questSo.GetTargetNpc()}. They have an interesting perspective on things. Always helps me out!",
            "It would be wonderful if you could hear what {questSo.GetTargetNpc()} has to say. They speak from the heart."
            };
        }
    }
}