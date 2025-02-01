using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs.PTBR
{
    public class ExchangeQuestOpener : QuestOpener
    {
        protected override string [] lowSocialOpeners {
            get => new string [] {
            "Ugh, eu não tenho tempo para essa missão. Se faça útil e faça isso por mim. Vá trocar um {questSo.GetItemString()} com {questSo.GetTargetNpc()}. Talvez você ganhe algo em troca.",
            "Olha, eu não estou afim de lidar com {questSo.GetTargetNpc()} hoje. Você cuida da troca e eles vão te recompensar. Tudo o que você precisa fazer é levar um {questSo.GetItemString()} até eles.",
            "Por que você não vai incomodar outra pessoa, hein? Leve um {questSo.GetItemString()} até [Nome do NPC]. Eles têm algo esperando para quem entregar—não me pergunte o quê.",
            "Vá trocar um {questSo.GetItemString()} com {questSo.GetTargetNpc()}. Você vai ganhar algum tipo de recompensa e eu vou ganhar um pouco de paz e silêncio.",
            "Se você está tão ansioso por recompensas, leve um {questSo.GetItemString()} até {questSo.GetTargetNpc()}. Eles vão te dar algo, tenho certeza.",
            "Eu estou muito ocupado para pedidos de troca. Leve um {questSo.GetItemString()} até {questSo.GetTargetNpc()}, eles vão te recompensar, e eu não terei que levantar um dedo."
            };
        }

        protected override string [] averageSocialOpeners{
            get => new string [] {
             "Se você topar, eu preciso que você troque um {questSo.GetItemString()} com {questSo.GetTargetNpc()}. Eles precisam disso para um novo feitiço que estão aprendendo e vão te recompensar generosamente por isso.",
            "Você estaria disposto a fazer uma troca para mim? Dê um {questSo.GetItemString()} para {questSo.GetTargetNpc()}, e ouvi dizer que eles têm uma boa recompensa pronta.",
            "Eu deveria ter dado {questSo.GetTargetNpc()} um {questSo.GetItemString()} antes do amanhecer para o novo feitiço deles, mas acho que não vou conseguir. Se você puder fazer isso por mim, pode pegar a recompensa deles.",
            "Você poderia levar um {questSo.GetItemString()} até {questSo.GetTargetNpc()}? Eles me disseram que estão procurando isso e têm uma recompensa para quem entregar."
            };
        }

        protected override string [] highSocialOpeners{
            get => new string [] {
            "Ah, você é exatamente a pessoa que eu preciso! Você poderia levar um {questSo.GetItemString()} até {questSo.GetTargetNpc()}? Tenho certeza de que eles vão te recompensar grandemente!",
            "Eu ouvi dizer que {questSo.GetTargetNpc()} está morrendo de vontade de colocar as mãos em um {questSo.GetItemString()}. Leve até eles, e eles vão ficar tão felizes—eles sempre dão as melhores recompensas!",
            "Eu estava querendo trocar um {questSo.GetItemString()} com {questSo.GetTargetNpc()}, mas estou tão ocupado! Se você fizer isso por mim, tenho certeza de que eles vão te dar algo incrível. Eles sempre têm os melhores tesouros!",
            "Você é a pessoa certa para isso! Leve um {questSo.GetItemString()} até {questSo.GetTargetNpc()}, e provavelmente eles vão te dar algo melhor do que você imaginava. Eles sempre me surpreendem com o que têm!",
            "Ah, eu sei que você está ocupado, mas {questSo.GetTargetNpc()} vai realmente apreciar uma troca! Eles sempre recompensam a gentileza—acredite, vai valer a pena o seu tempo! É só levar um {questSo.GetItemString()} até eles e você vai ver o que eu quero dizer!",
            "Ei, você poderia trocar um {questSo.GetItemString()} com {questSo.GetTargetNpc()}? Não vejo a hora de ver a expressão no rosto deles quando você entregar isso! Eles provavelmente vão te recompensar com algo que vai te deixar de queixo caído. Eles sempre têm as melhores coisas!"
            };
        }
    }
}