using Game.GameManager;

namespace Game.NPCs
{
    public class ExchangeQuestCloser : QuestDialogue
    {
        protected override string[] lowSocialDialogues
        {
            get
            {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                {
                    return new string[] {
                    "Hmph. Acho que devo te agradecer por fazer essa tarefa. Você pegou a recompensa, certo? Não espere mais favores de mim.",
                    "Bem, você realmente fez isso. Certo, obrigado por lidar com a troca. Imagino que tenha conseguido algo decente de {questSo.GetTargetNpc()}, não foi?",
                    "É, é, obrigado por fazer a troca com {questSo.GetTargetNpc()}. Eu não esperava que você realmente fosse fazer isso, mas aqui estamos.",
                    "Tanto faz, você fez o trabalho. Obrigado, acho. A recompensa foi boa ou só mais tralha?",
                    "Eu não achei que você realmente fosse fazer isso, mas fez. Obrigado, acho. {questSo.GetTargetNpc()} te deu algo que valeu o esforço?",
                    "Então, você fez a troca. Certo, você fez o que pedi. Obrigado, suponho. Espero que {questSo.GetTargetNpc()} tenha te dado algo decente.",
                    "Bem, é uma coisa a menos com que me preocupar. Bom trabalho com a troca, embora eu tenha certeza de que você não fez isso só por mim.",
                    "Admito, estou surpreso que você cumpriu. Obrigado, mas não se anime muito. {questSo.GetTargetNpc()} te deu algo útil?",
                    "Ugh. Você realmente fez isso. Obrigado pela troca. Provavelmente conseguiu algo bom disso, certo?",
                    "Huh. Não esperava que você fosse até o fim. Não vou te dar nada além do meu obrigado. Espero que você esteja satisfeito com a recompensa que {questSo.GetTargetNpc()} te deu."
                    };
                }
                else
                {
                    return new string[] {
                    "Hmph. I guess I should thank you for running that errand. You got the reward, right? Don’t expect any more favors from me.",
                    "Well, you actually did it. Fine, thanks for handling that trade. I suppose you got something decent from {questSo.GetTargetNpc()}, didn’t you?",
                    "Yeah, yeah, thanks for doing the trade with {questSo.GetTargetNpc()}. I wasn’t expecting you to actually follow through, but here we are.",
                    "Whatever, you got the job done. Thanks, I guess. Was the reward any good, or just more junk?",
                    "I didn’t think you'd actually go through with it, but you did. Thanks, I guess. Did {questSo.GetTargetNpc()} give you something worth the trouble?",
                    "So, you made the trade. Fine, you did what I asked. Thanks, I suppose. Hope {questSo.GetTargetNpc()} gave you something halfway decent.",
                    "Well, that’s one less thing for me to worry about. Nice job doing the trade, though I’m sure you didn’t do it just for me.",
                    "I’ll admit, I’m surprised you followed through. Thanks, but don’t get too excited. Did {questSo.GetTargetNpc()} give you anything useful?",
                    "Ugh. You actually went and did it. Thanks for the trade. You probably got something good out of it, right?",
                    "Huh. Didn’t expect you to go all the way. I won't give you anything other than my thanks. Hope you’re already happy enough with the reward {questSo.GetTargetNpc()} gave you."
                    };
                }
            }

        }

        protected override string[] averageSocialDialogues
        {
            get
            {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string[] {
                    "Obrigado por lidar com essa troca! {questSo.GetTargetNpc()} sempre dá boas recompensas, e tenho certeza de que você mereceu.",
                    "Agradeço por cuidar disso! {questSo.GetTargetNpc()} nem sempre é fácil de lidar, mas aposto que a recompensa valeu a pena.",
                    "Muito obrigado por fazer a troca! Tenho certeza de que você conseguiu algo ótimo. {questSo.GetTargetNpc()} sempre cumpre suas promessas.",
                    "Você realmente me ajudou. Obrigado por levar isso a {questSo.GetTargetNpc()}. Tenho certeza de que a recompensa valeu o esforço.",
                    "Foi um grande favor, e não posso te agradecer o suficiente! Espero que {questSo.GetTargetNpc()} tenha te dado uma recompensa que valeu a pena.",
                    "Você fez exatamente o que eu precisava, e agradeço por isso. Tenho certeza de que {questSo.GetTargetNpc()} não decepcionou com a recompensa!",
                    "Muito obrigado por completar a troca! {questSo.GetTargetNpc()} sempre tem algo especial para quem ajuda."
                    };

                return new string[] {
                    "Thank you for handling that trade! {questSo.GetTargetNpc()} always gives good rewards, and I’m sure you’ve earned it.",
                    "I appreciate you taking care of that! {questSo.GetTargetNpc()} isn’t the always easiest to deal with, but I bet their reward was worth it.",
                    "Thanks a lot for making the trade! I’m sure you got something great out of it. {questSo.GetTargetNpc()} always keeps their promises.",
                    "You’ve really helped me out. Thanks for taking that to {questSo.GetTargetNpc()}. I’m sure their reward was well worth the effort.",
                    "That was a huge favor, and I can’t thank you enough! I hope {questSo.GetTargetNpc()} gave you a reward that made it all worthwhile.",
                    "You did exactly what I needed, and I appreciate it. I’m sure {questSo.GetTargetNpc()} didn’t disappoint with the reward!",
                    "Thank you so much for going through with the trade! {questSo.GetTargetNpc()} always has something special for those who help."
                };
            }
        }

