using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs.PTBR
{
    public class ExploreQuestOpener : QuestOpener
    {
        protected override string [] lowSocialOpeners{
            get => new string [] {
            "Já faz um tempo desde que deixei minhas funções aqui como {questSo.Npc.Job}. Nem sei se essa masmorra tem {questSo.GetRoomAmount()} cômodos. ... Você pode confirmar isso?",
            "Ótimo, outra coisa que eu não posso fazer. Vá procurar {questSo.GetRoomAmount()} cômodos nesta masmorra e me avise o que encontrou.",
            "Eu preciso de alguém para investigar {questSo.GetRoomAmount()} cômodos, e você, por sorte, é o único disponível. Vai lá.",
            "Tem {questSo.GetRoomAmount()} cômodos que precisam ser investigados, e eu estou ocupado demais. Adivinha? Agora é seu problema.",
            "Eu não estou afim de lidar com isso, então você ficou com a tarefa. Vá verificar {questSo.GetRoomAmount()} cômodos e me diga o que encontrou. Dizem que essa masmorra muda cada vez que você entra..."
        };
        }

        protected override string [] averageSocialOpeners{
            get => new string [] {
            "Meu melhor amigo costumava ser cartógrafo. Ele vai me visitar em breve e eu estava pensando em preparar uma surpresa. Você pode ajudar? Eu queria fazer um mapa para explorarmos juntos, mas antes preciso confirmar o tamanho. Você poderia verificar se há pelo menos {questSo.GetRoomAmount()} cômodos aqui?",
            "Às vezes eu fico imaginando como seria ser cartógrafo. Talvez eu devesse tentar fazer um mapa para mim mesmo. Não sei se {questSo.GetRoomAmount()} seria um mapa muito grande... Você poderia explorar {questSo.GetRoomAmount()} cômodos e me contar como são para eu poder começar meu mapa?",
            "Se você tiver tempo, poderia explorar {questSo.GetRoomAmount()} cômodos na área? Eu ficaria muito mais tranquilo sabendo o que tem lá.",
            "Eu preciso que alguém explore {questSo.GetRoomAmount()} cômodos lá. Quem sabe o que você vai encontrar, mas vale a pena dar uma olhada.",
            "Ei, você poderia dar uma olhada em {questSo.GetRoomAmount()} cômodos nesta masmorra? Pode ser que haja algo útil lá."
        };
        }

        protected override string [] highSocialOpeners {
            get => new string [] {
            "Ei, você poderia me fazer um pequeno favor e explorar {questSo.GetRoomAmount()} cômodos nesta área? Eu iria eu mesmo, mas quem sabe o que tem lá! Eu só ia entrar em pânico!",
            "Então, uma história engraçada—eu estava querendo olhar aqueles {questSo.GetRoomAmount()} cômodos, mas sempre aparece algo! Você pode cuidar disso para mim? Tenho certeza que são fascinantes!",
            "Eu sei que é pedir demais, mas você poderia explorar {questSo.GetRoomAmount()} cômodos para mim? É que eu sempre fiquei me perguntando sobre eles e não aguento mais a curiosidade!",
            "Você é a pessoa perfeita para isso! Você poderia dar uma olhada em {questSo.GetRoomAmount()} cômodos nesta masmorra? Eu sei que tem algo incrível esperando para ser encontrado!",
            "Ah, isso é tão empolgante! Eu preciso que você explore {questSo.GetRoomAmount()} cômodos para mim—é muito misterioso para ignorar, e eu preciso saber o que tem lá!"
        };
        }
    }
}