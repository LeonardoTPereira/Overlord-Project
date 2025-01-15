using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs.PTBR
{
    public class ListenQuestOpener : QuestOpener
    {
        protected override string [] lowSocialOpeners {
            get => new string [] {
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
        }

        protected override string [] averageSocialOpeners {
            get => new string [] {
            "Você tem se perguntado sobre as origens dessa masmorra? Vai lá ouvir {questSo.GetTargetNpc()} — eles viram coisas que podem te ajudar a entender mais.",
            "Pode ser útil ouvir {questSo.GetTargetNpc()}. Eles têm conhecimentos que podem te ajudar.",
            "Eu sei que você está ocupado, mas ouvir o que {questSo.GetTargetNpc()} tem a dizer pode fazer a diferença.",
            "Confie em mim, vale a pena ouvir {questSo.GetTargetNpc()}. Eles estão cheios de informações valiosas.",
            "Escute atentamente o que {questSo.GetTargetNpc()} diz. Eles passaram por muita coisa e sabem segredos.",
            "Você deveria tirar um tempo para ouvir {questSo.GetTargetNpc()}. Eles podem ter um conhecimento que você pode usar."
            };
        }

        protected override string [] highSocialOpeners {
            get => new string [] {
            "Eu acho que seria útil você ouvir {questSo.GetTargetNpc()}. Eles têm uma sabedoria tranquila.",
            "Por favor, tire um tempo para ouvir {questSo.GetTargetNpc()}. Acho que você vai achar realmente proveitoso.",
            "Tenho certeza de que isso significaria muito para {questSo.GetTargetNpc()} se você ouvisse o que eles têm a dizer. Eles têm um coração gentil.",
            "Eu acredito que {questSo.GetTargetNpc()} pode te oferecer uma orientação valiosa. Você poderia tirar um momento para ouvi-los?",
            "Eu acho que isso pode te ajudar a ouvir de {questSo.GetTargetNpc()}. Eles têm uma perspectiva interessante sobre as coisas. Sempre me ajudam!",
            "Seria maravilhoso se você pudesse ouvir o que {questSo.GetTargetNpc()} tem a dizer. Eles falam com o coração."
            };
        }
    }
}
