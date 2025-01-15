using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs
{
    public class GoToQuestOpener : QuestOpener
    {
        protected override string [] lowSocialOpeners {
            get => new string [] {
                "Ugh, that area’s been a headache for me. Around {questSo.GetRoomCoordinates()}? Go check it out and see what’s causing all the fuss, would you?",
                "That place at {questSo.GetRoomCoordinates()} hasn’t been explored yet, and I’m too busy to deal with it. Guess that means it’s your problem now.",
                "I don’t care how you do it, but get over at {questSo.GetRoomCoordinates()} and see what’s there. Just don’t make it my issue later.",
                "That unexplored zone at {questSo.GetRoomCoordinates()}? Yeah, it’s been bothering me. Go look around and let me know if it’s worth my time.",
                "If you’re not too busy standing around, maybe you could actually explore over at {questSo.GetRoomCoordinates()} for me. Someone’s gotta do it.",
                "The room at {questSo.GetRoomCoordinates()} is an eyesore on my plans. Go figure out what’s going on there so I can stop worrying about it."
            };
        }

        protected override string [] averageSocialOpeners{
            get => new string [] {
            "I'm considering entering a painting contest, but I'm still deciding what to paint. I've heard there's a beautifull place in the coordinates {questSo.GetRoomCoordinates()}. Can you go there and check it out for me?",
            "Would you mind checking out that an area for me? I’ve heard there’s something interesting out there in the coordinates {questSo.GetRoomCoordinates()}, but I can’t go myself.",
            "There’s a place not far from here I’d like you to explore. In the coordinates {questSo.GetRoomCoordinates()}. Who knows what you might find, but it could be important.",
            "Could you head over to the coordinates {questSo.GetRoomCoordinates()} for me? I’ve got a hunch there’s something worth your time over there.",
            "I need someone to investigate the coordinates {questSo.GetRoomCoordinates()}. You seem capable, so if you’re up for it, could you explore it and let me know what you find?",
            "I’ve been curious about what’s beyond room {questSo.GetRoomCoordinates()}. Maybe you could take a look and see what’s going on over there?",
            "There’s an area up ahead that’s been untouched for a while. Ever heard of room {questSo.GetRoomCoordinates()}? Could you explore it and see if anything stands out?",
            "If you have some free time, could you go explore the coordinates {questSo.GetRoomCoordinates()}? I’ve heard rumors about strange things happening there.",
            "I’d appreciate it if you could take a trip to the coordinates {questSo.GetRoomCoordinates()}. I have a feeling there’s something there that could be of use.",
            "Take a look at the coordinates {questSo.GetRoomCoordinates()} when you get the chance. I’ve got a bad feeling about it, and I’d like you to check it out.",
            "I can’t go out there myself, but you seem like the adventurous type. Would you be willing to explore the coordinates {questSo.GetRoomCoordinates()} for me?"
        };
        }

        protected override string [] highSocialOpeners{
            get => new string [] {
                "Oh, you’re here! Fantastic! So, there’s this spot at {questSo.GetRoomCoordinates()} that’s been tickling my curiosity for ages. Could you check it out? I’d go myself, but, you know, reasons!",
                "Hey, I’ve been staring at this map forever, and there’s something intriguing about {questSo.GetRoomCoordinates()}. Could you explore it for me? Oh, and don’t forget to take notes—I love details!",
                "Okay, so here’s the deal—I found these coordinates, {questSo.GetRoomCoordinates()}, and I have to know what’s there. You’ll check it out, right? Please? Pretty please?",
                "You’re just the person I was hoping to see! There’s a location at {questSo.GetRoomCoordinates()} that’s been bugging me. What if it’s treasure? Or something mysterious? Could you investigate?",
                "So, you won’t believe this, but I’ve heard rumors about {questSo.GetRoomCoordinates()}. Weird ones! Can you go and see what’s up? I’ll be waiting here, dying to know what you find!",
                "Okay, so I’ve been marking places on this map, and {questSo.GetRoomCoordinates()} just screams ‘adventure!’ Could you explore it? I’ll owe you big time—seriously!",
                "Oh, you have to help me! There’s a spot at {questSo.GetRoomCoordinates()} that’s been haunting my dreams. Okay, not literally, but I’m so curious. Can you check it out for me?",
                "I’ve got a feeling about {questSo.GetRoomCoordinates()}. Don’t ask why—it’s just a hunch! Could you take a look and see if my instincts are onto something?",
                "Oh, I’ve got the perfect task for you! There’s something special at {questSo.GetRoomCoordinates()}. Well, I think it’s special, but that’s where you come in—go explore it, will you?",
                "So, funny story! I stumbled upon these coordinates, {questSo.GetRoomCoordinates()}, and I just know there’s something fascinating there. Can you go check it out and let me know? I’m so excited to hear what you find!"
        };
        }
    }
}