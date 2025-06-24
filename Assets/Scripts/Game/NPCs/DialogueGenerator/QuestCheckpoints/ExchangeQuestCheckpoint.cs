using Game.GameManager;
namespace Game.NPCs
{
    public class ExchangeQuestCheckPoint : QuestDialogue
    {
        protected override string[] lowSocialDialogues
        {
            get
            {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string[] {
                    "Não quero nada que você tenha a oferecer. O quê? {questSo.GetOwnerNpc()} te enviou? Affe, ta bom então."
                    };
                return new string[] {
                "I don't want anything you have to offer. What? {questSo.GetOwnerNpc()} sent you? Ugh, ok then."
                };
            }
        }

        protected override string[] averageSocialDialogues
        {
            get
            {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string[] {
                    "{questSo.GetOwnerNpc()} te enviou aqui pra trocar alguns itens?"
                    };
                return new string[] {
                "{questSo.GetOwnerNpc()} sent you here to trade a few items?"
                };
            }
        }

        protected override string[] highSocialDialogues
        {
            get
            {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string[] {
                    "Uuuuuh, eu lembro sim do {questSo.GetOwnerNpc()} mencionando sobre uma troca. Eu fiquei tão animado que até esqueci o que ele ofereceu. Estou feliz que você está aqui pra ajudar c:"
                    };
                return new string[] {
                "Uuuuh, I do remember {questSo.GetOwnerNpc()} mentioning about a trade. I got so excited about it I forgot what he even offered. I'm glad you're here to help c:"
                };
            }
        }
    }
}