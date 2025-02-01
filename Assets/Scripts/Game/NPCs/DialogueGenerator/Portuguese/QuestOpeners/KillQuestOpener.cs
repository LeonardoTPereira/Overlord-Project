using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs.PTBR
{
    public class KillQuestOpener : QuestOpener
    {
        protected override string [] lowSocialOpeners{
            get => new string [] {
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
        }

        protected override string [] averageSocialOpeners{
            get => new string [] {
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
        }

        protected override string [] highSocialOpeners{
            get => new string [] {
            "Há muito tempo, um bando de {questSo.GetEnemyString()} matou minha mãe. Não sou muito fã de caçar monstros, mas acredito que devemos fazer algo para que as pessoas não se machuquem. Eu não tenho forças para isso, mas ficaria muito grato se você conseguisse se livrar de {questSo.GetEnemyAmountString()}.",
            "Estamos desesperados por ajuda. Os {questSo.GetEnemyString()} devem ser parados! Você pode lidar com {questSo.GetEnemyAmountString()} para nós?",
            "Oh, graças a Deus você chegou! Essas criaturas chatas estão por toda parte, e eu não aguento mais. Você poderia, talvez, sei lá, se livrar de {questSo.GetEnemyAmountString()} delas? Isso seria incrível!",
            "Ok, aqui está a situação. Esses inimigos estão por toda parte, causando caos, e eu só estou aqui, impotente. Você poderia lidar com, digamos, {questSo.GetEnemyAmountString()} deles? Eu me sentiria muito melhor!",
            "Eu tenho visto esses inimigos causando estragos, e, honestamente, é exaustivo só de pensar nisso. Você poderia se livrar de {questSo.GetEnemyAmountString()} deles? Por favorzinho? Você é o melhor!",
            "Você conhece esses inimigos que estão causando todos os problemas? Então, eu preciso que você se livre de {questSo.GetEnemyAmountString()} deles. Você é bom nisso, né? Claro que é!",
            "Então, lá estava eu, pensando em como poderíamos resolver esse problema com os inimigos, e aí você apareceu! Perfeito timing. Você poderia se livrar de {questSo.GetEnemyAmountString()} deles? Isso significaria o mundo para mim.",
            "Oh meu Deus, você não vai acreditar como essas criaturas são irritantes! Se você pudesse apenas lidar com, ah, sei lá, {questSo.GetEnemyAmountString()} delas, eu ficaria eternamente grato!"
            };
        }
    }
}