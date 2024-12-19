namespace Game.NPCs
{
    public class GoToQuestCloser : QuestCloser
    {
        protected override string [] lowSocialClosers {
            get => new string [] {
            "Well, you actually made it there and back. Guess I should thank you for that, but don’t expect me to be all cheery about it.",
            "You went to those coordinates and returned in one piece. Fine, thanks, I guess. Now don’t make me ask you again.",
            "Huh. You went out there, didn’t you? Well, at least you brought something useful back. Thanks, I suppose.",
            "I didn’t think you’d actually make it, but you did. Fine, here’s your thank you—don’t let it go to your head.",
            "You did what I asked, went to the coordinates, and came back. So, uh, thanks. Don’t expect me to be impressed, though.",
            "You got the report, huh? Well, you did what you had to do. Thanks. Now, can we move on?",
            "I was half-expecting you to get lost out there, but you actually came back with something. Guess I owe you a ‘thanks,’ but don’t expect a parade.",
            "You went to the spot and actually came back with info. Fine, whatever. Thanks. That’s all I’ve got for you.",
            "I’m surprised you didn’t get stuck out there. Good job getting the report, I guess. I’ll take it from here.",
            "Well, you got the job done. Thanks for that. Just don’t make a habit of it, alright?"
            };
        }

        protected override string [] averageSocialClosers {
            get => new string [] {
            "You actually went all the way out there and came back with the info? Well done! I’m glad you could get me that report. It’s exactly what I needed!",
            "I can’t believe you went to those coordinates and made it back safely! You’ve done a fantastic job, and I appreciate the update.",
            "Thank you for taking the time to head out there and gather the details. That location is tricky, but you’ve done exactly what I asked. Great work!",
            "You went out there, didn’t you? I can’t tell you how much I appreciate you reporting back with all the information. This is going to help us a lot.",
            "You actually made it to those coordinates and came back with a full report? That’s more than I could’ve asked for—thank you so much!",
            "I knew you’d handle it, but I didn’t expect you to make it out there and back so quickly! Your report is exactly what I needed, and I’m grateful.",
            "Well, I didn’t think you’d make it to those coordinates, let alone bring back a solid report! I can’t thank you enough for going the extra mile.",
            "You went straight to the heart of it, didn’t you? I’m so glad you were able to make it to that spot and bring back the details. It’ll be so helpful for our next steps!",
            "That was quite a trek to those coordinates, but you did it! Thank you for the thorough report. I couldn’t have asked for better results.",
            "You made it there and back without any trouble? I have to hand it to you—this report is exactly what we needed. Thanks for taking the time to go and bring back all this valuable information."
            };
        }

        protected override string [] highSocialClosers {
            get => new string [] {
            "Oh wow, you actually went all the way out there? I mean, I knew you were up to the task, but I didn’t expect you to get back so quickly! I can’t tell you how helpful this information is. We’re one step closer to solving this!",
            "You made the journey to those coordinates and came back with all the details? That’s impressive! I’ll admit, I wasn’t sure you’d manage it, but here we are—thank you! This info is exactly what we needed.",
            "You did it! I can’t believe you made it out there and came back to report so fast. Honestly, I’m amazed! This is going to make a huge difference—thank you for going the extra mile!",
            "You went all the way out there, got the info, and came back—talk about dedication! You really went above and beyond, and I’m honestly grateful for the effort you put in. This is fantastic!",
            "Well, you sure didn’t waste any time! I was expecting you to take forever, but you were back before I knew it. Thank you for getting all the information from that spot, it’s just what we needed!",
            "I can hardly believe it, but you actually did it! You went out to those coordinates, gathered the details, and came back in record time! Seriously, I can’t thank you enough for this. You’re a real gem.",
            "You’ve really outdone yourself, haven’t you? I thought that place was going to be a headache, but you went and brought back exactly what we needed. Now that’s what I call efficiency. You’ve made my day, thank you!",
            "Well, well, well, look who went all the way to those coordinates and brought back the info! You must be a pro at this, huh? Thank you so much for being so thorough. You really came through for us!",
            "You did it! I was half expecting you to get lost out there, but you’ve actually come back with solid information. I can’t tell you how much this helps, thank you for putting in all that effort!",
            "You didn’t just head out to those coordinates, you nailed it! I honestly didn’t expect you to get back so soon with all the details. You’ve made my job so much easier, and for that, I’m truly grateful!"
            };
        }
    }
}