using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestCategory
{
    Mastery,
    Immersion,
    Creativity,
    Achievement
}

public class QuestLoader : MonoBehaviour
{
    public static QuestLoader Instance;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    public string[] GetQuestSentence(string questType)
    {
        switch (questType)
        {
            //Mastery
            case "kill":
            case "damage":
                return new string[] { "Elimine algumas naves inimigas.", "Eles estão por toda parte.", "E não se preocupe. Isso é apenas um jogo e ninguém será machucado." };

            //Immersion
            case "listen":
            case "read":
            case "report":
            case "give":
                {
                    return new string[] { "Veja bem, a missão é o seguinte.", "Ouvi dizer que há um planeta por perto cheio de insetos.", 
                        "Insetos gigantescos do tamanho de naves inteiras.", "Dizem a rainha do enxame possui um sangue que concede poderes inimagináveis para outras naves.",
                    "Mas infelizmente esse jogo é apenas um protótipo e você não poderá completar essa missão por enquanto...", "Na verdade, só de ler isso, você acaba de completar a missão, olha só."};
                }

            //Creativity
            case "explore":
            case "goto":
                return new string[] { "Explore essa 'dungeon'", "Que não é uma dungeon, é o espaço.", "Que por algum motivo tem setas que te teleportam para outros lugares." };

            //Achievement
            case "gather":
            case "exchange":
                return new string[] { "Colete alguns PugoPoints.", "Que são uns quadradinhos azuis que as vezes aparecem em algumas salas.", "Cuidado que elas caem de pressa." };

            default:
                return new string[] { "Complete the quest objective." };

        }
    }
}
