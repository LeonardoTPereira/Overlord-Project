using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs.PTBR
{
    public class GiveQuestOpener : QuestOpener
    {
        protected override string [] lowSocialOpeners{
            get => new string [] {
            "Leve {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()}. Eu faria isso, mas tenho coisas mais importantes para fazer.",
            "Ugh, tá bom. Você está aqui, então seja útil e entregue {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()}. Não perca.",
            "Eu não tenho tempo para essa bobagem. Leve {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()} e seja rápido.",
            "{questSo.GetTargetNpc()} precisa de {questSo.GetItemAmountString()}, e eu não estou com paciência para lidar com isso. Você cuida disso.",
            "Entregue {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()} para mim, pode ser? Eu já tenho o suficiente para me preocupar.",
            "Por que sempre sou eu que fico fazendo isso? Seja lá, agora é sua vez. Dê {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()}.",
            "Leve um {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()}. E não me pergunte por quê—não é da sua conta.",
            "Olha, eu não confio em mais ninguém para isso, então você vai fazer. Dê {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()}. Tente não estragar tudo.",
            "Você vai para lá de qualquer forma, certo? Ótimo. Entregue {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()} enquanto estiver indo.",
            "Eu não aguento mais lidar com {questSo.GetTargetNpc()} hoje. Leve {questSo.GetItemAmountString()} para eles por mim, e não me faça me arrepender de ter pedido."
        };
        }

        protected override string [] averageSocialOpeners{
            get => new string [] {
            "Você poderia levar {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()} para mim? Eles estão esperando isso, e eu ficaria grato pela ajuda!",
            "Aqui está um item que {questSo.GetTargetNpc()} precisa. Você pode entregá-lo para eles? Eu confio que você vai fazer isso com segurança.",
            "Eu estava querendo entregar {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()}, mas estou atolado. Você se importaria de levar para eles?",
            "Oh, que coincidência! Eu preciso de alguém para entregar {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()}. Você acha que consegue fazer isso?",
            "Eu realmente ficaria grato se você pudesse levar {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()}. É algo que eles estão pedindo.",
            "Você poderia correr até {questSo.GetTargetNpc()} e entregar {questSo.GetItemAmountString()}? É importante que eles recebam isso logo, e eu sei que posso contar com você."
        };
        }

        protected override string [] highSocialOpeners{
            get => new string [] {
            "Oh, oi! Justo a pessoa que eu esperava ver! Então, {questSo.GetTargetNpc()} me disse um tempo atrás que eles precisam de {questSo.GetItemAmountString()}. Mas, você sabe como é, eu sempre me distraio e esqueço no meio do caminho. Você pode entregar para eles por mim? Por favor?",
            "Ai meu Deus, eu estava querendo levar {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()} há séculos! Bem, ok, talvez não séculos, mas parece que foi. Enfim, você pode entregar? Você é muito melhor nisso!",
            "Então, engraçado, eu prometi a {questSo.GetTargetNpc()} que eu entregaria {questSo.GetItemAmountString()}, mas, bem, sempre acontece alguma coisa! Você pode me ajudar e levar para eles? Você é um salva-vidas!",
            "Oh, isso é perfeito! {questSo.GetTargetNpc()} precisa de {questSo.GetItemAmountString()}, e você é a pessoa perfeita para entregar! Não se importa, né? Quero dizer, você já está indo para lá, certo?",
            "Ok, então aqui vai o trato—eu preciso levar {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()}, mas sejamos sinceros, eu provavelmente ia deixar cair ou perder ou fazer algo bobo assim. Mas você? Você vai dar conta de tudo!",
            "Então, eu estava querendo deixar {questSo.GetItemAmountString()} com {questSo.GetTargetNpc()}, mas, você sabe como é, a vida acaba atrapalhando! Você se importaria de levar para eles? Eu vou te dever um favor. Ou dois!"
        };
        }
    }
}
