using Game.GameManager;

namespace Game.NPCs
{
    public class ReportQuestOpener : QuestDialogue
    {
        protected override string [] lowSocialDialogues {
            get {
                if (GameManagerSingleton.Instance.IsInPortuguese) 
                    return new string [] {
                    "Eu odeio ser o portador de más notícias... Ou de qualquer notícia... Seja útil e entregue este envelope para {questSo.GetTargetNpc()}.",
                    "O que {questSo.GetTargetNpc()} me pediu para fazer vai se atrasar. Se os ver por aí, avise-os. Não vou sair do meu caminho para fazer isso.",
                    "Leve esta notícia até {questSo.GetTargetNpc()}, por favor? Eu não tenho tempo para lidar com isso.",
                    "Ugh, não estou fazendo recados hoje. Vá contar para {questSo.GetTargetNpc()} o que aconteceu e me deixe fora disso.",
                    "Tá bom. Se você está tão ansioso para ajudar, vá entregar esta notícia para {questSo.GetTargetNpc()}. E não estrague tudo.",
                    "Olha, estou ocupado demais para isso. Vai encontrar {questSo.GetTargetNpc()} e dar a atualização. Entendeu?",
                    "Por que eu tenho que fazer tudo por aqui? Vá contar para {questSo.GetTargetNpc()} o que eles precisam saber.",
                    "Esta notícia é importante, mas não vou perder meu tempo entregando ela. Você cuida disso—{questSo.GetTargetNpc()} está esperando.",
                    "Aqui está o acordo: você leva essa mensagem para {questSo.GetTargetNpc()}, e eu finalmente consigo um pouco de paz e sossego.",
                    "Quer ser útil? Vá contar para {questSo.GetTargetNpc()} o que está acontecendo. E não espere que eu te agradeça.",
                    "Não estou afim de ser o mensageiro. Vá você mesmo reportar a notícia para {questSo.GetTargetNpc()}.",
                    "Se você está parado aqui, não está ajudando. Vá entregar essa notícia para {questSo.GetTargetNpc()} antes que eu perca a paciência."
                    };
                return new string [] {
                "I hate being the bearer of bad news... Or even news at all... Be of some use and send this envelope to {questSo.GetTargetNpc()}.",
                "The thing {questSo.GetTargetNpc()} asked me to do is gonna be late. If you see them around, do tell. I'm not getting out of my way to do it.",
                "Take this news to {questSo.GetTargetNpc()}, would you? I don’t have time to deal with it myself.",
                "Ugh, I’m not running errands today. Go tell {questSo.GetTargetNpc()} what’s happened and leave me out of it.",
                "Fine. If you’re so eager to help, go deliver this news to {questSo.GetTargetNpc()}. And don’t mess it up.",
                "Look, I’m too busy for this. Go find {questSo.GetTargetNpc()} and give them the update. Got it?",
                "Why do I have to do everything around here? Go tell {questSo.GetTargetNpc()} what they need to know.",
                "This news is important, but I’m not wasting my time delivering it. You handle it—{questSo.GetTargetNpc()} is waiting.",
                "Here’s the deal: you take this message to {questSo.GetTargetNpc()}, and I can finally get some peace and quiet.",
                "You want to be useful? Go tell {questSo.GetTargetNpc()} what’s going on. And don’t expect me to thank you.",
                "I’m not in the mood to play messenger. You go report the news to {questSo.GetTargetNpc()} instead.",
                "If you’re standing here, you’re not helping. Go deliver this news to {questSo.GetTargetNpc()} before I lose my patience."
                };
            }
        }

        protected override string [] averageSocialDialogues {
            get {
                if (GameManagerSingleton.Instance.IsInPortuguese) 
                    return new string [] {
                    "Preciso que alguém avise {questSo.GetTargetNpc()} que não vou conseguir devolver o livro deles tão cedo. Você pode fazer esse favor e contar para eles?",
                    "{questSo.GetTargetNpc()} me emprestou um livro mágico, mas acho que vou precisar de mais algumas semanas para aprender os feitiços. Se os ver por aí, você pode perguntar se está tudo bem?",
                    "Preciso que você leve essa mensagem até {questSo.GetTargetNpc()}. Eles precisam saber disso o quanto antes.",
                    "Você pode entregar essa notícia para {questSo.GetTargetNpc()}? Eles são quem precisa saber agora.",
                    "Tenho uma informação crítica que precisa chegar até {questSo.GetTargetNpc()}. Você pode cuidar disso para mim?",
                    "Essa notícia é importante demais para esperar. Por favor, reporte para {questSo.GetTargetNpc()} imediatamente.",
                    "Você se importaria de encontrar {questSo.GetTargetNpc()} e passar essa informação? É urgente.",
                    "Alguém precisa informar {questSo.GetTargetNpc()} sobre isso. Posso confiar em você para fazer isso?",
                    "Isso é grande. Vá até {questSo.GetTargetNpc()} e tenha certeza de que estão atualizados.",
                    "Eu iria eu mesmo, mas estou ocupado aqui. Você pode entregar essa atualização para {questSo.GetTargetNpc()} por mim?",
                    "Você pode reportar isso para {questSo.GetTargetNpc()}? É crucial que eles saibam sobre isso.",
                    "Precisamos garantir que {questSo.GetTargetNpc()} esteja informado. Você vai levar a notícia até eles?"
                    };
                return new string [] {
                "I need someone to tell {questSo.GetTargetNpc()} that I won't be able to get their book back as soon as I thought. Could you do me a favor and tell them?",
                "{questSo.GetTargetNpc()} borrowed me a magical book but I think I'll need a couple more weeks to learn these spells. If you see them around, could you ask them if that's okay?",
                "I need you to take this message to {questSo.GetTargetNpc()}. They need to hear this as soon as possible.",
                "Could you deliver this news to {questSo.GetTargetNpc()}? They’re the one who needs to know right now.",
                "I’ve got some critical information that needs to reach {questSo.GetTargetNpc()}. Can you take care of that for me?",
                "This news is too important to wait. Please report it to {questSo.GetTargetNpc()} immediately.",
                "Would you mind finding {questSo.GetTargetNpc()} and passing this along? It’s urgent.",
                "Someone needs to inform {questSo.GetTargetNpc()} about this. Can I trust you to do that?",
                "This is big. Go to {questSo.GetTargetNpc()} and make sure they’re up to speed.",
                "I’d go myself, but I’m tied up here. Could you deliver this update to {questSo.GetTargetNpc()} for me?",
                "Can you report this to {questSo.GetTargetNpc()}? It’s crucial that they know about it.",
                "We need to make sure {questSo.GetTargetNpc()} is informed. Will you take the news to them?"
                };
            }
        }

