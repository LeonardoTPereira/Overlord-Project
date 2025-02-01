namespace Game.NPCs
{
    public class ReadQuestCloser : QuestCloser
    {
        protected override string [] lowSocialClosers {
            get => new string [] {
            "Hmph, I didn’t think you’d actually bother to read it. But, fine. The info on those cursed ruins? Yeah, it’s exactly what we needed. Now we can move forward. I guess… thanks.",
            "You actually read that? Not bad. The scroll on magical creatures? It had some decent info. Weaknesses, habits, that sort of thing. Anyway, thanks, I suppose.",
            "Well, you went ahead and read the whole thing. The book on ancient traps wasn’t exactly fun, was it? But, hey, at least you found what we needed. Fine, I’ll admit it, that’s useful.",
            "You read that dusty old book? Whatever, it’s fine. The bit about the old kingdom’s downfall? Yeah, it’ll help. Now we know who to blame for all the mess. Thanks, I guess.",
            "So, you went through that whole thing, huh? The scroll on dark magic? Yeah, it wasn’t totally useless. At least we know a bit more about what we’re up against now. Thanks, I suppose.",
            "I didn’t think you’d have the patience, but you did. The book on enchanted artifacts? It had some decent points. Now we’ve got a lead. Don’t get used to hearing thanks from me, though.",
            "Huh. You actually made it through that? The scroll on the old legends—well, it’s got the details we need. Not that I’m surprised, but, uh, thanks.",
            "So you read it, huh? I wasn’t expecting that. The history of the magic wards? It’s useful. We know more now, at least. Don’t expect me to be all cheery about it, though.",
            "Well, color me surprised. You actually read that ancient book. The part about the haunted ruins? That’s what we needed. I’ll give you that, fine. Thanks.",
            "You went ahead and read the scroll on curses, huh? Didn’t think you’d be up for it. But you came back with some solid info on their weaknesses. I guess I owe you a ‘thanks’ for that."
            };
        }

        protected override string [] averageSocialClosers {
            get => new string [] {
            "You really read through all that? Impressive! The book's details on the ancient rituals are exactly what I was looking for. This will make our next move much easier—thanks for your effort!",
            "Ah, you actually read it! The scroll on magical creatures had some fascinating details, didn’t it? Now we know more about their weaknesses. I can’t thank you enough for this.",
            "Well done! The book you read about the lost city was full of useful information. We now have the exact location of the temple ruins. You've saved us so much time—thank you!",
            "I can’t believe you made it through that whole scroll! The history of the enchanted artifacts was exactly what we needed. We’ll be able to track them down much quicker now. Great work!",
            "You really tackled that book, didn’t you? The chapter on the old king’s treasure—just what we needed. You’ve made this search a whole lot easier, and I’m grateful for that!",
            "Wow, you actually read that entire scroll! The information on the dark magic is incredibly detailed. I can’t tell you how valuable this is. You've done us a huge favor!",
            "You actually read that? I thought it would be too much for you! But the details on the ancient rituals were exactly what we needed. We’ll have a much clearer plan going forward, all thanks to you.",
            "That was a lot of reading, but you did it! The information on the cursed ruins is a game-changer. You've given us the edge we needed to understand what’s out there. Thanks so much!",
            "You didn’t just skim through it, you read the whole thing? Amazing! The insights about the old battles are going to help us tremendously. You've really pulled us ahead in this quest.",
            "I didn’t expect you to go through all that, but you did! The book on the magical wards is full of useful details. We’re one step closer to understanding the mystery. Thanks for taking the time to read it!"
            };
        }

        protected override string [] highSocialClosers {
            get => new string [] {
            "Ah, so you actually read it! I was wondering what you'd find in there. The information on ancient artifacts is exactly what I needed—thank you for going through all that trouble!",
            "Oh wow, you actually went and read it! The scroll’s history of the old kingdom? Fascinating stuff, right? I had a feeling it would shed some light on our next move. I really appreciate it!",
            "You went through the whole thing, huh? Well, I’m glad you did, because that book about the lost city holds some real gems. It’s all in there—maps, details, everything we need. Great job!",
            "I’m impressed! You read the scroll and got through the fine print. The details about the ancient rituals? Exactly what I was hoping to find. Thanks for taking the time!",
            "You actually made it through that book? Impressive! The sections about the forbidden magic were just as I expected—helpful, but risky. I appreciate you diving into that for us.",
            "You’ve got the patience of a saint, I’ll give you that. You read through that scroll and came back with info on the enchanted beasts? Just what we needed, and you’ve saved me hours of research. Thanks!",
            "Well, look at you! You didn’t just skim it—you read the whole thing! That text about the lost artifacts? Now we know where to start our search. You’ve really helped us out, thank you!",
            "Wow, you actually tackled that book on the ancient ruins. I thought it’d be a lot to digest, but you took the important details right out of it. The information on the magical wards is exactly what we needed. Thank you!",
            "You went through the scroll on the dark magic, didn’t you? And came back with all the key points. I have to admit, that was no small feat. But now we know what we’re dealing with. Thanks so much for that!",
            "You read that entire book? Wow, I didn’t expect you to get through it so fast! The part about the sacred artifacts—fantastic find. We’ve got a solid lead now, and I owe it to you. Thanks!"
            };
        }
    }
}