namespace Game.NPCs
{
    //TODO
    public class ExchangeQuestCheckPoint : QuestDialogue
    {
        protected override string[] lowSocialDialogues
        {
            get => new string[] {
            "Hmph. I guess I should thank you for running that errand. You got the reward, right? Don’t expect any more favors from me.",
            "Well, you actually did it. Fine, thanks for handling that trade. I suppose you got something decent from {questSo.GetTargetNpc()}, didn’t you?",
            "Yeah, yeah, thanks for doing the trade with {questSo.GetTargetNpc()}. I wasn’t expecting you to actually follow through, but here we are.",
            "Whatever, you got the job done. Thanks, I guess. Was the reward any good, or just more junk?",
            "I didn’t think you'd actually go through with it, but you did. Thanks, I guess. Did {questSo.GetTargetNpc()} give you something worth the trouble?",
            "So, you made the trade. Fine, you did what I asked. Thanks, I suppose. Hope {questSo.GetTargetNpc()} gave you something halfway decent.",
            "Well, that’s one less thing for me to worry about. Nice job doing the trade, though I’m sure you didn’t do it just for me.",
            "I’ll admit, I’m surprised you followed through. Thanks, but don’t get too excited. Did {questSo.GetTargetNpc()} give you anything useful?",
            "Ugh. You actually went and did it. Thanks for the trade. You probably got something good out of it, right?",
            "Huh. Didn’t expect you to go all the way. I won't give you anything other than my thanks. Hope you’re already happy enough with the reward {questSo.GetTargetNpc()} gave you."
            };
        }

        protected override string[] averageSocialDialogues
        {
            get => new string[] {
            "Thank you for handling that trade! {questSo.GetTargetNpc()} always gives good rewards, and I’m sure you’ve earned it.",
            "I appreciate you taking care of that! {questSo.GetTargetNpc()} isn’t the always easiest to deal with, but I bet their reward was worth it.",
            "Thanks a lot for making the trade! I’m sure you got something great out of it. {questSo.GetTargetNpc()} always keeps their promises.",
            "You’ve really helped me out. Thanks for taking that to {questSo.GetTargetNpc()}. I’m sure their reward was well worth the effort.",
            "That was a huge favor, and I can’t thank you enough! I hope {questSo.GetTargetNpc()} gave you a reward that made it all worthwhile.",
            "You did exactly what I needed, and I appreciate it. I’m sure {questSo.GetTargetNpc()} didn’t disappoint with the reward!",
            "Thank you so much for going through with the trade! {questSo.GetTargetNpc()} always has something special for those who help."
            };
        }

        protected override string[] highSocialDialogues
        {
            get => new string[] {
            "You’re a gem! Thanks for helping out with that trade. {questSo.GetTargetNpc()} has the best rewards, doesn't they? I hope you got something incredible!",
            "I can’t thank you enough for doing that! {questSo.GetTargetNpc()} sure knows how to make a deal, and I’m sure they gave you something that’s worth your time!",
            "Wow, you actually went through with it! Thank you for taking that over to {questSo.GetTargetNpc()}. I bet their reward was just as amazing as I said, huh?",
            "Oh, thank you so much for handling that trade! I just knew {questSo.GetTargetNpc()} would give you something great in return! You're a real lifesaver!",
            "You did it! You made the trade with {questSo.GetTargetNpc()}! Thank you, thank you! I hope their reward was everything you hoped for and more!",
            "You went all the way for me! Thanks a ton for handling the trade with {questSo.GetTargetNpc()}. I’m sure they gave you a reward that made it all worthwhile!",
            "I knew I could count on you! Thanks for taking the time to trade with {questSo.GetTargetNpc()}. I’m sure their reward was as good as gold, wasn’t it?",
            "You’re too kind! Thanks for dealing with {questSo.GetTargetNpc()}. They always have the best rewards, and I bet you got something really special!",
            "Thank you for running that errand for me! I hope {questSo.GetTargetNpc()}’s reward was just as amazing as I promised. You make these things look easy!",
            "I really appreciate you handling the trade with {questSo.GetTargetNpc()}! I’m sure their reward was totally worth it—you’ve got a good eye for these things!"
            };
        }
    }
}