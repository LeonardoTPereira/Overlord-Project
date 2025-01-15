namespace Game.NPCs.PTBR
{
    public class ListenQuestCloser : QuestCloser
    {
        protected override string [] lowSocialClosers {
            get => new string [] {
            "Certo, tudo bem... obrigado por ouvir {questSo.GetTargetNpc()}. Isso me economiza o trabalho, eu acho.",
            "Hmph. Acho que devo te agradecer por lidar com {questSo.GetTargetNpc()}. Eles realmente falam demais, não?",
            "Bom, você realmente ouviu [Nome do NPC]? Acho que devo te agradecer por isso.",
            "Você realmente ficou e ouviu {questSo.GetTargetNpc()}? Você tem mais paciência do que eu. Obrigado, eu acho.",
            "Certo, obrigado por dedicar seu tempo a {questSo.GetTargetNpc()}. Você me fez um favor, quer saiba disso ou não.",
            "Acho que devo te agradecer por ouvir {questSo.GetTargetNpc()}. Eles teriam continuado me incomodando até alguém fazer isso.",
            "Não costumo dizer isso, mas... obrigado. Ouvir {questSo.GetTargetNpc()} deve ter exigido paciência.",
            "Estou feliz que alguém finalmente tenha dado atenção a {questSo.GetTargetNpc()}. Agora talvez eles parem de me incomodar..."
            };
        }

        protected override string [] averageSocialClosers {
            get => new string [] {
            "Obrigado por ouvir {questSo.GetTargetNpc()}. Eles precisavam disso, e eu sei que fez a diferença.",
            "Significa muito que você tenha ouvido {questSo.GetTargetNpc()}. Não muitos teriam dedicado seu tempo.",
            "Obrigado por mostrar tanta paciência a {questSo.GetTargetNpc()}. Eles precisavam de alguém como você para ouvi-los.",
            "Estou realmente grato por você ter conversado com {questSo.GetTargetNpc()}. Eles tinham tanto a compartilhar, e você ouviu.",
            "Obrigado por prestar atenção em {questSo.GetTargetNpc()}. Eu sei que eles se sentiram ouvidos por sua causa.",
            "Eu realmente aprecio você ter dedicado seu tempo a {questSo.GetTargetNpc()}. Você elevou o espírito deles.",
            "Obrigado. Ouvir {questSo.GetTargetNpc()} não foi apenas gentil—foi exatamente o que eles precisavam."
            };
        }

        protected override string [] highSocialClosers {
            get => new string [] {
            "Oh, muito obrigado por ouvir {questSo.GetTargetNpc()}! Eles têm as histórias mais fascinantes, não têm?",
            "Você ouviu {questSo.GetTargetNpc()}? Maravilhoso! Não são uma verdadeira fonte de informações?",
            "Obrigado, obrigado! {questSo.GetTargetNpc()} tem coisas tão interessantes para dizer, e eu sabia que você ia apreciar!",
            "Ah, eu sabia que você ia ouvir {questSo.GetTargetNpc()}! Não é ótimo ouvir o lado deles? Obrigado por dar ouvidos a eles!",
            "Muito obrigado por ouvir {questSo.GetTargetNpc()}! Eles sempre têm as melhores percepções. Eles te contaram sobre a vez que...?",
            "Obrigado! {questSo.GetTargetNpc()} tem tanto a dizer, e eu sabia que você seria a pessoa certa para ouvir!",
            "Oh, obrigado por ouvir {questSo.GetTargetNpc()}! Eu poderia conversar com eles o dia todo, e agora sei que você também poderia!",
            "Muito obrigado! {questSo.GetTargetNpc()} sempre tem coisas maravilhosas para compartilhar. Eles te contaram as últimas novidades?"
            };
        }
    }
}
