using Game.GameManager;
namespace Game.NPCs
{
    public class KillQuestCloser : QuestDialogue
    {
        protected override string[] lowSocialDialogues
        {
            get
            {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string[] {
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
                return new string[] {
                "Well, you went and killed them. Didn’t think you had it in you, but here you are. Thanks, I guess.",
                "You killed the enemies and came back to report? Fine. I’ll take it. Just don’t make me ask again.",
                "Huh. You actually did it. You killed them and came back with the details. Well, thanks, I suppose.",
                "I don’t usually give out praise, but you handled that. Thanks for killing those enemies and actually coming back with something useful.",
                "You did what I asked. Killed the enemies, got the report, and didn’t get yourself killed. Guess I should thank you for that.",
                "I expected you to mess it up, but you didn’t. Thanks for taking care of the enemies and actually making it back in one piece.",
                "You killed those things and came back. Fine, you did it. Thanks. But don’t think I’m going to be all nice about it.",
                "I don’t care much for formalities, but you did what I needed. You killed the enemies, came back, and reported. Thanks for that, I guess.",
                "I wasn’t sure you’d make it back alive, but here you are. You did the job and brought the report. Fine, thanks. Now move along.",
                "You actually did it—killed the enemies and returned with the info. Well, thanks for that, but don’t expect me to go on about it."
                };
            }
        }

        protected override string[] averageSocialDialogues
        {
            get
            {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string[] {
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
                return new string[] {
                "Thank you for handling those enemies. I wasn’t sure you’d make it back, but you did. Your report is exactly what we needed.",
                "I can’t believe you actually took them down and came back with all the information! Great job—this is going to help a lot.",
                "You did it! You killed those enemies and brought back the details we were waiting for. You’ve saved us a lot of trouble. Thank you!",
                "Well done! You really handled that well. The report you’ve brought back is invaluable. Thanks for taking care of those enemies for us.",
                "I’m impressed! Not only did you take down those enemies, but you also came back with all the necessary details. You’ve done more than I expected.",
                "Excellent work! I knew you could handle those enemies, but coming back with a full report like this? You’ve really exceeded expectations. Thank you.",
                "You’ve really come through for us. You killed the enemies and brought back exactly what we needed. This report is going to be a game-changer.",
                "I can’t thank you enough for your effort. You went out there, took care of the problem, and returned with everything we need. This will make things much easier.",
                "That was no easy task, but you did it. You handled the enemies and reported back with all the right information. Great work—thank you!",
                "Well, you certainly didn’t disappoint. You took down the enemies and gave us exactly the info we needed. I can’t thank you enough for your hard work."
                };
            }
        }

        protected override string[] highSocialDialogues
        {
            get
            {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string[] {
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
                return new string[] {
                "Oh, you’ve really done it this time! You went out there, handled those enemies, and came back with all the details. That’s more than I could’ve hoped for. Honestly, you’ve saved us a lot of headaches. I owe you one!",
                "Well, well, well! You actually went out and killed those enemies without breaking a sweat, then came back with a full report. You’ve really outdone yourself. You’ve got a knack for this kind of thing, don’t you?",
                "I’ve got to hand it to you, I was a little skeptical, but you’ve proven me wrong! Not only did you take down those enemies, but you’ve returned with such detailed information. I can’t tell you how much this is going to help.",
                "You know, I expected you to make a mess of things, but here you are, killing enemies left and right and bringing back a comprehensive report. You’ve really outdone yourself—thanks a ton for that!",
                "I was starting to think no one would be able to get rid of those pests, but you—wow! You not only wiped them out, but you also came back with all the info we need. You’re a real asset, you know that?",
                "You really went above and beyond, didn’t you? You didn’t just kill those enemies—you came back with everything I needed. It’s rare to find someone so reliable. Thanks a bunch for this!",
                "I was almost expecting you to get lost out there, but look at you! You took down those enemies, came back with a full report, and didn’t even break a sweat. I’m impressed—thank you so much!",
                "That was impressive, I won’t lie. Not only did you go out there and deal with those enemies, but you also brought back a detailed report. We’ll be able to make some real progress with this. Thanks, you really made my day!",
                "Well, you’ve certainly outdone yourself! You went straight into danger, took care of those enemies, and returned with all the information I could ever need. I’m just in awe. Thanks for being so thorough!",
                "I was a little worried about you, but you’ve proven me wrong. You took down the enemies, made it back, and even brought me all the details I asked for. I can’t even tell you how much I appreciate this. You’ve made my job so much easier!"
                };
            }
        }
    }
}