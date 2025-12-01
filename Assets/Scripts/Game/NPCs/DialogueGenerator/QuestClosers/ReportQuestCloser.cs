using Game.GameManager;
namespace Game.NPCs
{
    public class ReportQuestCloser : QuestDialogue
    {
        protected override string[] lowSocialDialogues
        {
            get
            {
                if (_language == Util.Enums.Language.Portuguese)
                    return new string[] {
                    "Ugh, então você realmente fez isso... Bem, acho que o outro NPC já sabe agora. Não espere que eu seja todo agradecido, mas... tudo bem, obrigado.",
                    "Demorou, né? Pelo menos agora a informação está com {questSo.GetTargetNpc()}. Eu não achei que você fosse seguir até o fim, mas aqui estamos.",
                    "Bem, está feito. Você passou a mensagem. Vou te dar o crédito onde é devido... só não espere que eu aplauda você.",
                    "Ótimo, agora que {questSo.GetTargetNpc()} sabe, posso seguir em frente. Obrigado... acho.",
                    "Não sei por que você está procurando elogios. Você fez o que foi pedido, e agora podemos seguir em frente. Seja lá, obrigado.",
                    "Você fez isso, e {questSo.GetTargetNpc()} tem a informação. Mas não se acostume a ouvir ‘obrigado’ de mim, não.",
                    "Acho que está feito. Pelo menos alguém finalmente está recebendo a mensagem. De nada pela tarefa fácil, eu acho.",
                    "Você não estragou tudo... pela primeira vez. Acho que isso vale alguma coisa. Obrigado, mas não espere mais do que isso.",
                    "Bem, foi rápido. Não sei se você merece uma medalha, mas aqui vai um obrigado de qualquer forma.",
                    "Você fez seu trabalho. {questSo.GetTargetNpc()} têm a informação agora. Isso é tudo o que preciso."
                    };
                return new string[] {
                "Ugh, so you actually did it... Well, I guess the other NPC knows now. Don’t expect me to be all grateful, but... fine, thanks.",
                "Took you long enough, huh? At least the info’s with them now. I didn’t think you'd follow through, but here we are.",
                "Well, it’s done. You passed the message. I’ll give you credit where it’s due... just don’t expect me to cheer for you.",
                "Great, now that they know, I can move on. Thanks... I guess.",
                "I don’t know why you’re looking for praise. You did what was asked, and now we can move on. Whatever, thanks.",
                "You did it, and they’ve got the info. Don’t get used to hearing ‘thank you’ from me though.",
                "I guess it’s done. At least someone’s finally getting the message. You’re welcome for the easy task, I suppose.",
                "You didn’t mess it up... for once. I suppose that’s worth something. Thanks, but don’t expect a parade.",
                "Well, that was quick. I don’t know if you deserve a medal, but here’s a thanks anyway.",
                "You did your job. They have the information now. That’s all I can say."
                };
            }
        }

        protected override string[] averageSocialDialogues
        {
            get
            {
                if (_language == Util.Enums.Language.Portuguese)
                    return new string[] {
                    "Obrigado por passar isso para {questSo.GetTargetNpc()}. Tenho certeza de que vai facilitar as coisas para todo mundo. Bom trabalho!",
                    "Bom trabalho! {questSo.GetTargetNpc()} tem a informação agora. Agradeço por ter tirado um tempo para reportar isso.",
                    "Obrigado por cuidar disso. Tenho certeza de que {questSo.GetTargetNpc()} apreciou os detalhes. Você foi uma grande ajuda.",
                    "Bem, isso foi rápido! Obrigado por garantir que {questSo.GetTargetNpc()} recebeu a informação. Você fez um bom trabalho.",
                    "Bom trabalho, você fez exatamente o que era necessário. {questSo.GetTargetNpc()} tem a informação agora, então podemos seguir em frente.",
                    "Agradeço por passar a mensagem. Agora que {questSo.GetTargetNpc()} sabe o que está acontecendo, podemos seguir em frente com o próximo passo.",
                    "Ótimo trabalho, {questSo.GetTargetNpc()} tem o que precisa. Tenho certeza de que isso ajudará muito daqui para frente. Obrigado!",
                    "Obrigado por cuidar disso. Agora que {questSo.GetTargetNpc()} sabe, as coisas vão andar muito mais suaves. Muito bem feito!",
                    "Simples e direto, né? Obrigado por levar a informação onde ela precisava estar.",
                    "Você fez exatamente o que era necessário. Tenho certeza de que {questSo.GetTargetNpc()} vai achar útil. Obrigado por cuidar disso."
                    };
                return new string[] {
                "Thanks for passing that along to them. I’m sure it’ll make things easier for everyone. Nice work!",
                "Good job! They’ve got the info now. I appreciate you taking the time to report it back.",
                "Thanks for handling that. I’m sure they’ll appreciate the details. You've been a real help.",
                "Well, that was quick! Thanks for making sure they got the information. You’ve done good work.",
                "Nice work, you’ve done exactly what was needed. They have the info now, so we can move on.",
                "Appreciate you passing along that message. Now they’ve got the info, and we can move ahead.",
                "Great job, they’ve got what they need. I’m sure that’ll help a lot moving forward. Thanks!",
                "Thanks for taking care of that. Now that they know, things should go a lot smoother. Well done!",
                "Nice and simple, huh? Thanks for getting the information where it needed to go. It’s much appreciated.",
                "You did exactly what was needed. I’m sure they’ll find it helpful. Thanks for taking care of it."
                };
            }
        }

