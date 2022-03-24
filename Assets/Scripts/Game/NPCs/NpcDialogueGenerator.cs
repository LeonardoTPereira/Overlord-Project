using System.Text;

namespace Game.NPCs
{
    public static class NpcDialogueGenerator
    {
        public static string CreateGreeting(NpcSo speaker)
        {
            var greeting = new StringBuilder();
            if(speaker.SocialFactor < 3)
            {
                greeting.Append("...");
            }
            else if (speaker.SocialFactor < 5)
            {
                greeting.Append("Hey,");
            }
            else if (speaker.SocialFactor < 8)
            {
                greeting.Append("Hello,");
            }
            else
            {
                greeting.Append(speaker.ViolenceFactor < 8 ? "What a pleasure," : "What do you want with me?");
            }
            greeting.Append(" I'm "+speaker.NpcName+", the "+speaker.Job+".");
            return greeting.ToString();
        }
    }
}