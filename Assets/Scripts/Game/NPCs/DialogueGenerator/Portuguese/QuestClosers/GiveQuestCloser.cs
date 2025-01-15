namespace Game.NPCs.PTBR
{
    public class GiveQuestCloser : QuestCloser
    {
        protected override string [] lowSocialClosers {
            get => new string [] {
            "Bem, você realmente conseguiu. Encontrou o item e entregou para eles. Acho que isso vale um 'obrigado.'",
            "Huh. Você passou por todo esse trabalho e realmente entregou o item. Tudo bem, suponho que devo agradecer.",
            "Não acredito que você realmente conseguiu. Obrigado, acho, por ter entregado isso para eles.",
            "Você conseguiu fazer o que eu pedi. Aqui está o seu agradecimento—não espere muito mais de mim.",
            "Nada mal. Você encontrou o item e entregou. Tudo bem, meus agradecimentos, mas é só isso.",
            "Você realmente fez isso, hein? Bem, obrigado pela entrega. Não espere que eu fique todo animado com isso.",
            "Você pegou o item e entregou. Tudo bem, admito, estou grato... só não faça disso um hábito.",
            "Não achei que você conseguiria, mas conseguiu. Obrigado por levar o item até eles, acho.",
            "Você realmente fez isso. Suponho que deveria agradecer por passar isso adiante, mas não espere mais elogios de mim.",
            "Bem, você conseguiu. Obrigado por entregar o item a eles—só não me faça pedir de novo."
            };
        }

        protected override string [] averageSocialClosers {
            get => new string [] {
            "Obrigado por pegar aquele item e entregá-lo a eles. Não posso dizer o quanto isso vai ajudar.",
            "Eu realmente aprecio você ter cuidado disso por mim. Entregar esse item para eles não foi uma tarefa pequena. Você fez um ótimo trabalho!",
            "Excelente trabalho! Fico feliz que você conseguiu encontrar o item e entregá-lo. Era exatamente o que precisávamos.",
            "Você realmente nos ajudou! Obrigado por recuperar o item e garantir que ele chegasse à pessoa certa.",
            "Muito obrigado pelos seus esforços! Colocar esse item nas mãos deles significa muito para nós. Você foi de grande ajuda.",
            "Eu sabia que podia contar com você! Obrigado por não apenas reunir o item, mas também entregá-lo a eles tão rapidamente.",
            "Muito bem! Você foi além para garantir que o item chegasse à pessoa certa. Seus esforços são realmente apreciados.",
            "Obrigado pela dedicação em reunir aquele item e passá-lo adiante. Você realmente fez a diferença com essa tarefa.",
            "Você executou essa tarefa perfeitamente! Obrigado por não apenas coletar o item, mas também garantir que ele chegasse à pessoa certa.",
            "Não consigo te agradecer o suficiente por pegar aquele item e entregá-lo a eles. Era exatamente o que eles precisavam, e você fez isso acontecer!"
            };
        }

        protected override string [] highSocialClosers {
            get => new string [] {
            "Uau, você realmente fez isso! Você reuniu o item e o entregou, nada menos! Quero dizer, eu sabia que você tinha isso em você, mas ainda assim! Foi uma tarefa e tanto, e estou tão grato que você fez acontecer!",
            "Oh meu Deus, você foi além, não foi? Não só reuniu o item, mas realmente o entregou! Você está facilitando a vida de todos por aqui. Muito obrigado!",
            "Você conseguiu! Você realmente conseguiu! Reunir aquele item não foi fácil, mas depois garantir que ele chegasse à pessoa certa? Estou impressionado. Sério, você ganhou minha gratidão dez vezes mais por essa!",
            "Ah, que alívio! Você conseguiu o item e o entregou para eles—finalmente! Estou tão feliz que você conseguiu cuidar disso. Aquela tarefa era demais para mim, mas você cuidou dela sem problemas!",
            "Você acredita nisso? Você reuniu o item e realmente o entregou também! Eu ainda estaria aqui tentando descobrir como fazer isso chegar até eles se não fosse por você. Obrigado, obrigado, obrigado!",
            "Bem, olha só você! Eu sabia que você poderia lidar com isso, mas você realmente arrasou. Não só encontrou o item, mas garantiu que ele chegasse à pessoa certa—isso é dedicação! Agradeço mais do que você imagina.",
            "Tenho que dizer, estou impressionado! Você encontrou o item e o entregou a eles sem perder tempo. Agora, não sei o que eu teria feito sem você. Você tornou todo esse processo muito mais fácil, e por isso, sou realmente grato!",
            "Oh, uau, você conseguiu! Estou honestamente impressionado! Você não só reuniu o item, mas garantiu que ele chegasse às mãos certas. Você fez um trabalho fantástico, e não posso te agradecer o suficiente pela ajuda.",
            "Olha você, sempre cumprindo a tarefa! Você reuniu o item e ainda garantiu que chegasse à pessoa certa sem problemas. Honestamente, estou muito grato por todo o trabalho duro que você colocou nisso. Você tornou tudo muito mais fácil!",
            "Você realmente arrasou nessa! Reunir o item e entregá-lo sem problemas? Isso é impressionante! Não consigo te agradecer o suficiente por cuidar disso. As coisas vão ser muito mais tranquilas graças a você."
            };
        }
    }
}
