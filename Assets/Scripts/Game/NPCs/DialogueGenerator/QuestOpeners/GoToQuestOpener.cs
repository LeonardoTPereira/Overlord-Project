using Game.GameManager;

namespace Game.NPCs
{
    public class GoToQuestOpener : QuestDialogue
    {
        protected override string [] lowSocialDialogues {
            get
            {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string[] {
                    "Ugh, aquela área tem sido uma dor de cabeça para mim. Lá nas coordenadas {questSo.GetRoomCoordinates()}? Vai lá dar uma olhada e ver o que está causando todo esse alvoroço, pode ser?",
                    "Aquele lugar em {questSo.GetRoomCoordinates()} ainda não foi explorado, e eu estou ocupado demais para lidar com isso. Acho que agora é seu problema.",
                    "Não me importa como você faz isso, mas vá até {questSo.GetRoomCoordinates()} e veja o que tem lá. Só não faça disso meu problema depois.",
                    "Aquela zona inexplorada em {questSo.GetRoomCoordinates()}? Sim, tem me incomodado. Dá uma olhada e me diga se vale a pena meu tempo.",
                    "Se você não estiver muito ocupado só parado por aí, talvez possa realmente explorar em {questSo.GetRoomCoordinates()} para mim. Alguém tem que fazer isso.",
                    "A sala em {questSo.GetRoomCoordinates()} é uma mancha nos meus planos. Vai lá descobrir o que está acontecendo lá para que eu possa parar de me preocupar com isso."
                    };
                return new string[] {
                "Ugh, that area’s been a headache for me. Around {questSo.GetRoomCoordinates()}? Go check it out and see what’s causing all the fuss, would you?",
                "That place at {questSo.GetRoomCoordinates()} hasn’t been explored yet, and I’m too busy to deal with it. Guess that means it’s your problem now.",
                "I don’t care how you do it, but get over at {questSo.GetRoomCoordinates()} and see what’s there. Just don’t make it my issue later.",
                "That unexplored zone at {questSo.GetRoomCoordinates()}? Yeah, it’s been bothering me. Go look around and let me know if it’s worth my time.",
                "If you’re not too busy standing around, maybe you could actually explore over at {questSo.GetRoomCoordinates()} for me. Someone’s gotta do it.",
                "The room at {questSo.GetRoomCoordinates()} is an eyesore on my plans. Go figure out what’s going on there so I can stop worrying about it."
                };
            }
        }

        protected override string [] averageSocialDialogues{
            get
            {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string[] {
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
                return new string[] {
                "I'm considering entering a painting contest, but I'm still deciding what to paint. I've heard there's a beautifull place in the coordinates {questSo.GetRoomCoordinates()}. Can you go there and check it out for me?",
                "Would you mind checking out that an area for me? I’ve heard there’s something interesting out there in the coordinates {questSo.GetRoomCoordinates()}, but I can’t go myself.",
                "There’s a place not far from here I’d like you to explore. In the coordinates {questSo.GetRoomCoordinates()}. Who knows what you might find, but it could be important.",
                "Could you head over to the coordinates {questSo.GetRoomCoordinates()} for me? I’ve got a hunch there’s something worth your time over there.",
                "I need someone to investigate the coordinates {questSo.GetRoomCoordinates()}. You seem capable, so if you’re up for it, could you explore it and let me know what you find?",
                "I’ve been curious about what’s beyond room {questSo.GetRoomCoordinates()}. Maybe you could take a look and see what’s going on over there?",
                "There’s an area up ahead that’s been untouched for a while. Ever heard of room {questSo.GetRoomCoordinates()}? Could you explore it and see if anything stands out?",
                "If you have some free time, could you go explore the coordinates {questSo.GetRoomCoordinates()}? I’ve heard rumors about strange things happening there.",
                "I’d appreciate it if you could take a trip to the coordinates {questSo.GetRoomCoordinates()}. I have a feeling there’s something there that could be of use.",
                "Take a look at the coordinates {questSo.GetRoomCoordinates()} when you get the chance. I’ve got a bad feeling about it, and I’d like you to check it out.",
                "I can’t go out there myself, but you seem like the adventurous type. Would you be willing to explore the coordinates {questSo.GetRoomCoordinates()} for me?"
                };
            }
        }

        protected override string [] highSocialDialogues{
            get {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string [] {
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
                return new string[] {
                "Oh, you’re here! Fantastic! So, there’s this spot at {questSo.GetRoomCoordinates()} that’s been tickling my curiosity for ages. Could you check it out? I’d go myself, but, you know, reasons!",
                "Hey, I’ve been staring at this map forever, and there’s something intriguing about {questSo.GetRoomCoordinates()}. Could you explore it for me? Oh, and don’t forget to take notes—I love details!",
                "Okay, so here’s the deal—I found these coordinates, {questSo.GetRoomCoordinates()}, and I have to know what’s there. You’ll check it out, right? Please? Pretty please?",
                "You’re just the person I was hoping to see! There’s a location at {questSo.GetRoomCoordinates()} that’s been bugging me. What if it’s treasure? Or something mysterious? Could you investigate?",
                "So, you won’t believe this, but I’ve heard rumors about {questSo.GetRoomCoordinates()}. Weird ones! Can you go and see what’s up? I’ll be waiting here, dying to know what you find!",
                "Okay, so I’ve been marking places on this map, and {questSo.GetRoomCoordinates()} just screams ‘adventure!’ Could you explore it? I’ll owe you big time—seriously!",
                "Oh, you have to help me! There’s a spot at {questSo.GetRoomCoordinates()} that’s been haunting my dreams. Okay, not literally, but I’m so curious. Can you check it out for me?",
                "I’ve got a feeling about {questSo.GetRoomCoordinates()}. Don’t ask why—it’s just a hunch! Could you take a look and see if my instincts are onto something?",
                "Oh, I’ve got the perfect task for you! There’s something special at {questSo.GetRoomCoordinates()}. Well, I think it’s special, but that’s where you come in—go explore it, will you?",
                "So, funny story! I stumbled upon these coordinates, {questSo.GetRoomCoordinates()}, and I just know there’s something fascinating there. Can you go check it out and let me know? I’m so excited to hear what you find!"
                };
            }
        }
    }
}