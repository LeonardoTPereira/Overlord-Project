namespace Game.NPCs
{
    public class ReportQuestCheckPoint : QuestDialogue
    {
        protected override string [] lowSocialDialogues {
            get => new string [] {
            "Ugh, so you actually did it... Well, I guess the other NPC knows now. Don’t expect me to be all grateful, but... fine, thanks.",
            "Took you long enough, huh? At least the info’s with them now. I didn’t think you'd follow through, but here we are.",
            "Well, it’s done. You passed the message. I’ll give you credit where it’s due... just don’t expect me to cheer for you.",
            "Great, now that they know, I can move on. Thanks... I guess.",
            "I don’t know why you’re looking for praise. You did what was asked, and now we can move on. Whatever, thanks.",
            "You did it, and they’ve got the info. Don’t get used to hearing ‘thank you’ from me though.",
            "I guess it’s done. At least someone’s finally getting the message. You’re welcome for the easy task, I suppose.",
            "You didn’t mess it up... for once. I suppose that’s worth something. Thanks, but don’t expect a parade.",
            "Well, that was quick. I don’t know if you deserve a medal, but here’s a thanks anyway.",
            "You did your job. They have the information now. That’s all I can say."
            };
        }

        protected override string [] averageSocialDialogues {
            get => new string [] {
            "Thanks for passing that along to them. I’m sure it’ll make things easier for everyone. Nice work!",
            "Good job! They’ve got the info now. I appreciate you taking the time to report it back.",
            "Thanks for handling that. I’m sure they’ll appreciate the details. You've been a real help.",
            "Well, that was quick! Thanks for making sure they got the information. You’ve done good work.",
            "Nice work, you’ve done exactly what was needed. They have the info now, so we can move on.",
            "Appreciate you passing along that message. Now they’ve got the info, and we can move ahead.",
            "Great job, they’ve got what they need. I’m sure that’ll help a lot moving forward. Thanks!",
            "Thanks for taking care of that. Now that they know, things should go a lot smoother. Well done!",
            "Nice and simple, huh? Thanks for getting the information where it needed to go. It’s much appreciated.",
            "You did exactly what was needed. I’m sure they’ll find it helpful. Thanks for taking care of it."
            };
        }

        protected override string [] highSocialDialogues {
            get => new string [] {
            "Oh, fantastic! You’ve really done us a solid, getting that info to them. I’m sure they’ll be thrilled to know what you’ve uncovered. Great job!",
            "Well, look at you! Passing along important info like that—now they can get to work on it. I’m sure it’ll help a ton. Thanks a million!",
            "You didn’t waste any time, did you? I’m sure they’ll be so relieved to have all the details. You’re really making a difference around here!",
            "Now that the info’s been passed along, things should go much smoother. You’ve saved everyone a lot of time, and we’re all grateful for it. Thank you!",
            "Oh, you’re really on top of it! I’m sure they’re going to love hearing what you brought back. That’ll make everything so much easier. Big thanks!",
            "You’ve done great! Now the other NPC has all the info they need. This is going to speed things up quite a bit, I’m sure. Thanks for taking care of it!",
            "Excellent work! The other NPC will be over the moon to hear all the details you brought. This info will really change things around here!",
            "I can already imagine their face when they get this news! You’ve really done a great service, and I can’t thank you enough. You’re a real lifesaver!",
            "Now we can finally get moving! The info you gave them will make all the difference. I’m so glad you’re on our side—thank you!",
            "Ah, you’ve really done it now! Passing the info along means we can finally make some progress. You’ve been invaluable in getting everything on track!"
            };
        }
    }
}