using Game.GameManager;

namespace Game.NPCs
{
    public class ExchangeQuestOpener : QuestDialogue
    {
        protected override string[] lowSocialDialogues
        {
            get {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string [] {
                    "Ugh, eu não tenho tempo para essa missão. Se faça útil e faça isso por mim. Vá trocar um {questSo.GetItemString()} com {questSo.GetTargetNpc()}. Talvez você ganhe algo em troca.",
                    "Olha, eu não estou afim de lidar com {questSo.GetTargetNpc()} hoje. Você cuida da troca e eles vão te recompensar. Tudo o que você precisa fazer é levar um {questSo.GetItemString()} até eles.",
                    "Por que você não vai incomodar outra pessoa, hein? Leve um {questSo.GetItemString()} até [Nome do NPC]. Eles têm algo esperando para quem entregar—não me pergunte o quê.",
                    "Vá trocar um {questSo.GetItemString()} com {questSo.GetTargetNpc()}. Você vai ganhar algum tipo de recompensa e eu vou ganhar um pouco de paz e silêncio.",
                    "Se você está tão ansioso por recompensas, leve um {questSo.GetItemString()} até {questSo.GetTargetNpc()}. Eles vão te dar algo, tenho certeza.",
                    "Eu estou muito ocupado para pedidos de troca. Leve um {questSo.GetItemString()} até {questSo.GetTargetNpc()}, eles vão te recompensar, e eu não terei que levantar um dedo."
                    };
                return new string[] {
                "Ugh, I don’t have time for this request. Make yourself usefull and do it for me. Go trade a {questSo.GetItemString()} with {questSo.GetTargetNpc()}. You might get something out of it.",
                "Look, I don’t feel like dealing with {questSo.GetTargetNpc()} today. You handle the trade, and they’ll reward you. All you need to do is get them an {questSo.GetItemString()}",
                "Why don't you go bother someone else, will ya? Take a {questSo.GetItemString()} over to [NPC Name]. They’ve got something waiting for whoever delivers it—don’t ask me what.",
                "Go trade a {questSo.GetItemString()} with {questSo.GetTargetNpc()}. You’ll get some sort of reward, and I’ll get some peace and quiet.",
                "If you’re so eager for rewards, bring a {questSo.GetItemString()} to {questSo.GetTargetNpc()}. They’ll give you something, I’m sure.",
                "I’m too busy for trade request. Take a {questSo.GetItemString()} to {questSo.GetTargetNpc()}, they’ll reward you, and I won’t have to lift a finger."
                };
            }
        }

        protected override string[] averageSocialDialogues
        {
            get {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string [] {
                    "Se você topar, eu preciso que você troque um {questSo.GetItemString()} com {questSo.GetTargetNpc()}. Eles precisam disso para um novo feitiço que estão aprendendo e vão te recompensar generosamente por isso.",
                    "Você estaria disposto a fazer uma troca para mim? Dê um {questSo.GetItemString()} para {questSo.GetTargetNpc()}, e ouvi dizer que eles têm uma boa recompensa pronta.",
                    "Eu deveria ter dado {questSo.GetTargetNpc()} um {questSo.GetItemString()} antes do amanhecer para o novo feitiço deles, mas acho que não vou conseguir. Se você puder fazer isso por mim, pode pegar a recompensa deles.",
                    "Você poderia levar um {questSo.GetItemString()} até {questSo.GetTargetNpc()}? Eles me disseram que estão procurando isso e têm uma recompensa para quem entregar."
                    };
                return new string[] {
                "If you’re up for it, I need you to trade a {questSo.GetItemString()} with {questSo.GetTargetNpc()}. They need it for a new spell they are learning and will reward you handsomely for it.",
                "Would you be willing to make a trade for me? Give a {questSo.GetItemString()} to {questSo.GetTargetNpc()}, and I hear they’ve got a nice reward ready.",
                "I was supposed to get {questSo.GetTargetNpc()} a {questSo.GetItemString()} before dawn for their new spell, but I don't think I'll make it. If you can do it for me, you can get their reward.",
                "Could you take a {questSo.GetItemString()} to {questSo.GetTargetNpc()}? They told me they are looking for it, and they have a reward for anyone that brings it to them."
                };
            }
        }

        protected override string[] highSocialDialogues
        {
            get {
                if (GameManagerSingleton.Instance.IsInPortuguese)
                    return new string [] {
                    "Ah, você é exatamente a pessoa que eu preciso! Você poderia levar um {questSo.GetItemString()} até {questSo.GetTargetNpc()}? Tenho certeza de que eles vão te recompensar grandemente!",
                    "Eu ouvi dizer que {questSo.GetTargetNpc()} está morrendo de vontade de colocar as mãos em um {questSo.GetItemString()}. Leve até eles, e eles vão ficar tão felizes—eles sempre dão as melhores recompensas!",
                    "Eu estava querendo trocar um {questSo.GetItemString()} com {questSo.GetTargetNpc()}, mas estou tão ocupado! Se você fizer isso por mim, tenho certeza de que eles vão te dar algo incrível. Eles sempre têm os melhores tesouros!",
                    "Você é a pessoa certa para isso! Leve um {questSo.GetItemString()} até {questSo.GetTargetNpc()}, e provavelmente eles vão te dar algo melhor do que você imaginava. Eles sempre me surpreendem com o que têm!",
                    "Ah, eu sei que você está ocupado, mas {questSo.GetTargetNpc()} vai realmente apreciar uma troca! Eles sempre recompensam a gentileza—acredite, vai valer a pena o seu tempo! É só levar um {questSo.GetItemString()} até eles e você vai ver o que eu quero dizer!",
                    "Ei, você poderia trocar um {questSo.GetItemString()} com {questSo.GetTargetNpc()}? Não vejo a hora de ver a expressão no rosto deles quando você entregar isso! Eles provavelmente vão te recompensar com algo que vai te deixar de queixo caído. Eles sempre têm as melhores coisas!"
                    };
                return new string[] {
                "Oh, you’re just the person I need! Could you take a {questSo.GetItemString()} over to {questSo.GetTargetNpc()}? I’m sure they’ll reward you greatly!",
                "I’ve heard that {questSo.GetTargetNpc()} is dying to get their hands on a {questSo.GetItemString()}. Take it to them, and they’ll be so thrilled—they always give the best rewards!",
                "I’ve been meaning to trade a {questSo.GetItemString()} with {questSo.GetTargetNpc()}, but I’m just so busy! If you do it for me, I’m sure they’ll give you something incredible. They always have the best treasures!",
                "You’re just the right person for this! Take a {questSo.GetItemString()} to {questSo.GetTargetNpc()}, and they’ll probably give you something better than you imagined. They always surprise me with what they have!",
                "Oh, I know you’re busy, but {questSo.GetTargetNpc()} will really appreciate a trade! They always reward kindness—trust me, it’ll be worth your time! Just get them a {questSo.GetItemString()} and you'll see what I mean!",
                "Heyy, could you trade a {questSo.GetItemString()} with {questSo.GetTargetNpc()}? I can’t wait to see the look on their face when you give them it! They’ll probably reward you with something that will blow your mind. They always have the best stuff!"
                };
            }
        }
    }
}