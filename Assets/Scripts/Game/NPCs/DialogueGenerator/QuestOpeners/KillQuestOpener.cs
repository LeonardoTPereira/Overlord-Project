using Game.GameManager;
namespace Game.NPCs
{
    public class KillQuestOpener : QuestDialogue
    {
        protected override string [] lowSocialDialogues{
            get {
                if (_language == Util.Enums.Language.Portuguese)
                    return new string [] {
                    "Não há nada que eu desgoste mais do que {questSo.GetEnemyString()}. Vou te dizer o que, se você se livrar de {questSo.GetEnemyAmountString()}, talvez eu te dê uma recompensa.",
                    "Esses {questSo.GetEnemyString()} estão atrapalhando meu caminho. Seja útil e mate {questSo.GetEnemyAmountString()}.",
                    "Não acredito que deixei essas criaturas imundas me roubarem! EU VOU me vingar. Mate {questSo.GetEnemyAmountString()} para mim.",
                    "Eu não consigo me concentrar no meu trabalho com essas malditas criaturas por aqui. Seja útil e se livre de {questSo.GetEnemyAmountString()}.",
                    "Esses pestes estão me irritando. Mate {questSo.GetEnemyAmountString()} deles, por favor? Eu tenho coisas melhores para fazer.",
                    "Ugh, esses {questSo.GetEnemyString()} estão causando caos novamente. Vai lá e lide com {questSo.GetEnemyAmountString()} deles. Não me faça repetir.",
                    "Se eu tiver que ouvir mais alguma coisa sobre essas criaturas, eu vou pirar. Mata {questSo.GetEnemyAmountString()} delas e seja rápido.",
                    "Quer ajudar? Beleza. Se livra de {questSo.GetEnemyAmountString()} dessas pragas para mim. Quem sabe assim eu consiga um pouco de paz e tranquilidade.",
                    "Olha, não estou no clima para desculpas. Vai lá e elimina {questSo.GetEnemyAmountString()} desses {questSo.GetEnemyString()} antes que eles piorem as coisas.",
                    "Essas coisas estão por toda parte e está me deixando maluco. Você é capaz, certo? Vai lá e elimina {questSo.GetEnemyAmountString()} delas."
                    };
                return new string[] {
                "There is nothing I dislike more than {questSo.GetEnemyString()}. I'll tell you what, if you get rid of about {questSo.GetEnemyAmountString()}, I might give you a reward.",
                "These {questSo.GetEnemyString()} have been getting in my way. Be of some use and kill about {questSo.GetEnemyAmountString()}.",
                "I can't believe I allowed these filthy monsters to steal from me! I WILL get my revenge. Kill {questSo.GetEnemyAmountString()} for me.",
                "I can't concentrate on my work with these stupid monsters around. Make yourself usefull and get rid of {questSo.GetEnemyAmountString()}.",
                "Those pests are getting on my nerves. Take out {questSo.GetEnemyAmountString()} of them, will you? I’ve got better things to do.",
                "Ugh, those {questSo.GetEnemyString()} are causing havoc again. Go deal with {questSo.GetEnemyAmountString()} of them. Don’t make me repeat myself.",
                "If I have to hear one more thing about those creatures, I’m going to lose it. Go kill {questSo.GetEnemyAmountString()} of them and be quick about it.",
                "You want to help? Fine. Get rid of {questSo.GetEnemyAmountString()} of those nuisances for me. Maybe then I’ll get some peace and quiet.",
                "Look, I’m not in the mood for excuses. Go out there and take down {questSo.GetEnemyAmountString()} of those {questSo.GetEnemyString()} before they make things worse.",
                "Those things are everywhere, and it’s driving me crazy. You’re capable, right? Go take out {questSo.GetEnemyAmountString()} of them."
                };
            }
        }

