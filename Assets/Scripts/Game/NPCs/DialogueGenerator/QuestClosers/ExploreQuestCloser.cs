using Game.GameManager;
namespace Game.NPCs
{
    public class ExploreQuestCloser : QuestDialogue
    {
        protected override string[] lowSocialDialogues
        {
            get
            {
                if (_language == Util.Enums.Language.Portuguese)
                    return new string[] {
                        "Bem, você realmente fez isso. Acho que você não é completamente inútil, afinal. Obrigado pelo relatório.",
                        "Huh, você explorou todas aquelas áreas. Certo, acho que você serve para alguma coisa. Obrigado, acho.",
                        "Eu não sei por que esperava que você falhasse, mas você não falhou. Obrigado por trazer as informações de volta.",
                        "Você conseguiu fazer isso. Nada mal. Obrigado por verificar esses lugares e me contar o que encontrou.",
                        "Eu não costumo dar elogios, mas você fez o que pedi. Obrigado pela atualização.",
                        "Não tenho reclamações... bem, pelo menos não sobre você. Obrigado por sair e trazer o relatório.",
                        "Eu não achei que você iria continuar com isso, mas você foi até o fim. Certo, eu admito—obrigado pelas informações.",
                        "Bem, parece que você é realmente capaz, afinal. Obrigado por explorar essas áreas e me deixar a par.",
                        "Eu tinha certeza de que você ia enrolar, mas você fez o trabalho. Certo, tem meus agradecimentos.",
                        "Nada mal. Você realmente explorou as áreas e trouxe o que eu precisava. Obrigado, acho."
                    };
                return new string[] {
                    "Well, you actually did it. I guess you’re not completely useless after all. Thanks for the report.",
                    "Huh, you explored all those areas. Fine, I guess you’re good for something. Thanks, I guess.",
                    "I don’t know why I expected you to fail, but you didn’t. Thanks for bringing the info back.",
                    "You managed to do it. Not bad. Thanks for checking out those places and letting me know what you found.",
                    "I don’t usually hand out praise, but you did what I asked. Thanks for the update.",
                    "I’ve got no complaints... well, none about you anyway. Thanks for going out there and reporting back.",
                    "I didn’t think you’d stick with it, but you did. Fine, I’ll admit it—thanks for the info.",
                    "Well, looks like you’re actually capable after all. Thanks for exploring those areas and filling me in.",
                    "I thought for sure you’d slack off, but you got the job done. Fine, you’ve got my thanks.",
                    "Not bad. You actually explored the areas and came back with what I needed. Thanks, I guess."
                };
            }
        }

        protected override string[] averageSocialDialogues
        {
            get
            {
                if (_language == Util.Enums.Language.Portuguese)
                    return new string[] {
                    "Obrigado por explorar essas áreas e trazer as informações de volta. Elas serão inestimáveis para nossos próximos passos.",
                    "Eu realmente aprecio você ter tirado o tempo para investigar essas áreas. Suas descobertas farão uma enorme diferença.",
                    "Você fez um ótimo trabalho lá fora. Obrigado por relatar os detalhes de todas essas áreas!",
                    "Obrigado pelo seu esforço! Sei que não foi fácil, mas sua exploração nos deu informações cruciais.",
                    "Não consigo agradecer o suficiente por enfrentar essas áreas e trazer relatórios tão úteis. Você foi de grande ajuda.",
                    "Muito bem! Obrigado por explorar esses lugares e me contar o que encontrou. Isso ajudará muito.",
                    "Seus esforços são muito apreciados. Obrigado por explorar todas essas áreas e compartilhar suas descobertas comigo.",
                    "Você fez mais do que eu poderia esperar. Obrigado por sua exploração minuciosa e pelas informações valiosas que reuniu!",
                    "Obrigado pela sua dedicação em explorar todas essas áreas. Seu relatório será fundamental para avançarmos com nossos planos.",
                    "Eu realmente aprecio o esforço que você fez para descobrir todas essas informações. Obrigado por explorar essas áreas e me manter atualizado."
                    };
                return new string[] {
                "Thank you for exploring those areas and bringing back the information. It’ll be invaluable to our next steps.",
                "I really appreciate you taking the time to investigate those areas. Your findings will make a huge difference.",
                "You did a great job out there. Thanks for reporting back with the details from all those areas!",
                "Thank you for your hard work! I know it wasn’t easy, but your exploration has given us some crucial information.",
                "I can’t thank you enough for braving those areas and bringing back such useful reports. You’ve been a real help.",
                "Well done! Thanks for exploring those places and letting me know what you found. This will help a lot.",
                "Your efforts are greatly appreciated. Thanks for exploring all those areas and sharing your findings with me.",
                "You’ve done more than I could’ve hoped for. Thank you for your thorough exploration and the valuable information you’ve gathered!",
                "Thanks for your dedication in exploring all those areas. Your report will be key to moving forward with our plans.",
                "I truly appreciate your effort in uncovering all that information. Thanks for exploring those areas and keeping me updated."
                };
            }
        }

