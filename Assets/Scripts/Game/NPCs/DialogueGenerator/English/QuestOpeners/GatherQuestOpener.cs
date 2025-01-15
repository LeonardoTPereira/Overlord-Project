using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs
{
    public class GatherQuestOpener : QuestOpener
    {
        protected override string [] lowSocialOpeners {
            get => new string [] {
            "Gosh, I wonder how this place even got so messy in the first place. Make yourself of use and gather the {questSo.GetItemAmountString()} that are lying around.",
            "I need {questSo.GetItemAmountString()} for a spell. What spell? None of your business. You can even keep the {questSo.GetItemString()}.",
            "Great, more work for me—except I’m pawning it off on you. Go gather {questSo.GetItemAmountString()}, and don’t make me wait too long.",
            "You’re still here? Good, because I need {questSo.GetItemAmountString()}. Think you can handle that, or is it too much for you?",
            "You want to be useful? Fine, go gather {questSo.GetItemAmountString()} for me. If you’re quick about it, maybe I’ll say thanks. Maybe.",
            "Look, I don’t have the energy to explain why. Just get {questSo.GetItemAmountString()} and stop asking questions."
            };
        }

        protected override string [] averageSocialOpeners {
            get => new string [] {
            "This place is a real mess! Could you please gather the {questSo.GetItemAmountString()} that are lying around?",
            "I'm starting to study {questSo.GetItemString()}. Could you bring me {questSo.GetItemAmountString()} so I can take a look at them? You can keep them, I just want to study them for a few hours...",
            "I really miss my mom. I've been reading about this comunication spell, but I'd need {questSo.GetItemAmountString()}. You'd get that for me? Oh thanks! I don't have much to offer, but you can have the {questSo.GetItemString()} for yourself as a reward.",
            "Did you know {questSo.GetItemString()} are magical items? You can use them in many types of spells. I've been trying to analyse them but I would need {questSo.GetItemAmountString()}. You can keep them as a reward!",
            "Could you help me out? I’m looking for {questSo.GetItemAmountString()}, and you seem like the perfect person to track them down.",
            "Hey, I’m in need of {questSo.GetItemAmountString()} for a project I’m working on. Can you find them for me?",
            "Hey, you’re good at finding things, right? I need {questSo.GetItemAmountString()}—could you gather them for me when you get the chance?"
            };
        }

        protected override string [] highSocialOpeners {
            get => new string [] {
            "Oh, you know, I’ve been thinking about this for a while, and I just have to ask—could you gather {questSo.GetItemAmountString()} for me? They’re so sparkly and rare! I’d do it myself, but, well, you’re just so much better at this kind of thing!",
            "You won’t believe this, but I heard there are exactly {questSo.GetItemAmountString()} out there just waiting to be found! Would you mind collecting them for me? I mean, imagine the possibilities once we have them!",
            "You’ve got to help me with this! I need {questSo.GetItemAmountString()}, and I’ve been wracking my brain trying to figure out where to find them. But you? Oh, you’re a natural treasure hunter!",
            "Oh my gosh, you’re here! Perfect timing! I need {questSo.GetItemAmountString()}, like, urgently. Well, not urgent-urgent, but you know, soon-ish. Can you help me out?",
            "Okay, so I was thinking, wouldn’t it be amazing if we had {questSo.GetItemAmountString()}? I mean, just think about all the amazing things we could do with them! You’ll help me gather them, right?",
            "Oh, you’re here! Wonderful! I’ve got this little problem—well, it’s not a problem exactly, more of an opportunity—I need {questSo.GetItemAmountString()}, and I just know you’re the person to find them!",
            "Can I tell you a secret? I’ve been dying to get my hands on {questSo.GetItemAmountString()}! They’re just so perfect for… oh, never mind that part. Anyway, could you gather them for me? Pretty please?",
            "So, funny story! I was planning to collect {questSo.GetItemAmountString()} myself, but then I remembered, 'Oh wait, I know someone way more capable!' That’s you, by the way. Can you help me out?"
            };
        }
    }
}