        protected override string [] averageSocialDialogues{
            get {
                if (_language == Util.Enums.Language.Portuguese)
                    return new string [] {
                    "ATCHOOO, a-- ATCHOOO .... Oh, desculpe por isso. Na verdade, sou alérgico a {questSo.GetEnemyString()}. Como funciona isso, você pergunta? Bem, melhor perguntar para o meu médico do que para mim... Você poderia se livrar de {questSo.GetEnemyAmountString()}? Isso me ajudaria muito...",
                    "Isso é um pouco embaraçoso, mas na verdade eu sou assustado com {questSo.GetEnemyString()}. Você poderia se livrar de alguns deles para mim? Acho que se você matar {questSo.GetEnemyAmountString()} eu me sentiria bem mais seguro!",
                    "Sabia que um estudo recente mostrou que a maioria das nossas mortes relacionadas a monstros são causadas por {questSo.GetEnemyString()}. Isso não é assustador? Você poderia se livrar de {questSo.GetEnemyAmountString()}?",
                    "Estamos em apuros com esses inimigos por aí. Você pode eliminar {questSo.GetEnemyAmountString()} deles para nos ajudar?",
                    "Essas criaturas estão se tornando uma verdadeira ameaça. Você acha que poderia diminuir o número delas? {questSo.GetEnemyAmountString()} deve ser o suficiente.",
                    "Precisamos colocar essa área sob controle. Posso contar com você para eliminar {questSo.GetEnemyAmountString()} desses inimigos?",
                    "Ei, pode dar uma força? Se você eliminar {questSo.GetEnemyAmountString()} desses inimigos, vai deixar as coisas muito mais seguras para todos.",
                    "A presença do inimigo está se tornando avassaladora. Cuide de {questSo.GetEnemyAmountString()} deles, e podemos ter uma chance.",
                    "Este lugar não vai ser seguro até lidarmos com essas criaturas. Elimine {questSo.GetEnemyAmountString()} delas e volte para me contar."
                    };
                return new string[] {
                "ATCHOOO, a-- ATCHOOO .... Oh, sorry about that. I'm actually allergic to {questSo.GetEnemyString()}. How does that work you ask? Well, you better ask my doctor instead of me... Could you actually get rid of {questSo.GetEnemyAmountString()}? It would really help me...",
                "This is a bit embarassing but I actually am scared of {questSo.GetEnemyString()}. Could you get rid of some of them for me? I guess if you killed {questSo.GetEnemyAmountString()} I would feel way safer!",
                "Did you know that a recent study showed that most of our monster related deaths are caused by {questSo.GetEnemyString()}. That sure is scary, isn't it? Could you get rid of {questSo.GetEnemyAmountString}?",
                "We’re in trouble with all these enemies around. Can you take out {questSo.GetEnemyAmountString()} of them to help us out?",
                "Those creatures are becoming a real threat. Think you could thin their numbers? {questSo.GetEnemyAmountString()} should do the trick.",
                "We need to get this area under control. Can I count on you to eliminate {questSo.GetEnemyAmountString()} of those enemies?",
                "Hey, can you lend a hand? If you take out {questSo.GetEnemyAmountString()} of those enemies, it’ll make things a lot safer for everyone.",
                "The enemy presence is getting overwhelming. Take care of {questSo.GetEnemyAmountString()} of them, and we might just stand a chance.",
                "This place won’t be safe until we deal with those creatures. Take out {questSo.GetEnemyAmountString()} of them and report back."
                };
            }
        }

        protected override string [] highSocialDialogues{
            get {
                if (_language == Util.Enums.Language.Portuguese)
                    return new string [] {
                    "Há muito tempo, um bando de {questSo.GetEnemyString()} matou minha mãe. Não sou muito fã de caçar monstros, mas acredito que devemos fazer algo para que as pessoas não se machuquem. Eu não tenho forças para isso, mas ficaria muito grato se você conseguisse se livrar de {questSo.GetEnemyAmountString()}.",
                    "Estamos desesperados por ajuda. Os {questSo.GetEnemyString()} devem ser parados! Você pode lidar com {questSo.GetEnemyAmountString()} para nós?",
                    "Oh, graças a Deus você chegou! Essas criaturas chatas estão por toda parte, e eu não aguento mais. Você poderia, talvez, sei lá, se livrar de {questSo.GetEnemyAmountString()} delas? Isso seria incrível!",
                    "Ok, aqui está a situação. Esses inimigos estão por toda parte, causando caos, e eu só estou aqui, impotente. Você poderia lidar com, digamos, {questSo.GetEnemyAmountString()} deles? Eu me sentiria muito melhor!",
                    "Eu tenho visto esses inimigos causando estragos, e, honestamente, é exaustivo só de pensar nisso. Você poderia se livrar de {questSo.GetEnemyAmountString()} deles? Por favorzinho? Você é o melhor!",
                    "Você conhece esses inimigos que estão causando todos os problemas? Então, eu preciso que você se livre de {questSo.GetEnemyAmountString()} deles. Você é bom nisso, né? Claro que é!",
                    "Então, lá estava eu, pensando em como poderíamos resolver esse problema com os inimigos, e aí você apareceu! Perfeito timing. Você poderia se livrar de {questSo.GetEnemyAmountString()} deles? Isso significaria o mundo para mim.",
                    "Oh meu Deus, você não vai acreditar como essas criaturas são irritantes! Se você pudesse apenas lidar com, ah, sei lá, {questSo.GetEnemyAmountString()} delas, eu ficaria eternamente grato!"
                    };
                return new string[] {
                "Long ago, a bunch of {questSo.GetEnemyString()} killed my mother. I'm not too found of monster hunting, but I do believe we should do something so people don't get hurt. I don't have it in me, but I'd be gratefull if you were able to take care of {questSo.GetEnemyAmountString()}.",
                "We’re in desperate need of help. The  {questSo.GetEnemyString()} must be stopped! Could you deal with {questSo.GetEnemyAmountString()} for us?",
                "Oh, thank goodness you’re here! Those pesky creatures are everywhere, and I can’t take it anymore. Could you, maybe, I don’t know, take out {questSo.GetEnemyAmountString()} of them? That would be amazing!",
                "Okay, so here’s the deal. These enemies are all over the place, causing chaos, and I’m just sitting here, helpless. Could you handle, say, {questSo.GetEnemyAmountString()} of them? I’d feel so much better!",
                "I’ve been watching these enemies wreak havoc, and, honestly, it’s exhausting just thinking about it. Could you take care of {questSo.GetEnemyAmountString()} of them? Pretty please? You’re the best!",
                "You know those enemies causing all the problems? Yeah, I need you to get rid of {questSo.GetEnemyAmountString()} of them. You’re good at that, right? Of course you are!",
                "So, there I was, thinking about how we could solve this enemy problem, and then you showed up! Perfect timing. Could you take out {questSo.GetEnemyAmountString()} of them? It’d mean the world to me.",
                "Oh my gosh, you wouldn’t believe how annoying those creatures are! If you could just deal with, oh, I don’t know, {questSo.GetEnemyAmountString()} of them, I’d be forever grateful!"
                };
            }
        }
    }
}