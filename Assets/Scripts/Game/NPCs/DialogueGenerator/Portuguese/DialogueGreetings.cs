using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs.PTBR
{
    public static class DialogueGreetings
    {
        static string [] lowSocialGreeting = {
            "...",
            "UGH, O que você quer?",
            "UGH, É você de novo... Como assim não nos conhecemos?",
            "Não posso ter um único momento de paz?",
            "Eu realmente não quero conversar agora.",
            "Não ouse me chamar de mal-humorado!",
            "O clima hoje está terrível...",
            "Não quero papo furado.",
            "Saia daqui.",
            "Eu pareço alguém que gostaria de estar falando com você agora?",
            "Estou ocupado.",
            "Você não tem nada melhor para fazer?",
            "Você não tem outro lugar para estar?",
            "Só mais um dia ruim...",
            "UGH, \"AVENTUREIROS\" hoje em dia....",
            "Perdeu algo na minha cara? Não? Então por que está me encarando?",
            "O que você ainda está fazendo aqui?",
            "O quê?",
            "Pff, não perca meu tempo.",
            "O que você É afinal?"
        };

        static string [] lowSocialIntro = {
            " Eu sou {speaker.NpcName}, o {speaker.Job}.",
            " Quem sou eu? Claro, {speaker.NpcName}, o {speaker.Job}. Quem mais você achou que eu poderia ser?",
            " Meu nome? {speaker.NpcName}. Você também quer saber minha profissão? O que vem depois, meu CPF? Meu cartão de crédito??",
            " Sou um {speaker.Job} de profissão. Então, se você precisar de algo relacionado a isso, pode vir a mim.",
            " ... Tudo bem, vou te contar sobre mim. Eu sou {speaker.NpcName}, o {speaker.Job}.",
            " Você deveria se considerar sortudo por respirar o mesmo ar que {speaker.NpcName}, o {speaker.Job}... Que sou eu...",
            " Eu tenho muito o que fazer como {speaker.Job}. As pessoas às vezes também me chamam de {speaker.NpcName}.",
            "Sou {speaker.NpcName}, o melhor {speaker.Job} que você encontrará, e não gosto de perder tempo."
        };

        static string [] averageSocialGreeting = {
            "Oi!",
            "Olá! Como você está?",
            "Não é muito comum ver aventureiros por aqui.",
            "Você já descobriu como sair daqui?",
            "Às vezes sinto falta de morar na cidade.",
            "Não te vejo há um tempo. O que você tem feito?",
            "Não estou muito ocupado, posso conversar um pouco.",
            "Estou todo ouvidos!",
            "O que tem acontecido ultimamente?",
            "Converse comigo.",
            "Como posso te ajudar?",
            "Só mais um dia.",
            "Se você descobrir como chegar à floresta, me avise.",
            "Você viu outras pessoas por aí?",
            "Eu nem sempre morei em uma masmorra, sabia?",
            "Eu costumava morar em uma grande cidade...",
            "Há mais do seu tipo por aí?",
            "ATCHIM!! Ops, desculpe por isso...",
            "Você se parece com alguém que eu costumava conhecer!"
        };

        static string [] averageSocialIntro = {
            " Eu sou {speaker.NpcName}, o {speaker.Job}.",
            " Meu nome é {speaker.NpcName} e trabalho como {speaker.Job}. Não estou totalmente certo sobre essa profissão, mas quem sabe o futuro?",
            " Você pode me chamar de {speaker.NpcName}. Trabalho como {speaker.Job} há tanto tempo quanto me lembro.",
            "As pessoas por aqui me chamam de {speaker.NpcName}, e me procuram sempre que precisam de um {speaker.Job}."
        };

        static string [] highSocialGreeting = {
            "Estou tão feliz em ver você!",
            "Não se preocupe, não estou ocupado!",
            "Algumas pessoas podem ser um pouco ranzinzas às vezes, mas não podemos culpá-las.",
            "Ah sim, EU AMO morar nesta masmorra.",
            "Hoje é um ótimo dia.",
            "O clima está bom.",
            "Prazer em conhecê-lo!",
            "YIPEE, um novo amigo!",
            "Sempre abro um sorriso quando vejo um aventureiro.",
            "Pensamentos felizes, vida feliz!",
            "Espero que você esteja tendo um dia incrível.",
            "Você fez algo bom para alguém hoje?",
            "Lembre-se de beber água! Mesmo preso em uma masmorra, hehe.",
            "Você já ouviu falar de alguém chamado \"JoJo\"? Acho que eles são populares por aqui...",
            "Como está seu dia?",
            "Você consegue imaginar derrotar um grande chefe só para descobrir que a princesa está em outro castelo? UFA....",
            "Acho que as pessoas não perguntam mais isso, mas qual é sua cor favorita?",
            "Você parece uma pessoa legal.",
            "O que te fez decidir ser um aventureiro?"
        };

        static string [] highSocialIntro = {
            "Sou {speaker.NpcName}, o {speaker.Job}.",
            "Eu realmente gosto de trabalhar como {speaker.Job}, mas me machuca um pouco quando as pessoas me chamam pelo título em vez do meu nome. {speaker.NpcName} não é tão difícil, né?",
            "Se você precisar de algo de um {speaker.Job}, é só falar com {speaker.NpcName}. Isso mesmo, EU!",
            "Meu nome? {speaker.NpcName}. Minha profissão? {speaker.Job}. Minhas informações de cartão de crédito? 3345-- Espera, não.",
            "Meu nome? {speaker.NpcName}. Minha profissão? {speaker.Job}. Meu pokémon favorito? Meowth, é claro!"
        };

        public static string CreateGreeting(NpcSo speaker)
        {
            switch (speaker.SocialFactor)
            {
                case < 3:
                    return GetGreetingAndIntro( lowSocialGreeting, lowSocialIntro, speaker );
                case < 5:
                    return GetGreetingAndIntro( averageSocialGreeting, averageSocialIntro, speaker );
                default:
                    return GetGreetingAndIntro( highSocialGreeting, highSocialIntro, speaker );
            }
        }

        private static string GetGreetingAndIntro( string[] greeting, string[] intro, NpcSo speaker )
        {
            var greetingAndIntro = new StringBuilder();
            int randomGreeting = Random.Range( 0, greeting.Length );
            int randomIntro = Random.Range( 0, intro.Length );

            greetingAndIntro.Append( greeting[randomGreeting] );
            greetingAndIntro.Append( 
                intro[randomIntro].Replace( "{speaker.NpcName}", speaker.NpcName ).Replace( "{speaker.Job}", speaker.Job.ToString() )
                );

            return greetingAndIntro.ToString();
        }
    }
}