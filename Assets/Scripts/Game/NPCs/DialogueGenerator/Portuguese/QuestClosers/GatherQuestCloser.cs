namespace Game.NPCs.PTBR
{
    public class GatherQuestCloser : QuestCloser
    {
        protected override string [] lowSocialClosers {
            get => new string [] {
            "Huh. Você encontrou as coisas. Não achei que você conseguiria. Obrigado, acho.",
            "Bem, você conseguiu reunir tudo. Vou te dar crédito por isso. Agora, me deixe em paz.",
            "Você não estragou tudo. Aqui está o seu ‘obrigado,’ seja lá o que isso vale.",
            "Você realmente encontrou os itens... Sabe, eu não esperava que você conseguisse. Então, obrigado, acho.",
            "Demorou bastante. Mas, ainda assim, você conseguiu os itens. Vou te agradecer, mas não se acostume.",
            "Você encontrou todos. Bem, é melhor do que eu esperava. Obrigado, acho.",
            "Certo, você fez isso. Não sei como, mas fez. Obrigado por reuni-los."
            };
        }

        protected override string [] averageSocialClosers {
            get => new string [] {
            "Você conseguiu! Eu realmente aprecio você ter reunido tudo isso. Isso será de grande ajuda daqui para frente.",
            "Você realmente se superou! Obrigado por conseguir tudo o que pedi—isso será muito útil.",
            "Maravilhoso! Você reuniu tudo o que eu precisava. Sou realmente grato pela sua ajuda com isso!",
            "Não consigo te agradecer o suficiente por isso! Você reuniu todos os itens tão rápido e eficientemente. Excelente trabalho!",
            "Você realmente cumpriu sua missão! Obrigado por reunir tudo e me mostrar. Tenho certeza de que isso fará a diferença.",
            "Trabalho fantástico! Você reuniu tudo o que pedi, e não poderia estar mais agradecido. Muito obrigado!"
            };
        }

        protected override string [] highSocialClosers {
            get => new string [] {
            "Oh, uau, você realmente encontrou todos eles! Estou tão feliz que você conseguiu localizá-los! Você não faz ideia de como as coisas vão ficar mais fáceis agora. Você é um salvador!",
            "Olhe para tudo isso! Não acredito que você encontrou tudo—bem, na verdade, acredito, conhecendo você! Você realmente se superou, não é? Vou começar a usar isso imediatamente!",
            "Bem, bem, bem! Você conseguiu, não conseguiu? Encontrou cada coisa que eu pedi! Você não tem ideia de quanto isso ajuda, mal posso esperar para colocar tudo isso em bom uso!",
            "Oh, você realmente encontrou todos! Honestamente, não achei que você conseguiria, mas você foi lá e provou que eu estava errado! Isso vai ser uma grande ajuda, realmente não consigo te agradecer o suficiente!",
            "Uau, olhe para tudo isso! Você reuniu cada um deles! Isso é dedicação de verdade. Já estou pensando em todas as coisas que podemos fazer com isso!"
            };
        }
    }
}