namespace Game.NPCs.PTBR
{
    public class ExchangeQuestCloser : QuestCloser
    {
        protected override string [] lowSocialClosers {
            get => new string [] {
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

        protected override string [] averageSocialClosers {
            get => new string [] {
            "Obrigado por lidar com essa troca! {questSo.GetTargetNpc()} sempre dá boas recompensas, e tenho certeza de que você mereceu.",
            "Agradeço por cuidar disso! {questSo.GetTargetNpc()} nem sempre é fácil de lidar, mas aposto que a recompensa valeu a pena.",
            "Muito obrigado por fazer a troca! Tenho certeza de que você conseguiu algo ótimo. {questSo.GetTargetNpc()} sempre cumpre suas promessas.",
            "Você realmente me ajudou. Obrigado por levar isso a {questSo.GetTargetNpc()}. Tenho certeza de que a recompensa valeu o esforço.",
            "Foi um grande favor, e não posso te agradecer o suficiente! Espero que {questSo.GetTargetNpc()} tenha te dado uma recompensa que valeu a pena.",
            "Você fez exatamente o que eu precisava, e agradeço por isso. Tenho certeza de que {questSo.GetTargetNpc()} não decepcionou com a recompensa!",
            "Muito obrigado por completar a troca! {questSo.GetTargetNpc()} sempre tem algo especial para quem ajuda."
            };
        }

        protected override string [] highSocialClosers {
            get => new string [] {
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
        }
    }
}
