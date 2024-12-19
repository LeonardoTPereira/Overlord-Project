using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs
{
    public class KillQuestOpener : QuestOpener
    {
        protected override string [] lowSocialOpeners{
            get => new string [] {
            "There is nothing I dislike more than {questSo.GetEnemyString()}. I'll tell you what, if you get rid of about {questSo.GetEnemyAmountString()}, I might give you a reward.",
            "These {questSo.GetEnemyString()} have been getting in my way. Be of some use and kill about {questSo.GetEnemyAmountString()}.",
            "I can't believe I allowed these filthy monsters to steal from me! I WILL get my revenge. Kill {questSo.GetEnemyAmountString()} for me.",
            "I can't concentrate on my work with these stupid monsters around. Make yourself usefull and get rid of {questSo.GetEnemyAmountString()}.",
            "Those pests are getting on my nerves. Take out {questSo.GetEnemyAmountString()} of them, will you? I’ve got better things to do.",
            "Ugh, those {questSo.GetEnemyString()} are causing havoc again. Go deal with {questSo.GetEnemyAmountString()} of them. Don’t make me repeat myself.",
            "If I have to hear one more thing about those creatures, I’m going to lose it. Go kill {questSo.GetEnemyAmountString()} of them and be quick about it.",
            "You want to help? Fine. Get rid of {questSo.GetEnemyAmountString()} of those nuisances for me. Maybe then I’ll get some peace and quiet.",
            "Look, I’m not in the mood for excuses. Go out there and take down {questSo.GetEnemyAmountString()} of those {questSo.GetEnemyString()} before they make things worse.",
            "Those things are everywhere, and it’s driving me crazy. You’re capable, right? Go take out {questSo.GetEnemyAmountString()} of them."
            };
        }

        protected override string [] averageSocialOpeners{
            get => new string [] {
            "ATCHOOO, a-- ATCHOOO .... Oh, sorry about that. I'm actually allergic to {questSo.GetEnemyString()}. How does that work you ask? Well, you better ask my doctor instead of me... Could you actually get rid of {questSo.GetEnemyAmountString()}? It would really help me...",
            "This is a bit embarassing but I actually am scared of {questSo.GetEnemyString()}. Could you get rid of some of them for me? I guess if you killed {questSo.GetEnemyAmountString()} I would feel way safer!",
            "Did you know that a recent study showed that most of our monster related deaths are caused by {questSo.GetEnemyString()}. That sure is scary, isn't it? Could you get rid of {questSo.GetEnemyAmountString}?",
            "We’re in trouble with all these enemies around. Can you take out {questSo.GetEnemyAmountString()} of them to help us out?",
            "Those creatures are becoming a real threat. Think you could thin their numbers? {questSo.GetEnemyAmountString()} should do the trick.",
            "We need to get this area under control. Can I count on you to eliminate {questSo.GetEnemyAmountString()} of those enemies?",
            "Hey, can you lend a hand? If you take out {questSo.GetEnemyAmountString()} of those enemies, it’ll make things a lot safer for everyone.",
            "The enemy presence is getting overwhelming. Take care of {questSo.GetEnemyAmountString()} of them, and we might just stand a chance.",
            "This place won’t be safe until we deal with those creatures. Take out {questSo.GetEnemyAmountString()} of them and report back."
            };
        }

        protected override string [] highSocialOpeners{
            get => new string [] {
            "Long ago, a bunch of {questSo.GetEnemyString()} killed my mother. I'm not too found of monster hunting, but I do believe we should do something so people don't get hurt. I don't have it in me, but I'd be gratefull if you were able to take care of {questSo.GetEnemyAmountString()}.",
            "We’re in desperate need of help. The  {questSo.GetEnemyString()} must be stopped! Could you deal with {questSo.GetEnemyAmountString()} for us?",
            "Oh, thank goodness you’re here! Those pesky creatures are everywhere, and I can’t take it anymore. Could you, maybe, I don’t know, take out {questSo.GetEnemyAmountString()} of them? That would be amazing!",
            "Okay, so here’s the deal. These enemies are all over the place, causing chaos, and I’m just sitting here, helpless. Could you handle, say, {questSo.GetEnemyAmountString()} of them? I’d feel so much better!",
            "I’ve been watching these enemies wreak havoc, and, honestly, it’s exhausting just thinking about it. Could you take care of {questSo.GetEnemyAmountString()} of them? Pretty please? You’re the best!",
            "You know those enemies causing all the problems? Yeah, I need you to get rid of {questSo.GetEnemyAmountString()} of them. You’re good at that, right? Of course you are!",
            "So, there I was, thinking about how we could solve this enemy problem, and then you showed up! Perfect timing. Could you take out {questSo.GetEnemyAmountString()} of them? It’d mean the world to me.",
            "Oh my gosh, you wouldn’t believe how annoying those creatures are! If you could just deal with, oh, I don’t know, {questSo.GetEnemyAmountString()} of them, I’d be forever grateful!"
            };
        }
    }
}