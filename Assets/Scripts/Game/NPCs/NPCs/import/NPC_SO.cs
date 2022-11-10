using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FitnessScript;

namespace Game.NPCsa
{
    public class NPC_SO : ScriptableObject
    {
        //trocar identity por nome
        [SerializeField]
        public string identity;
        [SerializeField]
        public int age;
        [SerializeField]
        public int quest;
        [SerializeField]
        public int combination;
        //ter atributo fitness guardado pra nao ficar calculando  s e m p r e

        // Tudo varia de 0 a 10
        // Dita NPC 'experience'
        [SerializeField]
        public float trauma;
        // Dita como o NPC manipula ou atribui tarefas
        [SerializeField]
        public float violence;
        // Dita os interesses do NPC com relação a materiais/efeitos/itens
        [SerializeField]
        public float attain;
        // Dita a curiosidade do NPC em geral
        [SerializeField]
        public float explore;
        // Dita a probabilidade do NPC de conversar ou conhecer mais gente
        [SerializeField]
        public float social;
        // peso Px
        [SerializeField]
        public float race;
        // peso Py
        [SerializeField]
        public float job;
        // atrmax
        [SerializeField]
        public int atrmax;


        public float fitness;
        public float fitness1;
        public float fitness2;
        public float funSoc;
        public float funAtt;
        public float funVio;
        public float funExp;

        public void Init()
        {
            atrmax = Random.Range(10, 30);
            race = Random.Range(0.0f, 1.0f);
            job = Random.Range(0.0f, 1.0f);

            violence = Random.Range(1f, 10f);
            attain = Random.Range(1, 10);
            explore = Random.Range(1, 10);
            social = Random.Range(1, 10);

            UpdateFitness();

            return;
        }

        public void Init(int atrmax, float race, float job, float violence, float attain, float explore, float social)
        {
            this.atrmax = atrmax;
            this.race = race;
            this.job = job;

            this.violence = violence;
            this.attain = attain;
            this.explore = explore;
            this.social = social;

            UpdateFitness();

            return;
        }

        public void UpdateFitness()
        {
            fitness = Fitness.Calculate(this);
            fitness1 = Fitness.Fitness1(this);
            fitness2 = Fitness.Fitness2(this);
            funSoc = Fitness.Social(this);
            funAtt = Fitness.Attain(this);
            funVio = Fitness.Violence(this);
            funExp = Fitness.Explore(this);
        }
    }
}