        protected override string[] highSocialDialogues
        {
            get
            {
                if (_language == Util.Enums.Language.Portuguese)
                    return new string[] {
                    "Oh, uau! Você realmente se superou! Explorou todas essas áreas e ainda trouxe todos os detalhes? Não consigo acreditar! Você é um verdadeiro tesouro, sabia disso?",
                    "Muito obrigado por fazer isso! Você cobriu tantas áreas, e estou absolutamente empolgado para ouvir o que descobriu. Quero dizer, quem mais poderia ter feito isso?",
                    "Mal posso acreditar que você passou por todos esses lugares! Você deve ter visto coisas incríveis. Mal posso esperar para saber tudo o que encontrou!",
                    "Uau, você realmente assumiu muita responsabilidade! Estou impressionado que você conseguiu explorar todas essas áreas e ainda retornar com relatórios tão detalhados. Você é um verdadeiro explorador!",
                    "Você é uma lenda! Explorar todas essas áreas e relatar de volta? Isso não é algo que qualquer um pode fazer. Você alegrou meu dia com todas essas informações!",
                    "Aha! Eu sabia que você podia fazer isso! Você explorou esses lugares e voltou com todos os detalhes interessantes! Tenho que dizer, estou impressionado!",
                    "Obrigado, obrigado, obrigado! Você descobriu tanto, e estou tão animado para saber sobre tudo o que viu. Você realmente foi além do esperado!",
                    "Você realmente fez isso, não foi? Todas essas áreas exploradas e agora você está de volta com tudo o que precisamos! Não sei como te agradecer o suficiente—sério!",
                    "Eu nem sei como te agradecer o suficiente por explorar todos esses lugares. Você certamente merece o título de melhor aventureiro por aí!",
                    "Você se superou! Quem mais poderia ter conseguido explorar todas essas áreas e depois voltar com detalhes tão fantásticos? Você é incrível!"
                    };
                return new string[] {
                "Oh, wow! You’ve really outdone yourself! You explored all those areas and actually brought back all the details? I can’t believe it! You’re a real treasure, you know that?",
                "Thank you so much for doing this! You’ve covered so many areas, and I’m absolutely thrilled to hear what you’ve discovered. I mean, who else could have done it?",
                "I can hardly believe you went through all those places! You must have seen some incredible things. I can’t wait to hear everything you found out!",
                "Wow, you really took on a lot! I’m amazed you managed to explore all those areas and actually return with such thorough reports. You’re a true explorer!",
                "You’re a legend! Exploring all those areas and reporting back? That’s not something just anyone can do. You’ve made my day with all this info!",
                "Aha! I knew you could do it! You’ve gone and explored those places and come back with all the juicy details! I’ve got to say, I’m impressed!",
                "Thank you, thank you, thank you! You’ve uncovered so much, and I’m so excited to hear about all the things you saw. You really went above and beyond!",
                "You really did it, didn’t you? All those areas explored and now you’re back with everything we need! I can’t tell you how much I appreciate this—seriously!",
                "I don’t even know how to thank you enough for exploring all those places. You’ve certainly earned your title as the best adventurer around!",
                "You’ve outdone yourself! Who else could have managed to explore all those areas and then come back with such fantastic details? You’re incredible!"
                };
            }
        }
    }
}