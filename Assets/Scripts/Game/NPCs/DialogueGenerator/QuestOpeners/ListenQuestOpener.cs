using Game.GameManager;
namespace Game.NPCs
{
    public class ListenQuestOpener : QuestDialogue
    {
        protected override string [] lowSocialDialogues {
            get {
                if (GameManagerSingleton.Instance.IsInPortuguese) 
                    return new string [] {
                    "Chega de perguntas! {questSo.GetTargetNpc()} tem as respostas. Vai lá e escute o que eles têm a dizer.",
                    "Ugh, vai lá ouvir {questSo.GetTargetNpc()}, vai? Eles não vão parar de falar até alguém ouvir.",
                    "Se você quer respostas, vai incomodar {questSo.GetTargetNpc()}. Eu já tenho o suficiente para fazer.",
                    "Escuta, eu não tenho tempo para explicar tudo. Vai lá e escuta de {questSo.GetTargetNpc()}.",
                    "Se você está tão curioso, vai falar com {questSo.GetTargetNpc()}. Eles adoram ficar tagarelando.",
                    "Vai lá, então. {questSo.GetTargetNpc()} tem todas as respostas que você está procurando. Não me faça repetir.",
                    "Quer informação? Vai ouvir {questSo.GetTargetNpc()} ao invés de me incomodar.",
                    "Só vai. {questSo.GetTargetNpc()} pode te contar tudo. Eu não tenho paciência para isso.",
                    "Olha, se você realmente precisa saber, vai ouvir {questSo.GetTargetNpc()}. Eles estão morrendo de vontade de conversar.",
                    "Estou ocupado. {questSo.GetTargetNpc()} tem todos os detalhes. Vai perder o tempo deles, não o meu.",
                    "Tá bom, vai lá ouvir {questSo.GetTargetNpc()}. Tenho certeza que eles vão te contar tudo... duas vezes.",
                    "Se você não vai embora, pelo menos vai incomodar {questSo.GetTargetNpc()}. Eles adoram conversar.",
                    "Se você está tão desesperado por informações, vai falar com {questSo.GetTargetNpc()}. Eles devem estar morrendo de vontade de falar.",
                    "Ugh, pela última vez—vai ouvir {questSo.GetTargetNpc()}. Eles vão te contar mais do que você queria saber."
                    };
                return new string [] {
                "Enough questions! {questSo.GetTargetNpc()} has the answers. Go get an earful from them.",
                "Ugh, just go listen to {questSo.GetTargetNpc()}, will you? They won't stop talking until someone does.",
                "If you want answers, go bother {questSo.GetTargetNpc()}. I've got enough on my plate.",
                "Listen, I don’t have time to explain everything. Go hear it from {questSo.GetTargetNpc()}.",
                "If you’re so curious, go talk to {questSo.GetTargetNpc()}. They love to ramble on.",
                "Go on, then. {questSo.GetTargetNpc()} has all the answers you’re looking for. Don’t make me repeat myself.",
                "You want information? Go listen to {questSo.GetTargetNpc()} instead of pestering me.",
                "Just go. {questSo.GetTargetNpc()} can tell you everything. I don’t have the patience for this.",
                "Look, if you really need to know, go listen to {questSo.GetTargetNpc()}. They’re dying to chat.",
                "I'm busy. {questSo.GetTargetNpc()} has all the details. Go waste their time, not mine.",
                "Fine, go listen to {questSo.GetTargetNpc()}. I’m sure they’ll tell you everything... twice.",
                "If you're not gonna leave, at least go bother {questSo.GetTargetNpc()}. They love talking.",
                "If you're that desperate for info, go talk to {questSo.GetTargetNpc()}. They’re probably itching to yap.",
                "Ugh, for the last time—go listen to {questSo.GetTargetNpc()}. They'll tell you more than you ever wanted."
                };
            }
        }

        protected override string [] averageSocialDialogues {
            get {
                if (GameManagerSingleton.Instance.IsInPortuguese) 
                    return new string [] {
                    "Você tem se perguntado sobre as origens dessa masmorra? Vai lá ouvir {questSo.GetTargetNpc()} — eles viram coisas que podem te ajudar a entender mais.",
                    "Pode ser útil ouvir {questSo.GetTargetNpc()}. Eles têm conhecimentos que podem te ajudar.",
                    "Eu sei que você está ocupado, mas ouvir o que {questSo.GetTargetNpc()} tem a dizer pode fazer a diferença.",
                    "Confie em mim, vale a pena ouvir {questSo.GetTargetNpc()}. Eles estão cheios de informações valiosas.",
                    "Escute atentamente o que {questSo.GetTargetNpc()} diz. Eles passaram por muita coisa e sabem segredos.",
                    "Você deveria tirar um tempo para ouvir {questSo.GetTargetNpc()}. Eles podem ter um conhecimento que você pode usar."
                    };
                return new string [] {
                "Have you been wondering about the origins of this dungeon? Go listen to {questSo.GetTargetNpc()} — they’ve seen things that could help you understand more.",
                "It might be helpful to listen to {questSo.GetTargetNpc()}. They’ve got knowledge that may aid you.",
                "I know you’re busy, but hearing what {questSo.GetTargetNpc()} has to say could make a difference.",
                "Trust me, it’ll be worth it to listen to {questSo.GetTargetNpc()}. They’re full of valuable information.",
                "Listen carefully to what {questSo.GetTargetNpc()} says. They’ve been through a lot and know secrets.",
                "You should take the time to hear out {questSo.GetTargetNpc()}. They may have knowledge you can use."
                };
            }
        }

        protected override string [] highSocialDialogues {
            get {
                if (GameManagerSingleton.Instance.IsInPortuguese) 
                    return new string [] {
                    "Eu acho que seria útil você ouvir {questSo.GetTargetNpc()}. Eles têm uma sabedoria tranquila.",
                    "Por favor, tire um tempo para ouvir {questSo.GetTargetNpc()}. Acho que você vai achar realmente proveitoso.",
                    "Tenho certeza de que isso significaria muito para {questSo.GetTargetNpc()} se você ouvisse o que eles têm a dizer. Eles têm um coração gentil.",
                    "Eu acredito que {questSo.GetTargetNpc()} pode te oferecer uma orientação valiosa. Você poderia tirar um momento para ouvi-los?",
                    "Eu acho que isso pode te ajudar a ouvir de {questSo.GetTargetNpc()}. Eles têm uma perspectiva interessante sobre as coisas. Sempre me ajudam!",
                    "Seria maravilhoso se você pudesse ouvir o que {questSo.GetTargetNpc()} tem a dizer. Eles falam com o coração."
                    };
                return new string [] {
                "I think you’d find it helpful to listen to {questSo.GetTargetNpc()}. They have a gentle wisdom about them.",
                "Please, take some time to listen to {questSo.GetTargetNpc()}. I think you'll find it truly worthwhile.",
                "I'm sure it would mean a lot to {questSo.GetTargetNpc()} if you listened to what they have to say. They have a kind heart.",
                "I believe {questSo.GetTargetNpc()} could offer you valuable guidance. Could you take a moment to hear them out?",
                "I think it might help you to hear from {questSo.GetTargetNpc()}. They have an interesting perspective on things. Always helps me out!",
                "It would be wonderful if you could hear what {questSo.GetTargetNpc()} has to say. They speak from the heart."
                };
            }
        }
    }
}