using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs
{
    public class ReadQuestOpener : QuestOpener
    {
        protected override string [] lowSocialOpeners {
            get => new string [] {
                "There’s a book out there you need to find. Don’t ask me where—just get it and read it.",
                "I’m not your librarian, but there’s a scroll you need to dig up and read. Go find it already.",
                "If you want answers, there’s a book you’ll need to track down and actually read. Yes, read. Got it?",
                "Ugh, why do I have to spell everything out? Find the scroll and read it yourself. It’s not my problem.",
                "There’s a book somewhere that explains everything you need. Go find it, read it, and stop bothering me.",
                "You’re looking for a scroll. It’s important. Once you find it, read it—assuming you can read."
        };
        }

        protected override string [] averageSocialOpeners{
            get => new string [] {
            "Did you know that around this dungeon there are magical books and scripts lying around? I've heard about one just near here. Could you tell me what's in it?",
            "I'm trying to master this new magical technique and I've heard there's a book with it. If you find it, could you tell me what it says?",
            "There’s a book that holds the knowledge we need. Can you find it and read through its pages for me?",
            "I’ve heard of a scroll with answers to our problems. Can you locate it and see what it says?",
            "We’re missing a key piece of information. There’s a book out there—find it, read it, and let me know what you discover.",
            "There’s a scroll rumored to be in the ruins. If you can find it and read it, it might just hold the key to solving this.",
            "I need someone with sharp eyes and sharper wits. Can you find a certain book and read it carefully? It’s vital.",
            "Legends speak of a scroll hidden in the library archives. Find it, read it, and bring its secrets back to me.",
            "There’s an ancient book containing the answers we seek. Could you locate it and see what wisdom it holds?",
            "Somewhere out there is a scroll with the information we need. Please find it, read it, and return with what you’ve learned.",
            "There’s an old book that holds the truth we’ve been searching for. If you can find it and read it, we’ll be one step closer.",
            "The answers lie in a scroll tucked away somewhere. Can you track it down, read it, and tell me what it says?"
        };
        }

        protected override string [] highSocialOpeners{
            get => new string [] {
                "Oh! I just remembered, there’s this fascinating book—I think it’s tucked away in the library—or maybe the old ruins? Anyway, you have to find it and read it! It’s really important!",
                "So, there’s this scroll, ancient and mysterious, and it’s said to contain secrets no one has ever fully understood! Can you find it and read it for me? I can’t wait to hear what it says!",
                "Alright, listen! There’s a book out there, full of knowledge and possibly a few surprises. You should find it and read every last word—then come back and tell me all about it, of course!",
                "You’re going to love this! Somewhere out there is a scroll that holds the answers we’ve been looking for! Can you find it? Oh, and make sure you read it carefully—don’t miss a thing!",
                "I heard a rumor about an old book hidden in the ruins. It’s supposed to be super important! Can you go find it, read it, and tell me everything? I mean everything!",
                "Oh, this is exciting! There’s a scroll we absolutely need—it might be hidden, or dusty, or ancient! Go find it, give it a good read, and let me know what it says. I’m dying to know!",
                "So, there’s this book, and it’s kind of a big deal. Full of wisdom, secrets, maybe some spells? I don’t know! But you have to find it, read it, and come back with all the juicy details!",
                "Okay, picture this: an old scroll, hidden away, containing vital information for us. Can you track it down, read it, and give me a full report? I’ll be waiting eagerly!",
                "Oh, this is thrilling! There’s a book somewhere out there, full of mysterious insights. I need you to find it, read it, and tell me all about it. Don’t leave out a single word!",
                "There’s a scroll out there that’s absolutely critical to our mission—or maybe it’s just really interesting! Either way, can you find it, read it, and then come back and tell me everything?"
        };
        }
    }
}