        protected override string[] highSocialDialogues
        {
            get
            {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string[] {
                    "Você é incrível! Obrigado por ajudar com essa troca. {questSo.GetTargetNpc()} tem as melhores recompensas, não é? Espero que você tenha conseguido algo incrível!",
                    "Não sei como te agradecer o suficiente por isso! {questSo.GetTargetNpc()} realmente sabe fazer um bom negócio, e tenho certeza de que eles te deram algo que vale seu tempo!",
                    "Uau, você realmente fez isso! Obrigado por levar isso para {questSo.GetTargetNpc()}. Aposto que a recompensa foi tão incrível quanto eu disse, né?",
                    "Ah, muito obrigado por lidar com essa troca! Eu sabia que {questSo.GetTargetNpc()} te daria algo ótimo em troca! Você é realmente um salvador!",
                    "Você conseguiu! Você fez a troca com {questSo.GetTargetNpc()}! Obrigado, obrigado! Espero que a recompensa deles tenha sido tudo o que você esperava e mais!",
                    "Você fez isso por mim! Muito obrigado por lidar com a troca com {questSo.GetTargetNpc()}. Tenho certeza de que eles te deram uma recompensa que valeu a pena!",
                    "Eu sabia que podia contar com você! Obrigado por tirar um tempo para trocar com {questSo.GetTargetNpc()}. Tenho certeza de que a recompensa foi tão boa quanto ouro, não foi?",
                    "Você é muito gentil! Obrigado por lidar com {questSo.GetTargetNpc()}. Eles sempre têm as melhores recompensas, e aposto que você conseguiu algo realmente especial!",
                    "Obrigado por fazer essa tarefa para mim! Espero que a recompensa de {questSo.GetTargetNpc()} tenha sido tão incrível quanto prometi. Você faz essas coisas parecerem fáceis!",
                    "Agradeço muito por lidar com a troca com {questSo.GetTargetNpc()}! Tenho certeza de que a recompensa valeu totalmente a pena—você tem um bom olho para essas coisas!"
                    };
                return new string[] {
                "You’re a gem! Thanks for helping out with that trade. {questSo.GetTargetNpc()} has the best rewards, doesn't they? I hope you got something incredible!",
                "I can’t thank you enough for doing that! {questSo.GetTargetNpc()} sure knows how to make a deal, and I’m sure they gave you something that’s worth your time!",
                "Wow, you actually went through with it! Thank you for taking that over to {questSo.GetTargetNpc()}. I bet their reward was just as amazing as I said, huh?",
                "Oh, thank you so much for handling that trade! I just knew {questSo.GetTargetNpc()} would give you something great in return! You're a real lifesaver!",
                "You did it! You made the trade with {questSo.GetTargetNpc()}! Thank you, thank you! I hope their reward was everything you hoped for and more!",
                "You went all the way for me! Thanks a ton for handling the trade with {questSo.GetTargetNpc()}. I’m sure they gave you a reward that made it all worthwhile!",
                "I knew I could count on you! Thanks for taking the time to trade with {questSo.GetTargetNpc()}. I’m sure their reward was as good as gold, wasn’t it?",
                "You’re too kind! Thanks for dealing with {questSo.GetTargetNpc()}. They always have the best rewards, and I bet you got something really special!",
                "Thank you for running that errand for me! I hope {questSo.GetTargetNpc()}’s reward was just as amazing as I promised. You make these things look easy!",
                "I really appreciate you handling the trade with {questSo.GetTargetNpc()}! I’m sure their reward was totally worth it—you’ve got a good eye for these things!"
                };

            }
        }
    }
}