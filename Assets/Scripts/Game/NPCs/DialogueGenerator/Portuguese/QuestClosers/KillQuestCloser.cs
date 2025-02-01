namespace Game.NPCs.PTBR
{
    public class KillQuestCloser : QuestCloser
    {
        protected override string [] lowSocialClosers {
            get => new string [] {
            "Bem, você foi e matou eles. Não achei que fosse capaz, mas aqui está você. Obrigado, acho.",
            "Você matou os inimigos e voltou para relatar? Certo. Eu aceito. Só não me faça pedir de novo.",
            "Huh. Você realmente fez isso. Matou eles e voltou com os detalhes. Bem, obrigado, suponho.",
            "Eu não costumo elogiar, mas você deu conta. Obrigado por matar aqueles inimigos e realmente trazer algo útil.",
            "Você fez o que pedi. Matou os inimigos, trouxe o relatório e não morreu. Acho que devo agradecer por isso.",
            "Eu esperava que você estragasse tudo, mas não o fez. Obrigado por cuidar dos inimigos e voltar inteiro.",
            "Você matou aqueles bichos e voltou. Certo, você conseguiu. Obrigado. Mas não espere que eu seja muito simpático.",
            "Eu não ligo muito para formalidades, mas você fez o que eu precisava. Matou os inimigos, voltou e relatou. Obrigado, acho.",
            "Eu não tinha certeza se você voltaria vivo, mas aqui está você. Fez o trabalho e trouxe o relatório. Certo, obrigado. Agora vá.",
            "Você realmente fez isso—matou os inimigos e voltou com as informações. Bem, obrigado por isso, mas não espere um discurso."
            };
        }

        protected override string [] averageSocialClosers {
            get => new string [] {
            "Obrigado por lidar com aqueles inimigos. Eu não tinha certeza se você voltaria, mas conseguiu. Seu relatório é exatamente o que precisávamos.",
            "Não acredito que você realmente os derrotou e voltou com todas as informações! Ótimo trabalho—isso vai ajudar muito.",
            "Você conseguiu! Matou aqueles inimigos e trouxe os detalhes que esperávamos. Você nos salvou de muitos problemas. Obrigado!",
            "Muito bem! Você realmente lidou com isso de forma excelente. O relatório que trouxe é inestimável. Obrigado por cuidar daqueles inimigos para nós.",
            "Estou impressionado! Não só você derrotou aqueles inimigos, mas também voltou com todos os detalhes necessários. Você fez mais do que eu esperava.",
            "Excelente trabalho! Eu sabia que você poderia lidar com aqueles inimigos, mas voltar com um relatório completo como esse? Você realmente superou as expectativas. Obrigado.",
            "Você realmente deu conta para nós. Matou os inimigos e trouxe exatamente o que precisávamos. Esse relatório vai mudar o jogo.",
            "Eu não sei como agradecer pelo seu esforço. Você foi lá, cuidou do problema e voltou com tudo o que precisávamos. Isso vai facilitar muito as coisas.",
            "Não foi uma tarefa fácil, mas você fez. Cuidou dos inimigos e trouxe todas as informações corretas. Ótimo trabalho—obrigado!",
            "Bem, você certamente não nos decepcionou. Derrotou os inimigos e nos trouxe exatamente as informações que precisávamos. Não posso agradecer o suficiente pelo seu trabalho árduo."
            };
        }

        protected override string [] highSocialClosers {
            get => new string [] {
            "Ah, você realmente fez isso agora! Você foi lá, lidou com aqueles inimigos e voltou com todos os detalhes. Isso é mais do que eu poderia esperar. Honestamente, você nos salvou de muitas dores de cabeça. Eu te devo uma!",
            "Bem, bem, bem! Você realmente foi lá e matou aqueles inimigos sem suar, e depois voltou com um relatório completo. Você realmente se superou. Você tem talento para esse tipo de coisa, não é?",
            "Eu tenho que te dar mérito, eu estava um pouco cético, mas você me provou errado! Não só derrotou aqueles inimigos, mas voltou com informações tão detalhadas. Eu não posso te dizer o quanto isso vai ajudar.",
            "Sabe, eu esperava que você estragasse tudo, mas aqui está você, matando inimigos para todos os lados e trazendo de volta um relatório completo. Você realmente se superou—muito obrigado por isso!",
            "Eu estava começando a achar que ninguém conseguiria se livrar desses inimigos, mas você—uau! Você não só os exterminou, como também voltou com todas as informações que precisávamos. Você é um verdadeiro recurso, sabia disso?",
            "Você realmente foi além, não foi? Você não só matou aqueles inimigos—voltou com tudo o que eu precisava. É raro encontrar alguém tão confiável. Muito obrigado por isso!",
            "Eu quase esperava que você se perdesse por aí, mas olha só! Você matou aqueles inimigos, voltou com um relatório completo e nem suou. Estou impressionado—muito obrigado!",
            "Isso foi impressionante, não vou mentir. Não só você foi lá e lidou com aqueles inimigos, como também trouxe de volta um relatório detalhado. Agora podemos realmente fazer progresso com isso. Obrigado, você realmente fez o meu dia!",
            "Bem, você realmente se superou! Foi direto para o perigo, cuidou daqueles inimigos e voltou com todas as informações que eu poderia precisar. Estou realmente maravilhado. Obrigado por ser tão detalhista!",
            "Eu estava um pouco preocupado com você, mas você me provou errado. Derrotou os inimigos, voltou e ainda trouxe todos os detalhes que pedi. Nem sei como agradecer. Você tornou meu trabalho muito mais fácil!"
            };
        }
    }
}