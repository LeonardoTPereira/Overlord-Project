using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs.PTBR
{
    public class GatherQuestOpener : QuestOpener
    {
        protected override string [] lowSocialOpeners {
            get => new string [] {
            "Nossa, eu me pergunto como esse lugar ficou tão bagunçado em primeiro lugar. Seja útil e colete os {questSo.GetItemAmountString()} que estão por aí.",
            "Eu preciso de {questSo.GetItemAmountString()} para um feitiço. Que feitiço? Não é da sua conta. Você pode até ficar com os {questSo.GetItemString()}.",
            "Ótimo, mais trabalho para mim—mas estou jogando isso para você. Vai lá e coleta {questSo.GetItemAmountString()}, e não me faça esperar muito.",
            "Você ainda está aqui? Ótimo, porque eu preciso de {questSo.GetItemAmountString()}. Acha que consegue lidar com isso, ou é muito para você?",
            "Quer ser útil? Beleza, vai lá e coleta {questSo.GetItemAmountString()} para mim. Se você for rápido, talvez eu até diga obrigado. Talvez.",
            "Olha, eu não tenho energia para explicar o porquê. Só colete {questSo.GetItemAmountString()} e pare de fazer perguntas."
            };
        }

        protected override string [] averageSocialOpeners {
            get => new string [] {
            "Esse lugar está uma bagunça! Você poderia, por favor, coletar os {questSo.GetItemAmountString()} que estão por aí?",
            "Eu estou começando a estudar {questSo.GetItemString()}. Você poderia me trazer {questSo.GetItemAmountString()} para eu dar uma olhada? Você pode ficar com eles, eu só preciso estudar um pouco…",
            "Eu realmente sinto falta da minha mãe. Estava lendo sobre um feitiço de comunicação, mas eu precisaria de {questSo.GetItemAmountString()}. Você poderia pegar isso para mim? Oh, obrigado! Eu não tenho muito a oferecer, mas você pode ficar com os {questSo.GetItemString()} como recompensa.",
            "Você sabia que {questSo.GetItemString()} são itens mágicos? Dá para usar eles em muitos tipos de feitiços. Eu tenho tentado analisá-los, mas eu precisaria de {questSo.GetItemAmountString()}. Você pode ficar com eles como recompensa!",
            "Você pode me ajudar? Estou procurando {questSo.GetItemAmountString()}, e você parece a pessoa perfeita para encontrar.",
            "Ei, eu preciso de {questSo.GetItemAmountString()} para um projeto em que estou trabalhando. Você pode encontrá-los para mim?",
            "Ei, você é bom em encontrar as coisas, certo? Eu preciso de {questSo.GetItemAmountString()}—poderia coletá-los para mim quando tiver a chance?"
            };
        }

        protected override string [] highSocialOpeners {
            get => new string [] {
            "Ah, sabe, eu venho pensando nisso há um tempo, e eu só preciso perguntar—você poderia coletar {questSo.GetItemAmountString()} para mim? Eles são tão brilhantes e raros! Eu faria isso, mas, bem, você é muito melhor nisso!",
            "Você não vai acreditar, mas eu ouvi dizer que existem exatamente {questSo.GetItemAmountString()} por aí, esperando para serem encontrados! Você se importaria de coletá-los para mim? Imagina as possibilidades quando tivermos eles!",
            "Você tem que me ajudar com isso! Eu preciso de {questSo.GetItemAmountString()}, e eu estou quebrando a cabeça tentando descobrir onde encontrá-los. Mas você? Ah, você é um caçador de tesouros natural!",
            "Ai meu Deus, você está aqui! Que momento perfeito! Eu preciso de {questSo.GetItemAmountString()}, tipo, urgentemente. Bem, não urgente-urgente, mas sabe, logo. Você pode me ajudar?",
            "Ok, então eu estava pensando, não seria incrível se tivéssemos {questSo.GetItemAmountString()}? Quero dizer, imagine todas as coisas incríveis que poderíamos fazer com eles! Você vai me ajudar a coletá-los, certo?",
            "Ah, você está aqui! Maravilhoso! Eu tenho esse probleminha—bem, não é um problema exatamente, é mais uma oportunidade—eu preciso de {questSo.GetItemAmountString()}, e eu só sei que você é a pessoa certa para encontrá-los!",
            "Posso te contar um segredo? Eu estou morrendo de vontade de pegar {questSo.GetItemAmountString()}! Eles são tão perfeitos para… ah, deixa pra lá. De qualquer forma, você pode coletá-los para mim? Por favor?",
            "Então, uma história engraçada! Eu estava planejando coletar {questSo.GetItemAmountString()} eu mesma, mas aí eu me lembrei, 'Ah, espera, eu conheço alguém muito mais capaz!' Essa pessoa é você, aliás. Pode me ajudar?"
            };
        }
    }
}