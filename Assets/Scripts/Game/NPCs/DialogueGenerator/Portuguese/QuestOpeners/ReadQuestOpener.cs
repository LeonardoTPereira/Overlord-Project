using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs.PTBR
{
    public class ReadQuestOpener : QuestOpener
    {
        protected override string [] lowSocialOpeners {
            get => new string [] {
                "Tem um livro por aí que você precisa encontrar. Não me pergunte onde—apenas vá lá e leia.",
                "Eu não sou seu bibliotecário, mas tem um pergaminho que você precisa desenterrar e ler. Vai lá e encontra logo.",
                "Se você quer respostas, tem um livro que você vai precisar rastrear e realmente ler. Sim, ler. Entendeu?",
                "Ugh, por que eu tenho que explicar tudo? Encontre o pergaminho e leia você mesmo. Não é meu problema.",
                "Tem um livro por aí que explica tudo o que você precisa. Vai encontrar, ler e parar de me incomodar.",
                "Você está procurando um pergaminho. Ele é importante. Quando encontrar, leia—desde que saiba ler."
            };
        }

        protected override string [] averageSocialOpeners{
            get => new string [] {
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
        }

        protected override string [] highSocialOpeners{
            get => new string [] {
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
        }
    }
}
