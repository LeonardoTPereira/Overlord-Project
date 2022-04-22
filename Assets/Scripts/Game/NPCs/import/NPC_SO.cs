using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FitnessScript;

public class NPC_SO : ScriptableObject
{
    //trocar identity por nome
    [SerializeField]
    public string identity;
    [SerializeField]
    public int age;
    [SerializeField]
    public int job;
    [SerializeField]
    public int race;
    //ter atributo fitness guardado pra nao ficar calculando  s e m p r e

    // Tudo varia de 0 a 10
    // Dita NPC 'experience'
    [SerializeField]
    public int trauma;
    // Dita como o NPC manipula ou atribui tarefas
    [SerializeField]
    public int violence;
    // Dita os interesses do NPC com relação a materiais/efeitos/itens
    [SerializeField]
    public int attain;
    // Dita a curiosidade do NPC em geral
    [SerializeField]
    public int explore;
    // Dita a probabilidade do NPC de conversar ou conhecer mais gente
    [SerializeField]
    public int social;

    public void Init(int race, int job)
    {
        identity = "To be Defined";
        age = (Random.Range(0, 100));
        this.job = job;
        this.race = race;
        trauma = (Random.Range(0, 10));
        violence = (Random.Range(0, 10));
        attain = (Random.Range(0, 10));
        explore = (Random.Range(0, 10));
        social = (Random.Range(0, 10));

        return;
    }

    public void Init(string identity, int age, int job, int race, int trauma, int violence, int attain, int explore, int social)
    {
        this.identity = identity;
        this.age = age;
        this.job = job;
        this.race = race;
        this.trauma = trauma;
        this.violence = violence;
        this.attain = attain;
        this.explore = explore;
        this.social = social;

        return;
    }

    // Print de Debug
    public void PrintNPCData(int n, int questType)
    {
        Debug.Log("Fitness do individuo: " + n + ": " + Fitness.Calculate(this, questType));

        Debug.Log(identity);
        Debug.Log(age);
        Debug.Log(job);
        Debug.Log(race);
        Debug.Log(trauma);
        Debug.Log(violence);
        Debug.Log(attain);
        Debug.Log(explore);
        Debug.Log(social);

        return;
    }

    // Print de Debug (sobrecarga)
    public void PrintNPCData(int questType)
    {

        Debug.Log(identity);
        Debug.Log(age);
        Debug.Log(job);
        Debug.Log(race);
        Debug.Log(trauma);
        Debug.Log(violence);
        Debug.Log(attain);
        Debug.Log(explore);
        Debug.Log(social);

        return;
    }
}
