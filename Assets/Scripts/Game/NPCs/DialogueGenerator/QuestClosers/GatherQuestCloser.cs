using Game.GameManager;
namespace Game.NPCs
{
    public class GatherQuestCloser : QuestDialogue
    {
        protected override string[] lowSocialDialogues
        {
            get
            {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string[] {
                    "Huh. Você encontrou as coisas. Não achei que você conseguiria. Obrigado, acho.",
                    "Bem, você conseguiu reunir tudo. Vou te dar crédito por isso. Agora, me deixe em paz.",
                    "Você não estragou tudo. Aqui está o seu ‘obrigado,’ seja lá o que isso vale.",
                    "Você realmente encontrou os itens... Sabe, eu não esperava que você conseguisse. Então, obrigado, acho.",
                    "Demorou bastante. Mas, ainda assim, você conseguiu os itens. Vou te agradecer, mas não se acostume.",
                    "Você encontrou todos. Bem, é melhor do que eu esperava. Obrigado, acho.",
                    "Certo, você fez isso. Não sei como, mas fez. Obrigado por reuni-los."
                    };
                return new string[] {
                "Huh. You found the stuff. Didn’t think you’d pull it off. Thanks, I suppose.",
                "Well, you managed to gather everything. I’ll give you credit for that. Now, leave me be.",
                "You didn’t mess it up. Here’s your ‘thanks,’ whatever that’s worth.",
                "You actually found the items... You know, I didn’t expect you to come through. So, thanks, I guess.",
                "Took you long enough. Still, you got the items. I’ll thank you, but don’t make a habit of it.",
                "You found them all. Well, it’s better than I thought you’d do. Thanks, I guess.",
                "Alright, you did it. Not sure how, but you did. Thanks for gathering them."
                };
            }
        }

        protected override string[] averageSocialDialogues
        {
            get
            {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string[] {
                    "Você conseguiu! Eu realmente aprecio você ter reunido tudo isso. Isso será de grande ajuda daqui para frente.",
                    "Você realmente se superou! Obrigado por conseguir tudo o que pedi—isso será muito útil.",
                    "Maravilhoso! Você reuniu tudo o que eu precisava. Sou realmente grato pela sua ajuda com isso!",
                    "Não consigo te agradecer o suficiente por isso! Você reuniu todos os itens tão rápido e eficientemente. Excelente trabalho!",
                    "Você realmente cumpriu sua missão! Obrigado por reunir tudo e me mostrar. Tenho certeza de que isso fará a diferença.",
                    "Trabalho fantástico! Você reuniu tudo o que pedi, e não poderia estar mais agradecido. Muito obrigado!"
                    };
                return new string[] {
                "You did it! I really appreciate you gathering all of these. This will be a big help moving forward.",
                "You’ve really outdone yourself! Thanks for getting everything I asked for—these will come in handy.",
                "Wonderful! You gathered everything I needed. I’m truly grateful for your help with this!",
                "I can’t thank you enough for this! You gathered all the items so quickly and efficiently. Great job!",
                "You really came through! Thanks for gathering everything and showing it to me. I’m sure it’ll make a difference.",
                "Fantastic work! You gathered everything I asked for, and I couldn’t be more grateful. Thank you!"
                };
            }
        }

        protected override string[] highSocialDialogues
        {
            get
            {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string[] {
                    "Oh, uau, você realmente encontrou todos eles! Estou tão feliz que você conseguiu localizá-los! Você não faz ideia de como as coisas vão ficar mais fáceis agora. Você é um salvador!",
                    "Olhe para tudo isso! Não acredito que você encontrou tudo—bem, na verdade, acredito, conhecendo você! Você realmente se superou, não é? Vou começar a usar isso imediatamente!",
                    "Bem, bem, bem! Você conseguiu, não conseguiu? Encontrou cada coisa que eu pedi! Você não tem ideia de quanto isso ajuda, mal posso esperar para colocar tudo isso em bom uso!",
                    "Oh, você realmente encontrou todos! Honestamente, não achei que você conseguiria, mas você foi lá e provou que eu estava errado! Isso vai ser uma grande ajuda, realmente não consigo te agradecer o suficiente!",
                    "Uau, olhe para tudo isso! Você reuniu cada um deles! Isso é dedicação de verdade. Já estou pensando em todas as coisas que podemos fazer com isso!"
                    };
                return new string[] {
                "Oh wow, you actually found them all! I’m so glad you managed to track these down! You wouldn’t believe how much easier things are going to be now. You’re a lifesaver!",
                "Look at all of this! I can’t believe you found everything—well, actually, I can believe it, knowing you! You really outdid yourself, huh? I’ll get started with this right away!",
                "Well, well, well! You did it, didn’t you? Found every single thing I asked for! You have no idea how much this helps, I can’t wait to put all of this to good use!",
                "Oh, you did find them all! I honestly didn’t think you’d pull it off, but you went and proved me wrong! This is going to be a huge help, I really can’t thank you enough!",
                "Wow, look at all this! You gathered every last one of them! That’s some serious dedication. I’m already thinking of all the things we can do with this!"
                };
            }
        }
    }
}