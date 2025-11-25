using Game.GameManager;
namespace Game.NPCs
{
    public class ReadQuestOpener : QuestDialogue
    {
        protected override string [] lowSocialDialogues {
            get {
                if (_language == Util.Enums.Language.Portuguese)
                    return new string [] {
                    "Tem um livro por aí que você precisa encontrar. Não me pergunte onde—apenas vá lá e leia.",
                    "Eu não sou seu bibliotecário, mas tem um pergaminho que você precisa desenterrar e ler. Vai lá e encontra logo.",
                    "Se você quer respostas, tem um livro que você vai precisar rastrear e realmente ler. Sim, ler. Entendeu?",
                    "Ugh, por que eu tenho que explicar tudo? Encontre o pergaminho e leia você mesmo. Não é meu problema.",
                    "Tem um livro por aí que explica tudo o que você precisa. Vai encontrar, ler e parar de me incomodar.",
                    "Você está procurando um pergaminho. Ele é importante. Quando encontrar, leia—desde que saiba ler."
                    };
                return new string [] {
                "There’s a book out there you need to find. Don’t ask me where—just get it and read it.",
                "I’m not your librarian, but there’s a scroll you need to dig up and read. Go find it already.",
                "If you want answers, there’s a book you’ll need to track down and actually read. Yes, read. Got it?",
                "Ugh, why do I have to spell everything out? Find the scroll and read it yourself. It’s not my problem.",
                "There’s a book somewhere that explains everything you need. Go find it, read it, and stop bothering me.",
                "You’re looking for a scroll. It’s important. Once you find it, read it—assuming you can read."
                };
            }
        }

        protected override string [] averageSocialDialogues{
            get {
                if (_language == Util.Enums.Language.Portuguese)
                    return new string [] {
                    "Você sabia que ao redor dessa masmorra há livros mágicos e escritos espalhados? Ouvi falar de um bem aqui perto. Você pode me dizer o que tem nele?",
                    "Estou tentando dominar essa nova técnica mágica e ouvi dizer que tem um livro sobre isso. Se encontrar, pode me dizer o que ele diz?",
                    "Tem um livro que guarda o conhecimento que precisamos. Você pode encontrá-lo e ler suas páginas para mim?",
                    "Ouvi falar de um pergaminho com respostas para nossos problemas. Você pode localizá-lo e ver o que diz?",
                    "Estamos faltando uma peça chave de informação. Tem um livro por aí—encontre, leia e me avise o que descobrir.",
                    "Tem um pergaminho que dizem estar nas ruínas. Se você conseguir encontrá-lo e lê-lo, pode ser a chave para resolver isso.",
                    "Eu preciso de alguém com olhos afiados e mente esperta. Você pode encontrar um certo livro e ler com atenção? É vital.",
                    "Lendas falam de um pergaminho escondido nos arquivos da biblioteca. Encontre-o, leia e traga seus segredos para mim.",
                    "Tem um livro antigo que contém as respostas que buscamos. Você pode localizá-lo e ver que sabedoria ele guarda?",
                    "Lá fora, tem um pergaminho com as informações que precisamos. Por favor, encontre-o, leia e volte com o que aprendeu.",
                    "Tem um livro antigo que guarda a verdade que estamos procurando. Se você conseguir encontrá-lo e lê-lo, estaremos um passo mais perto.",
                    "As respostas estão em um pergaminho escondido em algum lugar. Você consegue rastreá-lo, ler e me contar o que diz?"
                    };
                return new string [] {
                "Did you know that around this dungeon there are magical books and scripts lying around? I've heard about one just near here. Could you tell me what's in it?",
                "I'm trying to master this new magical technique and I've heard there's a book with it. If you find it, could you tell me what it says?",
                "There’s a book that holds the knowledge we need. Can you find it and read through its pages for me?",
                "I’ve heard of a scroll with answers to our problems. Can you locate it and see what it says?",
                "We’re missing a key piece of information. There’s a book out there—find it, read it, and let me know what you discover.",
                "There’s a scroll rumored to be in the ruins. If you can find it and read it, it might just hold the key to solving this.",
                "I need someone with sharp eyes and sharper wits. Can you find a certain book and read it carefully? It’s vital.",
                "Legends speak of a scroll hidden in the library archives. Find it, read it, and bring its secrets back to me.",
                "There’s an ancient book containing the answers we seek. Could you locate it and see what wisdom it holds?",
                "Somewhere out there is a scroll with the information we need. Please find it, read it, and return with what you’ve learned.",
                "There’s an old book that holds the truth we’ve been searching for. If you can find it and read it, we’ll be one step closer.",
                "The answers lie in a scroll tucked away somewhere. Can you track it down, read it, and tell me what it says?"
                };
            }
        }

