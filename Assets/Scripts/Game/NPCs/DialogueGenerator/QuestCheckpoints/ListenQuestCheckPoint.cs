using Game.GameManager;
namespace Game.NPCs
{
    //TODO
    public class ListenQuestCheckPoint : QuestDialogue
    {
        protected override string[] lowSocialDialogues
        {
            get
            {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string[] {
                    "Não acredito que {questSo.GetOwnerNpc()} está mandando bobalhões que nem você virem aqui tirar a minha paz."
                    };
                return new string[] {
                "I can't believe {questSo.GetOwnerNpc()} is sending fools like you to come here and end my peace."
                };
            }
        }

        protected override string[] averageSocialDialogues
        {
            get
            {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string[] {
                    "{questSo.GetOwnerNpc()} te enviou aqui pra ouvir o que eu tenho a dizer?"
                    };
                return new string[] {
                "{questSo.GetOwnerNpc()} sent you here to listen to what I have to say?"
                };
            }
        }

        protected override string[] highSocialDialogues
        {
            get
            {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string[] {
                    "{questSo.GetOwnerNpc()} disse pra você vir aqui?! Estou tão feliz! Adoro poder conversar :)"
                    };
                return new string[] {
                "{questSo.GetOwnerNpc()} told you to come here?! I'm so happy! I love being able to talk :)"
                };
            }
        }
    }
}