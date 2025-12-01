using Game.GameManager;

namespace Game.NPCs
{
    public class GatherQuestOpener : QuestDialogue
    {
        protected override string[] lowSocialDialogues
        {
            get {
                if (_language == Util.Enums.Language.Portuguese)
                    return new string [] {
                    "Nossa, eu me pergunto como esse lugar ficou tão bagunçado em primeiro lugar. Seja útil e colete os {questSo.GetItemAmountString()} que estão por aí.",
                    "Eu preciso de {questSo.GetItemAmountString()} para um feitiço. Que feitiço? Não é da sua conta. Você pode até ficar com os {questSo.GetItemString()}.",
                    "Ótimo, mais trabalho para mim—mas estou jogando isso para você. Vai lá e coleta {questSo.GetItemAmountString()}, e não me faça esperar muito.",
                    "Você ainda está aqui? Ótimo, porque eu preciso de {questSo.GetItemAmountString()}. Acha que consegue lidar com isso, ou é muito para você?",
                    "Quer ser útil? Beleza, vai lá e coleta {questSo.GetItemAmountString()} para mim. Se você for rápido, talvez eu até diga obrigado. Talvez.",
                    "Olha, eu não tenho energia para explicar o porquê. Só colete {questSo.GetItemAmountString()} e pare de fazer perguntas."
                    };
                return new string[] {
                "Gosh, I wonder how this place even got so messy in the first place. Make yourself of use and gather the {questSo.GetItemAmountString()} that are lying around.",
                "I need {questSo.GetItemAmountString()} for a spell. What spell? None of your business. You can even keep the {questSo.GetItemString()}.",
                "Great, more work for me—except I’m pawning it off on you. Go gather {questSo.GetItemAmountString()}, and don’t make me wait too long.",
                "You’re still here? Good, because I need {questSo.GetItemAmountString()}. Think you can handle that, or is it too much for you?",
                "You want to be useful? Fine, go gather {questSo.GetItemAmountString()} for me. If you’re quick about it, maybe I’ll say thanks. Maybe.",
                "Look, I don’t have the energy to explain why. Just get {questSo.GetItemAmountString()} and stop asking questions."
                };
            }
        }

        protected override string[] averageSocialDialogues
        {
            get {
                if (_language == Util.Enums.Language.Portuguese)
                    return new string [] {
                    "Esse lugar está uma bagunça! Você poderia, por favor, coletar os {questSo.GetItemAmountString()} que estão por aí?",
                    "Eu estou começando a estudar {questSo.GetItemString()}. Você poderia me trazer {questSo.GetItemAmountString()} para eu dar uma olhada? Você pode ficar com eles, eu só preciso estudar um pouco…",
                    "Eu realmente sinto falta da minha mãe. Estava lendo sobre um feitiço de comunicação, mas eu precisaria de {questSo.GetItemAmountString()}. Você poderia pegar isso para mim? Oh, obrigado! Eu não tenho muito a oferecer, mas você pode ficar com os {questSo.GetItemString()} como recompensa.",
                    "Você sabia que {questSo.GetItemString()} são itens mágicos? Dá para usar eles em muitos tipos de feitiços. Eu tenho tentado analisá-los, mas eu precisaria de {questSo.GetItemAmountString()}. Você pode ficar com eles como recompensa!",
                    "Você pode me ajudar? Estou procurando {questSo.GetItemAmountString()}, e você parece a pessoa perfeita para encontrar.",
                    "Ei, eu preciso de {questSo.GetItemAmountString()} para um projeto em que estou trabalhando. Você pode encontrá-los para mim?",
                    "Ei, você é bom em encontrar as coisas, certo? Eu preciso de {questSo.GetItemAmountString()}—poderia coletá-los para mim quando tiver a chance?"
                    };
                return new string[] {
                "This place is a real mess! Could you please gather the {questSo.GetItemAmountString()} that are lying around?",
                "I'm starting to study {questSo.GetItemString()}. Could you bring me {questSo.GetItemAmountString()} so I can take a look at them? You can keep them, I just want to study them for a few hours...",
                "I really miss my mom. I've been reading about this comunication spell, but I'd need {questSo.GetItemAmountString()}. You'd get that for me? Oh thanks! I don't have much to offer, but you can have the {questSo.GetItemString()} for yourself as a reward.",
                "Did you know {questSo.GetItemString()} are magical items? You can use them in many types of spells. I've been trying to analyse them but I would need {questSo.GetItemAmountString()}. You can keep them as a reward!",
                "Could you help me out? I’m looking for {questSo.GetItemAmountString()}, and you seem like the perfect person to track them down.",
                "Hey, I’m in need of {questSo.GetItemAmountString()} for a project I’m working on. Can you find them for me?",
                "Hey, you’re good at finding things, right? I need {questSo.GetItemAmountString()}—could you gather them for me when you get the chance?"
                };
            }
        }