        protected override string [] highSocialDialogues{
            get {
                if (_language == Util.Enums.Language.Portuguese)
                    return new string [] {
                    "Oh! Eu acabei de lembrar, tem esse livro fascinante—acho que está escondido na biblioteca—ou talvez nas antigas ruínas? Enfim, você tem que encontrar e ler! É muito importante!",
                    "Então, tem esse pergaminho, antigo e misterioso, que dizem conter segredos que ninguém jamais entendeu totalmente! Você consegue encontrá-lo e lê-lo para mim? Eu mal posso esperar para saber o que diz!",
                    "Certo, escute! Tem um livro lá fora, cheio de conhecimento e talvez algumas surpresas. Você deveria encontrá-lo e ler cada última palavra—depois, claro, venha me contar tudo!",
                    "Você vai adorar isso! Em algum lugar lá fora tem um pergaminho que guarda as respostas que estamos procurando! Você pode encontrá-lo? Ah, e não esquece de ler com atenção—não deixe passar nada!",
                    "Ouvi um rumor sobre um livro antigo escondido nas ruínas. Dizem que é super importante! Você pode ir encontrar, ler e me contar tudo? Quero dizer, tudo!",
                    "Oh, isso é empolgante! Tem um pergaminho que absolutamente precisamos—pode estar escondido, empoeirado, ou ser bem antigo! Vai lá encontrar, ler e me contar o que diz. Estou morrendo de curiosidade!",
                    "Então, tem esse livro, e é meio que uma grande coisa. Cheio de sabedoria, segredos, talvez até feitiços? Eu não sei! Mas você tem que encontrar, ler e voltar com todos os detalhes suculentos!",
                    "Ok, imagine isso: um pergaminho antigo, escondido, contendo informações vitais para nós. Você pode rastreá-lo, ler e me dar um relatório completo? Eu vou esperar ansiosamente!",
                    "Oh, isso é empolgante! Tem um livro por aí, cheio de insights misteriosos. Eu preciso que você o encontre, leia e me conte tudo sobre ele. Não deixe escapar nenhuma palavra!",
                    "Tem um pergaminho lá fora que é absolutamente crucial para nossa missão—ou talvez seja só muito interessante! De qualquer forma, você pode encontrá-lo, ler e depois voltar e me contar tudo?"
                    };
                return new string [] {
                "Oh! I just remembered, there’s this fascinating book—I think it’s tucked away in the library—or maybe the old ruins? Anyway, you have to find it and read it! It’s really important!",
                "So, there’s this scroll, ancient and mysterious, and it’s said to contain secrets no one has ever fully understood! Can you find it and read it for me? I can’t wait to hear what it says!",
                "Alright, listen! There’s a book out there, full of knowledge and possibly a few surprises. You should find it and read every last word—then come back and tell me all about it, of course!",
                "You’re going to love this! Somewhere out there is a scroll that holds the answers we’ve been looking for! Can you find it? Oh, and make sure you read it carefully—don’t miss a thing!",
                "I heard a rumor about an old book hidden in the ruins. It’s supposed to be super important! Can you go find it, read it, and tell me everything? I mean everything!",
                "Oh, this is exciting! There’s a scroll we absolutely need—it might be hidden, or dusty, or ancient! Go find it, give it a good read, and let me know what it says. I’m dying to know!",
                "So, there’s this book, and it’s kind of a big deal. Full of wisdom, secrets, maybe some spells? I don’t know! But you have to find it, read it, and come back with all the juicy details!",
                "Okay, picture this: an old scroll, hidden away, containing vital information for us. Can you track it down, read it, and give me a full report? I’ll be waiting eagerly!",
                "Oh, this is thrilling! There’s a book somewhere out there, full of mysterious insights. I need you to find it, read it, and tell me all about it. Don’t leave out a single word!",
                "There’s a scroll out there that’s absolutely critical to our mission—or maybe it’s just really interesting! Either way, can you find it, read it, and then come back and tell me everything?"
                };
            }
        }
    }
}