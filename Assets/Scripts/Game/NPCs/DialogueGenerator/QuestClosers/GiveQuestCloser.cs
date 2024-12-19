namespace Game.NPCs
{
    public class GiveQuestCloser : QuestCloser
    {
        protected override string [] lowSocialClosers {
            get => new string [] {
            "Well, you actually did it. You found the item and gave it to them. I guess that’s worth a ‘thanks.’",
            "Huh. You went through all that trouble and actually delivered the item. Fine, I suppose I should thank you.",
            "I can’t believe you actually got it done. Thanks, I guess, for handing it over to them.",
            "You managed to do what I asked. Here’s your thank you—don’t expect much more from me.",
            "Not bad. You found the item and got it to them. Fine, you’ve got my thanks, but that’s it.",
            "You really went and did it, huh? Well, thanks for the delivery. Don’t expect me to be all cheerful about it.",
            "You got the item and handed it over. Alright, I’ll admit, I’m thankful... just don’t make it a habit.",
            "I didn’t think you’d manage it, but you did. Thanks for getting that item to them, I guess.",
            "You really did it. I suppose I should say thanks for passing it along, but don’t expect any more praise from me.",
            "Well, you came through. Thanks for delivering the item to them—just don’t make me ask again."
            };
        }

        protected override string [] averageSocialClosers {
            get => new string [] {
            "Thank you for gathering that item and delivering it to them. I can’t tell you how much this will help.",
            "I really appreciate you handling that for me. Getting that item to them was no small task. You’ve done well!",
            "Great work! I’m glad you managed to find the item and get it to them. It’s exactly what we needed.",
            "You’ve really come through for us! Thanks for retrieving the item and making sure it got to the right person.",
            "Thank you so much for your efforts! Getting that item into their hands means a lot to us. You’ve been a real help.",
            "I knew I could count on you! Thanks for not only gathering the item but also getting it delivered to them so quickly.",
            "Well done! You went the extra mile to make sure the item reached the right person. Your efforts are truly appreciated.",
            "Thanks for your dedication in gathering that item and passing it along. You've really made a difference with that task.",
            "You handled that task perfectly! Thanks for not only collecting the item but also ensuring it reached the right person.",
            "I can’t thank you enough for gathering that item and delivering it to them. It’s exactly what they needed, and you made it happen!"
            };
        }

        protected override string [] highSocialClosers {
            get => new string [] {
            "Wow, you actually did it! You gathered the item and delivered it, no less! I mean, I knew you had it in you, but still! That was quite the task, and I’m so grateful you made it happen!",
            "Oh my goodness, you went above and beyond, didn’t you? Not only did you gather the item, but you actually got it to them! You’re making everyone’s life so much easier around here. Thank you so much!",
            "You did it! You really did! Gathering that item wasn’t easy, but then making sure it got to the right person? I’m impressed. Seriously, you’ve earned my gratitude ten times over for this one!",
            "Oh, what a relief! You’ve got the item and you’ve given it to them—finally! I’m just so happy you were able to handle it. That task was way too much for me, but you took care of it without a hitch!",
            "Can you believe it? You gathered the item and actually delivered it, too! I’d still be stuck here trying to figure out how to get that to them if it weren’t for you. Thank you, thank you, thank you!",
            "Well, look at you go! I knew you could handle it, but you really nailed it. Not only did you find the item, but you made sure it got to the right person—talk about dedication! I appreciate it more than you know.",
            "I have to say, I’m impressed! You found the item and got it to them without missing a beat. Now, I don’t know what I would’ve done without you. You’ve made this whole process a lot smoother, and for that, I’m truly grateful!",
            "Oh, wow, you did it! I’m honestly amazed! You didn’t just gather the item, you actually made sure it got to the right hands. You’ve done a fantastic job, and I can’t thank you enough for your help.",
            "Look at you, always getting the job done! You gathered the item and even made sure it reached the right person without a hitch. Honestly, I’m just so grateful for all the hard work you’ve put into this. You’ve made everything so much easier!",
            "You really knocked this one out of the park! Gathering the item and delivering it without a problem? That’s impressive! I can’t thank you enough for handling this. Things are going to go a lot more smoothly thanks to you."
            };
        }
    }
}