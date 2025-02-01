namespace Game.NPCs.PTBR
{
    public class ReadQuestCloser : QuestCloser
    {
        protected override string [] lowSocialClosers {
            get => new string [] {
            "Hmph, eu não achava que você realmente ia se dar ao trabalho de ler isso. Mas, tudo bem. As informações sobre aquelas ruínas amaldiçoadas? Sim, é exatamente o que precisávamos. Agora podemos seguir em frente. Eu acho... obrigado.",
            "Você realmente leu isso? Não foi ruim. O pergaminho sobre criaturas mágicas? Tinha algumas informações boas. Fraquezas, hábitos, esse tipo de coisa. Enfim, obrigado, eu acho.",
            "Bom, você leu tudo. O livro sobre armadilhas antigas não era exatamente divertido, né? Mas, ei, pelo menos você encontrou o que precisávamos. Tá bom, eu admito, isso é útil.",
            "Você leu aquele livro velho e empoeirado? Seja lá, tudo bem. A parte sobre a queda do antigo reino? Sim, isso vai ajudar. Agora sabemos a quem culpar por toda essa bagunça. Obrigado, eu acho.",
            "Então, você leu tudo aquilo, né? O pergaminho sobre magia negra? É, não foi totalmente inútil. Pelo menos agora sabemos um pouco mais sobre o que estamos enfrentando. Obrigado, eu acho.",
            "Eu não achava que você teria paciência, mas teve. O livro sobre artefatos encantados? Tinha alguns pontos bons. Agora temos uma pista. Não se acostume a ouvir agradecimentos da minha parte, porém.",
            "Huh. Você realmente conseguiu passar por isso? O pergaminho sobre as antigas lendas—bem, tem os detalhes que precisamos. Não que eu tenha me surpreendido, mas, uh, obrigado.",
            "Então você leu isso, né? Eu não esperava isso. A história das barreiras mágicas? É útil. Pelo menos sabemos mais agora. Não espere que eu fique todo animado com isso, no entanto.",
            "Bom, me surpreendeu. Você realmente leu aquele livro antigo. A parte sobre as ruínas assombradas? Isso é o que precisávamos. Eu vou te dar essa, tá bom. Obrigado.",
            "Você foi lá e leu o pergaminho sobre maldições, né? Não achei que você fosse topar. Mas você voltou com boas informações sobre as fraquezas delas. Acho que devo um ‘obrigado’ por isso."
            };
        }

        protected override string [] averageSocialClosers {
            get => new string [] {
            "Você realmente leu tudo isso? Impressionante! Os detalhes do livro sobre os rituais antigos são exatamente o que eu estava procurando. Isso vai facilitar muito nosso próximo passo—obrigado pelo seu esforço!",
            "Ah, você realmente leu isso! O pergaminho sobre criaturas mágicas tinha uns detalhes fascinantes, não tinha? Agora sabemos mais sobre suas fraquezas. Não tenho palavras para agradecer por isso.",
            "Muito bem! O livro que você leu sobre a cidade perdida estava cheio de informações úteis. Agora sabemos exatamente onde ficam as ruínas do templo. Você nos economizou muito tempo—muito obrigado!",
            "Eu não posso acreditar que você leu todo aquele pergaminho! A história dos artefatos encantados era exatamente o que precisávamos. Vamos conseguir encontrá-los muito mais rápido agora. Ótimo trabalho!",
            "Você realmente encarou aquele livro, né? O capítulo sobre o tesouro do velho rei—justo o que precisávamos. Você tornou essa busca muito mais fácil, e eu sou grato por isso!",
            "Uau, você realmente leu aquele pergaminho inteiro! As informações sobre a magia negra são incrivelmente detalhadas. Não consigo dizer o quanto isso é valioso. Você nos fez um grande favor!",
            "Você realmente leu isso? Achei que fosse demais para você! Mas os detalhes sobre os rituais antigos eram exatamente o que precisávamos. Agora teremos um plano muito mais claro daqui pra frente, tudo graças a você.",
            "Foi uma leitura e tanto, mas você conseguiu! As informações sobre as ruínas amaldiçoadas são um divisor de águas. Você nos deu a vantagem que precisávamos para entender o que está por aí. Muito obrigado!",
            "Você não só deu uma olhada, leu tudo? Impressionante! Os insights sobre as antigas batalhas vão nos ajudar muito. Você realmente nos fez avançar nessa missão.",
            "Eu não esperava que você fosse passar por tudo isso, mas passou! O livro sobre as barreiras mágicas está cheio de detalhes úteis. Estamos um passo mais perto de entender o mistério. Obrigado por ter dedicado seu tempo para isso!"
            };
        }

        protected override string [] highSocialClosers {
            get => new string [] {
            "Ah, então você realmente leu isso! Eu estava me perguntando o que você ia encontrar aí. As informações sobre artefatos antigos são exatamente o que eu precisava—obrigado por passar por todo esse trabalho!",
            "Uau, você realmente foi lá e leu isso! A história do antigo reino? Coisa fascinante, né? Eu tinha a sensação de que isso ia nos dar uma luz sobre nosso próximo passo. Eu realmente agradeço!",
            "Você leu tudo, né? Bem, fico feliz que tenha feito isso, porque aquele livro sobre a cidade perdida tem algumas informações valiosas. Está tudo lá—mapas, detalhes, tudo o que precisamos. Bom trabalho!",
            "Estou impressionado! Você leu o pergaminho e conseguiu passar pelos detalhes. As informações sobre os rituais antigos? Exatamente o que eu estava esperando encontrar. Obrigado por dedicar seu tempo!",
            "Você realmente passou por aquele livro? Impressionante! As seções sobre a magia proibida eram exatamente o que eu esperava—úteis, mas arriscadas. Eu agradeço você ter se aprofundado nisso por nós.",
            "Você tem a paciência de um santo, vou te dar isso. Você leu aquele pergaminho e voltou com informações sobre as bestas encantadas? Justo o que precisávamos, e você me poupou horas de pesquisa. Obrigado!",
            "Bom, olha só você! Não só deu uma olhada, leu tudo! Aquele texto sobre os artefatos perdidos? Agora sabemos por onde começar nossa busca. Você realmente nos ajudou, obrigado!",
            "Uau, você realmente encarou aquele livro sobre as ruínas antigas. Eu achava que seria difícil de digerir, mas você tirou os detalhes importantes. As informações sobre as barreiras mágicas são exatamente o que precisávamos. Obrigado!",
            "Você passou pelo pergaminho sobre magia negra, não foi? E voltou com todos os pontos-chave. Eu tenho que admitir, isso não foi tarefa fácil. Mas agora sabemos o que estamos enfrentando. Muito obrigado por isso!",
            "Você leu o livro inteiro? Uau, eu não esperava que você fosse conseguir tão rápido! A parte sobre os artefatos sagrados—uma descoberta fantástica. Agora temos uma boa pista, e devo isso a você. Obrigado!"
            };
        }
    }
}