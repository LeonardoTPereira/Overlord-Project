using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs
{
    public static class DialogueGreetings
    {
        static string [] lowSocialGreeting = {
            "...",
            "UGH, What do you want?",
            "UGH, It's you again... What do you mean we haven't met?",
            "Can't I have a single moment of peace?",
            "I really don't want to talk right now.",
            "Don't you dare call me grumpy!",
            "The wheather today is terrible...",
            "I don't want no small talk.",
            "Get out of here.",
            "Do I look like someone who'd like to be talking to you right now?",
            "I'm busy.",
            "Don't you have anything else to do?",
            "Don't you have anywhere else to be?",
            "Just another bad day...",
            "UGH, \"ADVENTURERS\" these days....",
            "Did you loose something in my face? No? Then why are you staring me?",
            "What are you still doing here?",
            "What?",
            "Pff, don't waste my time.",
            "What even ARE you?"
        };

        static string [] lowSocialIntro = {
            " I'm {speaker.NpcName}, the {speaker.Job}.",
            " Who am I? Of course, {speaker.NpcName}, the {speaker.Job}. Who else did you think I could be?",
            " My name? {speaker.NpcName}. You also want to know my job? What's next my social security number? My credit card info??",
            " I'm a {speaker.Job} for a living. So if you ever want anything related to that you come to me.",
            " ... Fine, I'll tell you about me. I'm {speaker.NpcName}, the {speaker.Job}.",
            " You should consider yourself lucky to so much as breathe the same air as {speaker.NpcName}, the {speaker.Job}... Which is me... ",
            " I have quite a lot do to as the {speaker.Job}. People sometimes also call me {speaker.NpcName}.",
            "I'm {speaker.NpcName}, the best {speaker.Job} there is to find, and I don't like wasting my time."
        };

        static string [] averageSocialGreeting = {
            "Heyy!",
            "Hello! How are you?",
            "It's not very common to see adventurers around here.",
            "Have you figured out how to leave here yet?",
            "Sometimes I miss living in the city.",
            "I haven't seen you in a while. What have you been up to?",
            "I'm not too busy, I can talk for a bit.",
            "I'm all ears!",
            "What's going on lately?",
            "Talk to me.",
            "How can I help you?",
            "Just another day.",
            "If you ever figure out how to get to the forest, do tell me.",
            "Have you seen other people around?",
            "I haven't always lived in a dungeon, you know?",
            "I used to live in a big city...",
            "Are there more of your kind?",
            "ATCHOO!! Ops, sorry about that...",
            "You look just like somebody that I used to know!"
        };

        static string [] averageSocialIntro = {
            " I'm {speaker.NpcName}, the {speaker.Job}.",
            " My name is {speaker.NpcName} and I work as a {speaker.Job}. Not totally set on that profession though. Who knows what the future holds?",
            " You can call me {speaker.NpcName}. I've been working as {speaker.Job} for as long as I can remember.",
            "People around here call me {speaker.NpcName}, and look for me whenever they need a {speaker.Job} "
        };

        static string [] highSocialGreeting = {
            "I'm so glad to see you!",
            "Don't worry, I'm not busy at all!",
            "Some people can get a bit grumpy sometimes, but we can't blame them.",
            "Oh yes, I LOVE living in this dungeon.",
            "Today is a great day.",
            "The weather is just fine.",
            "Nice to meet you!",
            "YIPEE, a new friend!",
            "I always open up a smile whenever I see an adventurer.",
            "Happy thoughts, happy life!",
            "I hope you are having an amazing day.",
            "Have you done something nice to someone today?",
            "Remember to drink water! Even if you are stuck in a dungeon, hehe.",
            "Have you ever heard about this \"JoJo\" person? I think they might be popular around here...",
            "How is your day going?",
            "Could you imagine defeating a huge boss just to figure out the princess is in another castle? PHEW....",
            "I don't think people ask this anymore, but what's your favorite color?",
            "You seem like a nice person.",
            "What made you decide to be an adventurer?"
        };

        static string [] highSocialIntro = {
            "I'm {speaker.NpcName}, the {speaker.Job}.",
            "I really enjoy working as a {speaker.Job}, but it hurts me a bit when people call me by my title instead of my name. {speaker.NpcName} isn't so hard, is it?",
            "If you ever need anything from the {speaker.Job}, just talk to {speaker.NpcName}. That's ME!",
            "My name? {speaker.NpcName} My job? {speaker.Job}. My credit card info? 3345-- Wait no.",
            "My name? {speaker.NpcName} My job? {speaker.Job}. My favorite pokemon? Meowth, that's right!"
        };

        public static string CreateGreeting(NpcSo speaker)
        {
            switch (speaker.SocialFactor)
            {
                case < 3:
                    return GetGreetingAndIntro( lowSocialGreeting, lowSocialIntro, speaker );
                case < 5:
                    return GetGreetingAndIntro( averageSocialGreeting, averageSocialIntro, speaker );
                default:
                    return GetGreetingAndIntro( highSocialGreeting, highSocialIntro, speaker );
            }
        }

        private static string GetGreetingAndIntro( string[] greeting, string[] intro, NpcSo speaker )
        {
            var greetingAndIntro = new StringBuilder();
            int randomGreeting = Random.Range( 0, greeting.Length );
            int randomIntro = Random.Range( 0, intro.Length );

            greetingAndIntro.Append( greeting[randomGreeting] );
            greetingAndIntro.Append( 
                intro[randomIntro].Replace( "{speaker.NpcName}", speaker.NpcName ).Replace( "{speaker.Job}", speaker.Job.ToString() )
                );

            return greetingAndIntro.ToString();
        }
    }
}