        protected override string[] highSocialDialogues
        {
            get {
                if (_language == Util.Enums.Language.Portuguese)
                    return new string [] {
                    "Ah, sabe, eu venho pensando nisso há um tempo, e eu só preciso perguntar—você poderia coletar {questSo.GetItemAmountString()} para mim? Eles são tão brilhantes e raros! Eu faria isso, mas, bem, você é muito melhor nisso!",
                    "Você não vai acreditar, mas eu ouvi dizer que existem exatamente {questSo.GetItemAmountString()} por aí, esperando para serem encontrados! Você se importaria de coletá-los para mim? Imagina as possibilidades quando tivermos eles!",
                    "Você tem que me ajudar com isso! Eu preciso de {questSo.GetItemAmountString()}, e eu estou quebrando a cabeça tentando descobrir onde encontrá-los. Mas você? Ah, você é um caçador de tesouros natural!",
                    "Ai meu Deus, você está aqui! Que momento perfeito! Eu preciso de {questSo.GetItemAmountString()}, tipo, urgentemente. Bem, não urgente-urgente, mas sabe, logo. Você pode me ajudar?",
                    "Ok, então eu estava pensando, não seria incrível se tivéssemos {questSo.GetItemAmountString()}? Quero dizer, imagine todas as coisas incríveis que poderíamos fazer com eles! Você vai me ajudar a coletá-los, certo?",
                    "Ah, você está aqui! Maravilhoso! Eu tenho esse probleminha—bem, não é um problema exatamente, é mais uma oportunidade—eu preciso de {questSo.GetItemAmountString()}, e eu só sei que você é a pessoa certa para encontrá-los!",
                    "Posso te contar um segredo? Eu estou morrendo de vontade de pegar {questSo.GetItemAmountString()}! Eles são tão perfeitos para… ah, deixa pra lá. De qualquer forma, você pode coletá-los para mim? Por favor?",
                    "Então, uma história engraçada! Eu estava planejando coletar {questSo.GetItemAmountString()} eu mesma, mas aí eu me lembrei, 'Ah, espera, eu conheço alguém muito mais capaz!' Essa pessoa é você, aliás. Pode me ajudar?"
                    };
                return new string[] {
                "Oh, you know, I’ve been thinking about this for a while, and I just have to ask—could you gather {questSo.GetItemAmountString()} for me? They’re so sparkly and rare! I’d do it myself, but, well, you’re just so much better at this kind of thing!",
                "You won’t believe this, but I heard there are exactly {questSo.GetItemAmountString()} out there just waiting to be found! Would you mind collecting them for me? I mean, imagine the possibilities once we have them!",
                "You’ve got to help me with this! I need {questSo.GetItemAmountString()}, and I’ve been wracking my brain trying to figure out where to find them. But you? Oh, you’re a natural treasure hunter!",
                "Oh my gosh, you’re here! Perfect timing! I need {questSo.GetItemAmountString()}, like, urgently. Well, not urgent-urgent, but you know, soon-ish. Can you help me out?",
                "Okay, so I was thinking, wouldn’t it be amazing if we had {questSo.GetItemAmountString()}? I mean, just think about all the amazing things we could do with them! You’ll help me gather them, right?",
                "Oh, you’re here! Wonderful! I’ve got this little problem—well, it’s not a problem exactly, more of an opportunity—I need {questSo.GetItemAmountString()}, and I just know you’re the person to find them!",
                "Can I tell you a secret? I’ve been dying to get my hands on {questSo.GetItemAmountString()}! They’re just so perfect for… oh, never mind that part. Anyway, could you gather them for me? Pretty please?",
                "So, funny story! I was planning to collect {questSo.GetItemAmountString()} myself, but then I remembered, 'Oh wait, I know someone way more capable!' That’s you, by the way. Can you help me out?"
                };
            }
        }
    }
}