using Game.GameManager;
namespace Game.NPCs
{
    public class ListenQuestCloser : QuestDialogue
    {
        protected override string[] lowSocialDialogues
        {
            get
            {
                if (_language == Util.Enums.Language.Portuguese)
                    return new string[] {
                    "Certo, tudo bem... obrigado por ouvir {questSo.GetTargetNpc()}. Isso me economiza o trabalho, eu acho.",
                    "Hmph. Acho que devo te agradecer por lidar com {questSo.GetTargetNpc()}. {questSo.GetTargetNpc()} realmente fala demais, não?",
                    "Bom, você realmente ouviu {questSo.GetTargetNpc()}? Acho que devo te agradecer por isso.",
                    "Você realmente ficou e ouviu {questSo.GetTargetNpc()}? Você tem mais paciência do que eu. Obrigado, eu acho.",
                    "Certo, obrigado por dedicar seu tempo a {questSo.GetTargetNpc()}. Você me fez um favor, quer saiba disso ou não.",
                    "Acho que devo te agradecer por ouvir {questSo.GetTargetNpc()}. {questSo.GetTargetNpc()} teria continuado me incomodando até alguém fazer isso.",
                    "Não costumo dizer isso, mas... obrigado. Ouvir {questSo.GetTargetNpc()} deve ter exigido paciência.",
                    "Estou feliz que alguém finalmente tenha dado atenção a {questSo.GetTargetNpc()}. Agora talvez eu finalmente tenha algum sossego..."
                    };
                return new string[] {
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
        }

        protected override string[] averageSocialDialogues
        {
            get
            {
                if (_language == Util.Enums.Language.Portuguese)
                    return new string[] {
                    "Obrigado por ouvir {questSo.GetTargetNpc()}. {questSo.GetTargetNpc()} precisavam disso, e eu sei que fez a diferença.",
                    "Significa muito que você tenha ouvido {questSo.GetTargetNpc()}. Não muitos teriam dedicado seu tempo.",
                    "Obrigado por mostrar tanta paciência a {questSo.GetTargetNpc()}. {questSo.GetTargetNpc()} precisava de alguém como você para ouvi-lo.",
                    "Estou realmente grato por você ter conversado com {questSo.GetTargetNpc()}. {questSo.GetTargetNpc()} tem tanto a compartilhar, e você ouviu.",
                    "Obrigado por prestar atenção em {questSo.GetTargetNpc()}. Eu sei que {questSo.GetTargetNpc()} se sentiu ouvido por sua causa.",
                    "Eu realmente aprecio você ter dedicado seu tempo a {questSo.GetTargetNpc()}. Você elevou o espírito dele.",
                    "Obrigado. Ouvir {questSo.GetTargetNpc()} não foi apenas gentil—foi exatamente o que ele precisava."
                    };
                return new string[] {
                "Thanks for listening to {questSo.GetTargetNpc()}. They needed that, and I know it made a difference.",
                "It means a great deal that you listened to {questSo.GetTargetNpc()}. Not many would take the time.",
                "Thank you for showing {questSo.GetTargetNpc()} such patience. They needed someone like you to hear them.",
                "I’m truly grateful that you spoke with {questSo.GetTargetNpc()}. They had so much to share, and you listened.",
                "Thank you for lending an ear to {questSo.GetTargetNpc()}. I know they feel heard because of you.",
                "I really appreciate you taking the time with {questSo.GetTargetNpc()}. You’ve lifted their spirits.",
                "Thank you. Listening to {questSo.GetTargetNpc()} wasn’t just kind—it was exactly what they needed."
                };
            }
        }

        protected override string[] highSocialDialogues
        {
            get
            {
                if (_language == Util.Enums.Language.Portuguese)
                    return new string[] {
                    "Oh, muito obrigado por ouvir {questSo.GetTargetNpc()}! Eles têm as histórias mais fascinantes, não têm?",
                    "Você ouviu {questSo.GetTargetNpc()}? Maravilhoso! Não são uma verdadeira fonte de informações?",
                    "Obrigado, obrigado! {questSo.GetTargetNpc()} tem coisas tão interessantes para dizer, e eu sabia que você ia apreciar!",
                    "Ah, eu sabia que você ia ouvir {questSo.GetTargetNpc()}! Não é ótimo ouvir o lado deles? Obrigado por dar ouvidos a eles!",
                    "Muito obrigado por ouvir {questSo.GetTargetNpc()}! Eles sempre têm as melhores percepções. Eles te contaram sobre a vez que...?",
                    "Obrigado! {questSo.GetTargetNpc()} tem tanto a dizer, e eu sabia que você seria a pessoa certa para ouvir!",
                    "Oh, obrigado por ouvir {questSo.GetTargetNpc()}! Eu poderia conversar com eles o dia todo, e agora sei que você também poderia!",
                    "Muito obrigado! {questSo.GetTargetNpc()} sempre tem coisas maravilhosas para compartilhar. Eles te contaram as últimas novidades?"
                    };
                return new string[] {
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
}