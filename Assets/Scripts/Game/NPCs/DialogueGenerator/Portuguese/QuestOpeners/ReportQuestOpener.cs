using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs.PTBR
{
    public class ReportQuestOpener : QuestOpener
    {
        protected override string [] lowSocialOpeners {
            get => new string [] {
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
        }

        protected override string [] averageSocialOpeners {
            get => new string [] {
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
        }

        protected override string [] highSocialOpeners {
            get => new string [] {
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
        }
    }
}