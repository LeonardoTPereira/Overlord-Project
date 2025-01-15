namespace Game.NPCs.PTBR
{
    public class ReportQuestCloser : QuestCloser
    {
        protected override string [] lowSocialClosers {
            get => new string [] {
                "Ugh, então você realmente fez isso... Bem, acho que o outro NPC já sabe agora. Não espere que eu seja todo agradecido, mas... tudo bem, obrigado.",
                "Demorou, né? Pelo menos agora a informação está com eles. Eu não achei que você fosse seguir até o fim, mas aqui estamos.",
                "Bem, está feito. Você passou a mensagem. Vou te dar o crédito onde é devido... só não espere que eu aplauda você.",
                "Ótimo, agora que eles sabem, posso seguir em frente. Obrigado... acho.",
                "Não sei por que você está procurando elogios. Você fez o que foi pedido, e agora podemos seguir em frente. Seja lá, obrigado.",
                "Você fez isso, e eles têm a informação. Não se acostume a ouvir ‘obrigado’ de mim, porém.",
                "Acho que está feito. Pelo menos alguém finalmente está recebendo a mensagem. De nada pela tarefa fácil, eu suponho.",
                "Você não estragou tudo... pela primeira vez. Acho que isso vale alguma coisa. Obrigado, mas não espere um desfile.",
                "Bem, foi rápido. Não sei se você merece uma medalha, mas aqui vai um obrigado de qualquer forma.",
                "Você fez seu trabalho. Eles têm a informação agora. Isso é tudo o que eu posso dizer."
            };
        }

        protected override string [] averageSocialClosers {
            get => new string [] {
                "Obrigado por passar isso para eles. Tenho certeza de que vai facilitar as coisas para todo mundo. Bom trabalho!",
                "Bom trabalho! Eles têm a informação agora. Agradeço por ter tirado um tempo para reportar isso.",
                "Obrigado por cuidar disso. Tenho certeza de que eles vão apreciar os detalhes. Você foi uma grande ajuda.",
                "Bem, isso foi rápido! Obrigado por garantir que eles recebam a informação. Você fez um bom trabalho.",
                "Bom trabalho, você fez exatamente o que era necessário. Eles têm a informação agora, então podemos seguir em frente.",
                "Agradeço por passar a mensagem. Agora que eles têm a informação, podemos seguir em frente.",
                "Ótimo trabalho, eles têm o que precisam. Tenho certeza de que isso ajudará muito daqui para frente. Obrigado!",
                "Obrigado por cuidar disso. Agora que eles sabem, as coisas vão andar muito mais suaves. Muito bem feito!",
                "Simples e direto, né? Obrigado por levar a informação onde ela precisava estar. Foi muito apreciado.",
                "Você fez exatamente o que era necessário. Tenho certeza de que eles vão achar útil. Obrigado por cuidar disso."
            };
        }

        protected override string [] highSocialClosers {
            get => new string [] {
                "Oh, fantástico! Você realmente fez um grande favor para nós, levando essa informação até eles. Tenho certeza de que eles vão adorar saber o que você descobriu. Ótimo trabalho!",
                "Olha só você! Passando informações importantes assim—agora eles podem começar a trabalhar nisso. Tenho certeza de que vai ajudar muito. Milhões de agradecimentos!",
                "Você não perdeu tempo, né? Tenho certeza de que eles vão ficar aliviados de ter todos os detalhes. Você realmente está fazendo a diferença por aqui!",
                "Agora que a informação foi passada, as coisas devem fluir muito melhor. Você economizou muito tempo para todo mundo, e todos nós somos gratos por isso. Obrigado!",
                "Oh, você realmente está em cima disso! Tenho certeza de que eles vão adorar ouvir o que você trouxe. Isso vai tornar tudo muito mais fácil. Grande obrigado!",
                "Você se saiu bem! Agora o outro NPC tem todas as informações de que precisa. Isso vai acelerar bastante as coisas, tenho certeza. Obrigado por cuidar disso!",
                "Excelente trabalho! O outro NPC vai ficar radiante ao ouvir todos os detalhes que você trouxe. Essa informação vai realmente mudar as coisas por aqui!",
                "Já consigo imaginar a cara deles quando receberem essa notícia! Você realmente fez um grande serviço, e não posso agradecer o suficiente. Você é um verdadeiro salvador!",
                "Agora finalmente podemos seguir em frente! A informação que você deu a eles fará toda a diferença. Estou tão feliz que você está do nosso lado—obrigado!",
                "Ah, agora você realmente fez! Passar a informação significa que finalmente podemos fazer algum progresso. Você foi indispensável para colocar tudo nos trilhos!"
            };
        }
    }
}
