using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs.PTBR
{
    public class GoToQuestOpener : QuestOpener
    {
        protected override string [] lowSocialOpeners {
            get => new string [] {
                "Ugh, aquela área tem sido uma dor de cabeça para mim. Lá nas coordenadas {questSo.GetRoomCoordinates()}? Vai lá dar uma olhada e ver o que está causando todo esse alvoroço, pode ser?",
                "Aquele lugar em {questSo.GetRoomCoordinates()} ainda não foi explorado, e eu estou ocupado demais para lidar com isso. Acho que agora é seu problema.",
                "Não me importa como você faz isso, mas vá até {questSo.GetRoomCoordinates()} e veja o que tem lá. Só não faça disso meu problema depois.",
                "Aquela zona inexplorada em {questSo.GetRoomCoordinates()}? Sim, tem me incomodado. Dá uma olhada e me diga se vale a pena meu tempo.",
                "Se você não estiver muito ocupado só parado por aí, talvez possa realmente explorar em {questSo.GetRoomCoordinates()} para mim. Alguém tem que fazer isso.",
                "A sala em {questSo.GetRoomCoordinates()} é uma mancha nos meus planos. Vai lá descobrir o que está acontecendo lá para que eu possa parar de me preocupar com isso."
            };
        }

        protected override string [] averageSocialOpeners{
            get => new string [] {
            "Estou pensando em entrar em um concurso de pintura, mas ainda estou decidindo o que pintar. Ouvi dizer que há um lugar bonito nas coordenadas {questSo.GetRoomCoordinates()}. Você pode ir lá e dar uma olhada para mim?",
            "Você se importaria de dar uma olhada em uma área para mim? Ouvi dizer que há algo interessante nas coordenadas {questSo.GetRoomCoordinates()}, mas eu não posso ir pessoalmente.",
            "Tem um lugar não muito longe daqui que eu gostaria que você explorasse. Nas coordenadas {questSo.GetRoomCoordinates()}. Quem sabe o que você pode encontrar, mas pode ser importante.",
            "Você poderia ir até as coordenadas {questSo.GetRoomCoordinates()} para mim? Tenho uma sensação de que há algo que vale a pena por lá.",
            "Eu preciso de alguém para investigar as coordenadas {questSo.GetRoomCoordinates()}. Você parece capaz, então se estiver afim, pode explorar e me avisar o que encontrar?",
            "Estou curioso para saber o que há além da sala {questSo.GetRoomCoordinates()}. Talvez você possa dar uma olhada e ver o que está acontecendo por lá?",
            "Tem uma área à frente que está inexplorada há um tempo. Já ouviu falar da sala {questSo.GetRoomCoordinates()}? Pode explorar e ver se algo se destaca?",
            "Se você tiver um tempo livre, poderia explorar as coordenadas {questSo.GetRoomCoordinates()}? Ouvi rumores sobre coisas estranhas acontecendo lá.",
            "Eu ficaria grato se você pudesse fazer uma viagem até as coordenadas {questSo.GetRoomCoordinates()}. Tenho um pressentimento de que há algo lá que pode ser útil.",
            "Dá uma olhada nas coordenadas {questSo.GetRoomCoordinates()} quando tiver uma oportunidade. Tenho um mau pressentimento sobre isso, e gostaria que você conferisse.",
            "Eu não posso ir até lá, mas você parece ser o tipo aventureiro. Você estaria disposto a explorar as coordenadas {questSo.GetRoomCoordinates()} para mim?"
        };
        }

        protected override string [] highSocialOpeners{
            get => new string [] {
                "Oh, você está aqui! Fantástico! Então, tem esse ponto nas coordenadas {questSo.GetRoomCoordinates()} que tem me incomodado de curiosidade há séculos. Você poderia dar uma olhada? Eu iria eu mesmo, mas, sabe, motivos!",
                "Ei, eu fiquei olhando esse mapa por horas, e tem algo intrigante nas coordenadas {questSo.GetRoomCoordinates()}. Você poderia explorar para mim? Ah, e não se esqueça de tirar umas notas—eu adoro detalhes!",
                "Ok, aqui vai a situação—eu encontrei essas coordenadas, {questSo.GetRoomCoordinates()}, e preciso saber o que tem lá. Você vai dar uma olhada, certo? Por favor? Por favorzinho?",
                "Você é exatamente a pessoa que eu esperava ver! Tem um lugar nas coordenadas {questSo.GetRoomCoordinates()} que tem me incomodado. E se for um tesouro? Ou algo misterioso? Você poderia investigar?",
                "Então, você não vai acreditar, mas eu ouvi rumores sobre {questSo.GetRoomCoordinates()}. Rumores estranhos! Pode ir lá ver o que está acontecendo? Eu vou ficar aqui, morrendo de vontade de saber o que você encontrar!",
                "Ok, então, eu estive marcando alguns lugares nesse mapa, e {questSo.GetRoomCoordinates()} simplesmente grita ‘aventura!’ Você poderia explorar? Eu te devo um grande favor—sério!",
                "Oh, você tem que me ajudar! Tem um ponto nas coordenadas {questSo.GetRoomCoordinates()} que tem me assombrado nos meus sonhos. Ok, não literalmente, mas estou tão curioso. Você pode dar uma olhada para mim?",
                "Eu tenho um pressentimento sobre {questSo.GetRoomCoordinates()}. Não pergunte por quê—é só uma intuição! Você pode dar uma olhada e ver se meus instintos estão certos?",
                "Oh, eu tenho a tarefa perfeita para você! Tem algo especial nas coordenadas {questSo.GetRoomCoordinates()}. Bem, eu acho que é especial, mas é aí que você entra—vai explorar para mim, vai?",
                "Então, história engraçada! Eu encontrei essas coordenadas, {questSo.GetRoomCoordinates()}, e eu simplesmente sei que tem algo fascinante lá. Você pode ir conferir e me contar? Estou tão animado para ouvir o que você encontra!"
            };
        }
    }
}