        protected override string [] highSocialDialogues {
            get {
                if (GameManagerSingleton.Instance.IsInPortuguese) 
                    return new string [] {
                    "PARE O MUNDO, A MAIOR FOFOCA ACABOU DE CHEGAR! Eu imploro, você pode contar para {questSo.GetTargetNpc()} sobre isso??",
                    "Oh, você é exatamente a pessoa que eu preciso! Tenho essa informação vital, e realmente preciso que você a leve até {questSo.GetTargetNpc()}. Eles vão ficar tão gratos de ouvir isso de você!",
                    "Estava querendo contar essa notícia para {questSo.GetTargetNpc()}, mas você sabe como é, o tempo escapa! Você pode ir encontrá-los e contar o que aconteceu? É bem importante!",
                    "Ok, não quero te sobrecarregar, mas preciso que você leve essa notícia para {questSo.GetTargetNpc()}—e, confie em mim, eles vão ficar tão felizes que você fez isso! É uma grande coisa, sabe?",
                    "Certo, aqui vai. Tenho todas essas informações suculentas, e realmente preciso que você passe para {questSo.GetTargetNpc()}. Eles vão querer saber, você vai ver!",
                    "Escuta, eu até poderia ir eu mesmo, mas estou meio ocupado. Além disso, tenho certeza de que {questSo.GetTargetNpc()} preferiria ouvir isso de você, né? Está na hora de colocá-los por dentro!",
                    "Certo, eu sei que você tem muita coisa acontecendo, mas isso é realmente importante. Você pode levar isso para {questSo.GetTargetNpc()}? Eles estão esperando a atualização há séculos!",
                    "Então, aqui está a fofoca: {questSo.GetTargetNpc()} realmente precisa saber o que acabou de acontecer. Você acha que poderia entregar a mensagem para mim? Eu faria isso eu mesmo, mas... bem, você sabe.",
                    "Certo, eu não quero te sobrecarregar, mas você poderia me fazer um grande favor? Leve essas informações para {questSo.GetTargetNpc()}, vai? Eu faria isso, mas estou no meio de três coisas agora!",
                    "Eu estava tentando alcançar {questSo.GetTargetNpc()} o dia todo, mas você sabe como eles são—sempre fugindo! Você pode ir encontrá-los e contar o que está acontecendo?",
                    "Você é a pessoa perfeita para essa tarefa! Eu preciso que você leve essa notícia importante para {questSo.GetTargetNpc()}. Eles estavam esperando por essa informação, e tenho certeza de que vão apreciar muito você levar até eles!"
                    };
                return new string [] {
                "STOP THE WORLD, THE BIGGEST GOSSIP JUST DROPPED! I beg of you, could you please tell {questSo.GetTargetNpc()} about this??",
                "Oh, you’re just the person I need! I have this vital information, and I really need you to take it to {questSo.GetTargetNpc()}. They’ll be so grateful to hear it from you!",
                "I’ve been meaning to get this news to {questSo.GetTargetNpc()}, but you know, time just slips away! Could you go find them and tell them about what’s happened? It’s pretty important!",
                "Okay, so I don’t want to overwhelm you, but I need you to take this news to {questSo.GetTargetNpc()}—and, trust me, they’ll be so glad you did! It’s kind of a big deal, you know?",
                "Alright, here’s the thing. I’ve got all this juicy info, and I really need you to pass it along to {questSo.GetTargetNpc()}. They’re gonna want to know, you’ll see!",
                "Listen, I could go myself, but I’m kind of tied up. Plus, I’m sure {questSo.GetTargetNpc()} would rather hear it from you, right? It’s about time we get them caught up!",
                "Okay, I know you’ve got a lot going on, but this is really important. Can you take this to {questSo.GetTargetNpc()}? They’ve been waiting for the update for ages!",
                "So, here’s the scoop: {questSo.GetTargetNpc()} really needs to hear what just happened. Do you think you could deliver the message for me? I’d do it myself, but... well, you know.",
                "Alright, I don’t want to burden you, but could you do me a huge favor? Take this info to {questSo.GetTargetNpc()}, will you? I’d do it, but I’m in the middle of three things right now!",
                "I’ve been trying to catch up with {questSo.GetTargetNpc()} all day, but you know how they are—always running off! Could you go find them and tell them what’s going on?",
                "You’re the perfect person for this job! I need you to take this important news to {questSo.GetTargetNpc()}. They’ve been waiting for this info, and I’m sure they’ll appreciate you bringing it to them!"
                };
            }
        }
    }
}