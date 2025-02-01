namespace Game.NPCs
{
    public class ListenQuestCloser : QuestCloser
    {
        protected override string [] lowSocialClosers {
            get => new string [] {
            "Alright, fine… thanks for listening to {questSo.GetTargetNpc()}. Saves me the trouble, I guess.",
            "Hmph. I suppose I should thank you for dealing with {questSo.GetTargetNpc()}. They do go on, don't they?",
            "Well, you actually listened to [NPC Name?] Guess I owe you a thanks for that.",
            "You actually stuck around and listened to {questSo.GetTargetNpc()}? You’re more patient than I am. Thanks, I guess.",
            "Alright, thanks for taking the time with {questSo.GetTargetNpc()}. You did me a favor, whether you know it or not.",
            "I guess I should thank you for hearing out {questSo.GetTargetNpc()}. They would've kept bothering me until someone did.",
            "I don’t say this often, but… thanks. Listening to {questSo.GetTargetNpc()} must have taken some patience.",
            "I'm glad someone finally humored {questSo.GetTargetNpc()}. Now maybe they’ll stop pestering me..."
            };
        }

        protected override string [] averageSocialClosers {
            get => new string [] {
            "Thanks for listening to {questSo.GetTargetNpc()}. They needed that, and I know it made a difference.",
            "It means a great deal that you listened to {questSo.GetTargetNpc()}. Not many would take the time.",
            "Thank you for showing {questSo.GetTargetNpc()} such patience. They needed someone like you to hear them.",
            "I’m truly grateful that you spoke with {questSo.GetTargetNpc()}. They had so much to share, and you listened.",
            "Thank you for lending an ear to {questSo.GetTargetNpc()}. I know they feel heard because of you.",
            "I really appreciate you taking the time with {questSo.GetTargetNpc()}. You’ve lifted their spirits.",
            "Thank you. Listening to {questSo.GetTargetNpc()} wasn’t just kind—it was exactly what they needed."
            };
        }

        protected override string [] highSocialClosers {
            get => new string [] {
            "Oh, thank you so much for listening to {questSo.GetTargetNpc()}! They have the most fascinating stories, don’t they?",
            "You listened to {questSo.GetTargetNpc()}? Wonderful! Aren’t they just a treasure trove of information?",
            "Thank you, thank you! {questSo.GetTargetNpc()} has such interesting things to say, and I knew you’d appreciate it!",
            "Ah, I knew you’d listen to {questSo.GetTargetNpc()}! Isn’t it great to hear their side of things? Thank you for indulging them!",
            "Thanks a bunch for hearing out {questSo.GetTargetNpc()}! They always have the best insights. Did they tell you about the time when…?",
            "Thank you! {questSo.GetTargetNpc()} has so much to say, and I just knew you’d be the one to listen!",
            "Oh, thanks for listening to {questSo.GetTargetNpc()}! I could talk to them all day, and now I know you could too!",
            "Thank you so much! {questSo.GetTargetNpc()} always has such wonderful things to share. Did they tell you about the latest news?"
            };
        }
    }
}