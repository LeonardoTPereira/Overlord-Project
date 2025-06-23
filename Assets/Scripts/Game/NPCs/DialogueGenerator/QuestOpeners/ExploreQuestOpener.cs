using Game.GameManager;
namespace Game.NPCs
{
    public class ExploreQuestOpener : QuestDialogue
    {
        protected override string [] lowSocialDialogues{
            get {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string [] {
                    "Já faz um tempo desde que deixei minhas funções aqui como {questSo.Npc.Job}. Nem sei se essa masmorra tem {questSo.GetRoomAmount()} cômodos. ... Você pode confirmar isso?",
                    "Ótimo, outra coisa que eu não posso fazer. Vá procurar {questSo.GetRoomAmount()} cômodos nesta masmorra e me avise o que encontrou.",
                    "Eu preciso de alguém para investigar {questSo.GetRoomAmount()} cômodos, e você, por sorte, é o único disponível. Vai lá.",
                    "Tem {questSo.GetRoomAmount()} cômodos que precisam ser investigados, e eu estou ocupado demais. Adivinha? Agora é seu problema.",
                    "Eu não estou afim de lidar com isso, então você ficou com a tarefa. Vá verificar {questSo.GetRoomAmount()} cômodos e me diga o que encontrou. Dizem que essa masmorra muda cada vez que você entra..."
                    };
                return new string [] {
                "It's been a while since I left my duties here as a {questSo.Npc.Job}. I don't even know if this dungeon even has {questSo.GetRoomAmount()} rooms. ... Could you confirm that?",
                "Great, another thing I can’t do myself. Go search {questSo.GetRoomAmount()} rooms in this dungeon and let me know what’s there.",
                "I need someone to investigate {questSo.GetRoomAmount()} rooms, and lucky you, you’re the only one available. Get to it.",
                "There are {questSo.GetRoomAmount()} rooms that need looking into, and I’m too busy for it. So guess what? It’s your problem now.",
                "I don’t feel like dealing with it myself, so you’re up. Go check out {questSo.GetRoomAmount()} rooms and report back. Rumors say this dungeon changes everytime you enter it..."
                };
            }
        }

        protected override string [] averageSocialDialogues{
            get {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string [] {
                    "Meu melhor amigo costumava ser cartógrafo. Ele vai me visitar em breve e eu estava pensando em preparar uma surpresa. Você pode ajudar? Eu queria fazer um mapa para explorarmos juntos, mas antes preciso confirmar o tamanho. Você poderia verificar se há pelo menos {questSo.GetRoomAmount()} cômodos aqui?",
                    "Às vezes eu fico imaginando como seria ser cartógrafo. Talvez eu devesse tentar fazer um mapa para mim mesmo. Não sei se {questSo.GetRoomAmount()} seria um mapa muito grande... Você poderia explorar {questSo.GetRoomAmount()} cômodos e me contar como são para eu poder começar meu mapa?",
                    "Se você tiver tempo, poderia explorar {questSo.GetRoomAmount()} cômodos na área? Eu ficaria muito mais tranquilo sabendo o que tem lá.",
                    "Eu preciso que alguém explore {questSo.GetRoomAmount()} cômodos lá. Quem sabe o que você vai encontrar, mas vale a pena dar uma olhada.",
                    "Ei, você poderia dar uma olhada em {questSo.GetRoomAmount()} cômodos nesta masmorra? Pode ser que haja algo útil lá."
                    };
                return new string [] {
                "My best friend used to be a cartographer. They are going to visit me soon and I was thinking about getting them a little surprise. Could you help? I wanted to make a map for us to explore together, but first I need to confirm the size. Could you check out if there are at least {questSo.GetRoomAmount()} rooms in this place?",
                "Sometimes I wonder how it'd be like to be a cartographer. Maybe I should try making a map for myself. I don't know if {questSo.GetRoomAmount()} would be too big of a map... Could you explore {questSo.GetRoomAmount()} rooms and tell me how they are so I can start working on my map?",
                "If you’ve got the time, could you explore {questSo.GetRoomAmount()} rooms in the area? I’d feel a lot better knowing what’s in there.",
                "I need someone to explore {questSo.GetRoomAmount()} rooms over there. Who knows what you’ll find, but it’s worth a look.",
                "Hey, could you scout through {questSo.GetRoomAmount()} rooms in this dungeon? There might be something useful in there."
                };
            }
        }

        protected override string[] highSocialDialogues {
            get {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string [] {
                    "Ei, você poderia me fazer um pequeno favor e explorar {questSo.GetRoomAmount()} cômodos nesta área? Eu iria eu mesmo, mas quem sabe o que tem lá! Eu só ia entrar em pânico!",
                    "Então, uma história engraçada—eu estava querendo olhar aqueles {questSo.GetRoomAmount()} cômodos, mas sempre aparece algo! Você pode cuidar disso para mim? Tenho certeza que são fascinantes!",
                    "Eu sei que é pedir demais, mas você poderia explorar {questSo.GetRoomAmount()} cômodos para mim? É que eu sempre fiquei me perguntando sobre eles e não aguento mais a curiosidade!",
                    "Você é a pessoa perfeita para isso! Você poderia dar uma olhada em {questSo.GetRoomAmount()} cômodos nesta masmorra? Eu sei que tem algo incrível esperando para ser encontrado!",
                    "Ah, isso é tão empolgante! Eu preciso que você explore {questSo.GetRoomAmount()} cômodos para mim—é muito misterioso para ignorar, e eu preciso saber o que tem lá!"
                    };
                return new string[] {
                "Hey, could you do me a tiny favor and explore {questSo.GetRoomAmount()} rooms in this area? I’d go myself, but who knows what’s in there! I’d just panic!",
                "So, funny story—I’ve been meaning to look into those {questSo.GetRoomAmount()} rooms, but something always comes up! Can you handle it for me? I’m sure they’re fascinating!",
                "I know it’s a bit much to ask, but could you explore {questSo.GetRoomAmount()} rooms for me? It’s just that I’ve been wondering about them forever and can’t stand the suspense!",
                "You’re the perfect person for this! Could you check out {questSo.GetRoomAmount()} rooms in this dungeon? I just know there’s something amazing waiting to be found!",
                "Oh, this is so exciting! I need you to explore {questSo.GetRoomAmount()} rooms for me—it’s just too mysterious to ignore, and I have to know what’s in there!"
                };
            }
        }
    }
}