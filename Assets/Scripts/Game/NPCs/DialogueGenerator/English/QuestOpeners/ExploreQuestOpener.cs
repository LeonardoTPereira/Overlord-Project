using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs
{
    public class ExploreQuestOpener : QuestOpener
    {
        protected override string [] lowSocialOpeners{
            get => new string [] {
            "It's been a while since I left my duties here as a {questSo.Npc.Job}. I don't even know if this dungeon even has {questSo.GetRoomAmount()} rooms. ... Could you confirm that?",
            "Great, another thing I can’t do myself. Go search {questSo.GetRoomAmount()} rooms in this dungeon and let me know what’s there.",
            "I need someone to investigate {questSo.GetRoomAmount()} rooms, and lucky you, you’re the only one available. Get to it.",
            "There are {questSo.GetRoomAmount()} rooms that need looking into, and I’m too busy for it. So guess what? It’s your problem now.",
            "I don’t feel like dealing with it myself, so you’re up. Go check out {questSo.GetRoomAmount()} rooms and report back. Rumors say this dungeon changes everytime you enter it..."
        };
        }

        protected override string [] averageSocialOpeners{
            get => new string [] {
            "My best friend used to be a cartographer. They are going to visit me soon and I was thinking about getting them a little surprise. Could you help? I wanted to make a map for us to explore together, but first I need to confirm the size. Could you check out if there are at least {questSo.GetRoomAmount()} rooms in this place?",
            "Sometimes I wonder how it'd be like to be a cartographer. Maybe I should try making a map for myself. I don't know if {questSo.GetRoomAmount()} would be too big of a map... Could you explore {questSo.GetRoomAmount()} rooms and tell me how they are so I can start working on my map?",
            "If you’ve got the time, could you explore {questSo.GetRoomAmount()} rooms in the area? I’d feel a lot better knowing what’s in there.",
            "I need someone to explore {questSo.GetRoomAmount()} rooms over there. Who knows what you’ll find, but it’s worth a look.",
            "Hey, could you scout through {questSo.GetRoomAmount()} rooms in this dungeon? There might be something useful in there."
        };
        }

        protected override string [] highSocialOpeners {
            get => new string [] {
            "Hey, could you do me a tiny favor and explore {questSo.GetRoomAmount()} rooms in this area? I’d go myself, but who knows what’s in there! I’d just panic!",
            "So, funny story—I’ve been meaning to look into those {questSo.GetRoomAmount()} rooms, but something always comes up! Can you handle it for me? I’m sure they’re fascinating!",
            "I know it’s a bit much to ask, but could you explore {questSo.GetRoomAmount()} rooms for me? It’s just that I’ve been wondering about them forever and can’t stand the suspense!",
            "You’re the perfect person for this! Could you check out {questSo.GetRoomAmount()} rooms in this dungeon? I just know there’s something amazing waiting to be found!",
            "Oh, this is so exciting! I need you to explore {questSo.GetRoomAmount()} rooms for me—it’s just too mysterious to ignore, and I have to know what’s in there!"
        };
        }
    }
}