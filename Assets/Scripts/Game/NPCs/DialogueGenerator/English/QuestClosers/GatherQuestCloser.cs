namespace Game.NPCs
{
    public class GatherQuestCloser : QuestCloser
    {
        protected override string [] lowSocialClosers {
            get => new string [] {
            "Huh. You found the stuff. Didn’t think you’d pull it off. Thanks, I suppose.",
            "Well, you managed to gather everything. I’ll give you credit for that. Now, leave me be.",
            "You didn’t mess it up. Here’s your ‘thanks,’ whatever that’s worth.",
            "You actually found the items... You know, I didn’t expect you to come through. So, thanks, I guess.","Took you long enough. Still, you got the items. I’ll thank you, but don’t make a habit of it.",
            "You found them all. Well, it’s better than I thought you’d do. Thanks, I guess.",
            "Alright, you did it. Not sure how, but you did. Thanks for gathering them."
            };
        }

        protected override string [] averageSocialClosers {
            get => new string [] {
            "You did it! I really appreciate you gathering all of these. This will be a big help moving forward.",
            "You’ve really outdone yourself! Thanks for getting everything I asked for—these will come in handy.",
            "Wonderful! You gathered everything I needed. I’m truly grateful for your help with this!",
            "I can’t thank you enough for this! You gathered all the items so quickly and efficiently. Great job!",
            "You really came through! Thanks for gathering everything and showing it to me. I’m sure it’ll make a difference.",
            "Fantastic work! You gathered everything I asked for, and I couldn’t be more grateful. Thank you!"
            };
        }

        protected override string [] highSocialClosers {
            get => new string [] {
            "Oh wow, you actually found them all! I’m so glad you managed to track these down! You wouldn’t believe how much easier things are going to be now. You’re a lifesaver!",
            "Look at all of this! I can’t believe you found everything—well, actually, I can believe it, knowing you! You really outdid yourself, huh? I’ll get started with this right away!",
            "Well, well, well! You did it, didn’t you? Found every single thing I asked for! You have no idea how much this helps, I can’t wait to put all of this to good use!",
            "Oh, you did find them all! I honestly didn’t think you’d pull it off, but you went and proved me wrong! This is going to be a huge help, I really can’t thank you enough!",
            "Wow, look at all this! You gathered every last one of them! That’s some serious dedication. I’m already thinking of all the things we can do with this!"
            };
        }
    }
}