        protected override string[] highSocialDialogues
        {
            get
            {
                if (_language == Util.Enums.Language.Portuguese)
                    return new string[] {
                    "Oh, fantástico! Você realmente fez um grande favor para nós, levando essa informação até {questSo.GetTargetNpc()}. Tenho certeza de que {questSo.GetTargetNpc()} adorou saber o que você descobriu. Ótimo trabalho!",
                    "Olha só você! Passando informações importantes assim—agora {questSo.GetTargetNpc()} pode começar a trabalhar nisso. Tenho certeza de que vai ajudar muito. Milhões de muitos obrigados!",
                    "Você não perdeu tempo, né? Tenho certeza de que {questSo.GetTargetNpc()} está aliviados de ter todos os detalhes. Você realmente está fazendo a diferença por aqui!",
                    "Agora que a informação foi passada, as coisas devem fluir muito melhor. Você economizou muito tempo para todo mundo, e todos nós somos gratos por isso. Obrigado!",
                    "Oh, você realmente conseguiu! Tenho certeza de que {questSo.GetTargetNpc()} adorou ouvir o que você trouxe. Isso vai tornar tudo muito mais fácil. Grande obrigado!",
                    "Você se saiu bem! Agora {questSo.GetTargetNpc()} tem todas as informações de que precisa. Isso vai acelerar bastante as coisas, tenho certeza. Obrigado por cuidar disso!",
                    "Excelente trabalho! {questSo.GetTargetNpc()} deve ter ficado radiante ao ouvir todos os detalhes que você trouxe. Essa informação vai realmente mudar as coisas por aqui!",
                    "Já consigo imaginar a cara do {questSo.GetTargetNpc()} quando recebeu a notícia! Você realmente fez um grande serviço, e não posso agradecer o suficiente. Você é um verdadeiro heroi!",
                    "Agora finalmente podemos seguir em frente! A informação que você deu a {questSo.GetTargetNpc()} fará toda a diferença. Estou tão feliz que você está do nosso lado—obrigado!",
                    "Ah, agora você realmente fez! Passar a informação significa que finalmente podemos fazer algum progresso. Você foi indispensável para colocar tudo nos trilhos!"
                    };
                return new string[] {
                "Oh, fantastic! You’ve really done us a solid, getting that info to them. I’m sure they’ll be thrilled to know what you’ve uncovered. Great job!",
                "Well, look at you! Passing along important info like that—now they can get to work on it. I’m sure it’ll help a ton. Thanks a million!",
                "You didn’t waste any time, did you? I’m sure they’ll be so relieved to have all the details. You’re really making a difference around here!",
                "Now that the info’s been passed along, things should go much smoother. You’ve saved everyone a lot of time, and we’re all grateful for it. Thank you!",
                "Oh, you’re really on top of it! I’m sure they’re going to love hearing what you brought back. That’ll make everything so much easier. Big thanks!",
                "You’ve done great! Now my friend has all the info they need. This is going to speed things up quite a bit, I’m sure. Thanks for taking care of it!",
                "Excellent work! My friend will be over the moon to hear all the details you brought. This info will really change things around here!",
                "I can already imagine their face when they get this news! You’ve really done a great service, and I can’t thank you enough. You’re a real lifesaver!",
                "Now we can finally get moving! The info you gave them will make all the difference. I’m so glad you’re on our side—thank you!",
                "Ah, you’ve really done it now! Passing the info along means we can finally make some progress. You’ve been invaluable in getting everything on track!"
                };
            }
        }
    }
}