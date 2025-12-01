using Game.GameManager;
namespace Game.NPCs
{
    public class ReportQuestCheckPoint : QuestDialogue
    {
        protected override string[] lowSocialDialogues
        {
            get
            {
                if (_language == Util.Enums.Language.Portuguese)
                    return new string[] {
                    "Eu não ligo pro que {questSo.GetOwnerNpc()} tem a \"reportar\". Diga pra {questSo.GetOwnerNpc()} não falar mais comigo."
                    };
                return new string[] {
                "I don't care what {questSo.GetOwnerNpc()} has to \"report\". Tell them to not speak with me again."
                };
            }
        }

        protected override string[] averageSocialDialogues
        {
            get
            {
                if (_language == Util.Enums.Language.Portuguese)
                    return new string[] {
                    "Oh? Você está com informações vindas de {questSo.GetOwnerNpc()}? ... Entendo, muito bem. Você pode dizer que a mensagem foi recebida."
                    };
                return new string[] {
                "Oh? You're here with information from {questSo.GetOwnerNpc()}? ... I see, very well. You can tell them the message was delivered."
                };
            }
        }

        protected override string[] highSocialDialogues
        {
            get
            {
                if (_language == Util.Enums.Language.Portuguese)
                    return new string[] {
                    "{questSo.GetOwnerNpc()} disse o queeeee? Muito obrigada por me avisar! Pode dizer um muito obrigado a {questSo.GetOwnerNpc()} também!"
                    };
                return new string[] {
                "{questSo.GetOwnerNpc()} said whaaaaat? Thanks so much for delivering the message! You can give them my thanks too!"
                };
            }
        }
    }
}