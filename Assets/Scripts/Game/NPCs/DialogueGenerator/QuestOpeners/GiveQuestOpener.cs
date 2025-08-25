using Game.GameManager;

namespace Game.NPCs
{
    public class GiveQuestOpener : QuestDialogue
    {
        protected override string [] lowSocialDialogues{
            get {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string [] {
                    "Leve {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()}. Eu faria isso, mas tenho coisas mais importantes para fazer.",
                    "Ugh, tá bom. Você está aqui, então seja útil e entregue {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()}. Não perca.",
                    "Eu não tenho tempo para essa bobagem. Leve {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()} e seja rápido.",
                    "{questSo.GetTargetNpc()} precisa de {questSo.GetItemAmountString()}, e eu não estou com paciência para lidar com isso. Você cuida disso.",
                    "Entregue {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()} para mim, pode ser? Eu já tenho o suficiente para me preocupar.",
                    "Por que sempre sou eu que fico fazendo isso? Seja lá, agora é sua vez. Dê {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()}.",
                    "Leve um {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()}. E não me pergunte por quê—não é da sua conta.",
                    "Olha, eu não confio em mais ninguém para isso, então você vai fazer. Dê {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()}. Tente não estragar tudo.",
                    "Você vai para lá de qualquer forma, certo? Ótimo. Entregue {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()} enquanto estiver indo.",
                    "Eu não aguento mais lidar com {questSo.GetTargetNpc()} hoje. Leve {questSo.GetItemAmountString()} para ele por mim, e não me faça me arrepender de ter pedido."
                    };
                return new string [] {
                "Take {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()}. I’d do it myself, but I’ve got better things to do.",
                "Ugh, fine. You’re here, so make yourself useful and give {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()}. Don’t lose it.",
                "I don’t have time for this nonsense. Take {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()} and be quick about it.",
                "{questSo.GetTargetNpc()} needs {questSo.GetItemAmountString()}, and I’m not in the mood to deal with them. You handle it.",
                "Deliver {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()} for me, will you? I’ve got enough on my plate as it is.",
                "Why am I always stuck doing this? Whatever, you’re doing it now. Give {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()}.",
                "Take a {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()}. And don’t ask me why—it’s none of your business.",
                "Look, I don’t trust anyone else with this, so you’re up. Give {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()}. Try not to mess it up.",
                "You’re heading that way anyway, right? Good. Hand {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()} for me while you’re at it.",
                "I can’t stand dealing with {questSo.GetTargetNpc()} today. Take {questSo.GetItemAmountString()} to them for me, and don’t make me regret asking you."
                };
            }
        }

        protected override string [] averageSocialDialogues{
            get {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string [] {
                    "Você poderia levar {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()} para mim? Ele está esperando isso, e eu ficaria grato pela ajuda!",
                    "Aqui está um item que {questSo.GetTargetNpc()} precisa. Você pode entregá-lo para ele? Eu confio que você vai fazer isso com segurança.",
                    "Eu estava querendo entregar {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()}, mas estou atolado. Você se importaria de levar para ele?",
                    "Oh, que coincidência! Eu preciso de alguém para entregar {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()}. Você acha que consegue fazer isso?",
                    "Eu realmente ficaria grato se você pudesse levar {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()}. É algo que ele está pedindo.",
                    "Você poderia correr até {questSo.GetTargetNpc()} e entregar {questSo.GetItemAmountString()}? É importante que ele recebam isso logo, e eu sei que posso contar com você."
                    };
                return new string [] {
                "Could you take {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()} for me? They’ve been waiting for it, and I’d appreciate the help!",
                "Here’s an item that {questSo.GetTargetNpc()} needs. Can you deliver it to them? I trust you’ll get it there safely.",
                "I’ve been meaning to give {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()}, but I’m swamped. Would you mind taking it to them for me?",
                "Oh, perfect timing! I need someone to deliver {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()}. Think you can handle that?",
                "I’d really appreciate it if you could take {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()}. It’s something they’ve been asking for.",
                "Could you run {questSo.GetItemAmountString()} over to {questSo.GetTargetNpc()}? It’s important they get it soon, and I know I can count on you."
                };
            }
        }

        protected override string [] highSocialDialogues{
            get {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string [] {
                    "Oh, oi! Justo a pessoa que eu esperava ver! Então, {questSo.GetTargetNpc()} me disse um tempo atrás que ele precisa de {questSo.GetItemAmountString()}. Mas, você sabe como é, eu sempre me distraio e esqueço no meio do caminho. Você pode entregar para ele por mim? Por favor?",
                    "Ai meu Deus, eu estava querendo levar {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()} há séculos! Bem, ok, talvez não séculos, mas parece que foi. Enfim, você pode entregar? Você é muito melhor nisso!",
                    "Então, engraçado, eu prometi a {questSo.GetTargetNpc()} que eu entregaria {questSo.GetItemAmountString()}, mas, bem, sempre acontece alguma coisa! Você pode me ajudar e levar para ele? Você é um salva-vidas!",
                    "Oh, isso é perfeito! {questSo.GetTargetNpc()} precisa de {questSo.GetItemAmountString()}, e você é a pessoa perfeita para entregar! Não se importa, né? Quero dizer, você já está indo para lá, certo?",
                    "Ok, então aqui vai o trato—eu preciso levar {questSo.GetItemAmountString()} para {questSo.GetTargetNpc()}, mas sejamos sinceros, eu provavelmente ia deixar cair ou perder ou fazer algo bobo assim. Mas você? Você vai dar conta de tudo!",
                    "Então, eu estava querendo deixar {questSo.GetItemAmountString()} com {questSo.GetTargetNpc()}, mas, você sabe como é, a vida acaba atrapalhando! Você se importaria de levar para ele? Eu vou te dever um favor. Ou dois!"
                    };
                return new string [] {
                "Oh, hey! Just the person I was hoping to see! So, {questSo.GetTargetNpc()} told me a while back that they need {questSo.GetItemAmountString()}. But, you know me—I’d get distracted and forget halfway there. Can you give them it for me? Pretty please?",
                "Oh my gosh, I’ve been meaning to get {questSo.GetItemAmountString()}  to {questSo.GetTargetNpc()} for ages! Well, okay, maybe not ages, but it feels like it. Anyway, could you deliver it? You’re so much better at this kind of thing!",
                "So, funny thing—I promised {questSo.GetTargetNpc()} I’d give them {questSo.GetItemAmountString()} , but, well, something always comes up! Could you help me out and take it to them? You’re a lifesaver!",
                "Oh, this is perfect! {questSo.GetTargetNpc()} needs {questSo.GetItemAmountString()}, and you’re just the person to deliver it! You don’t mind, right? I mean, you’re heading that way anyway... right?",
                "Okay, so here’s the deal—I need to get {questSo.GetItemAmountString()} to {questSo.GetTargetNpc()}, but let’s be real, I’d probably drop it or lose it or something silly like that. But you? You’ve got this in the bag!",
                "So, I’ve been meaning to drop {questSo.GetItemAmountString()}  off with {questSo.GetTargetNpc()}, but, you know, life gets in the way! Would you mind taking it to them? I’ll owe you one. Or two!"
                };
            }
        }
    }
}