using System;
using MyBox;
using UnityEngine;

namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    [Serializable]
    public class Parameters
    {
        [Foldout("EA Parameters", true)]
        [SerializeField, Range(1, 120)] private int time = 60;
        [SerializeField, Range(1, 100)] private int population = 25;
        [SerializeField, Range(0, 100)] private int mutation = 10;
        [SerializeField, Range(1, 10)] private int competitors = 2;
        [SerializeField, Range(1, 40)] private int minimumElite = 8;
        [SerializeField, Range(0.0f, 10.0f)] private float acceptableFitness = 0.75f;
        [SerializeField] private FitnessParameters fitnessParameters;
        
        public Parameters(int time, int population, int mutation, int competitors, int minimumElite,
            float acceptableFitness, FitnessParameters fitness) 
        {
            Time = time;
            Population = population;
            Mutation = mutation;
            Competitors = competitors;
            MinimumElite = minimumElite;
            AcceptableFitness = acceptableFitness;
            FitnessParameters = fitness;
        }

        public Parameters(FitnessParameters fitnessParameters)
        {
            FitnessParameters = fitnessParameters;
        }
        
        public int Time
        {
            get => time;
            set => time = value;
        }
        
        public int Population
        {
            get => population;
            set => population = value;
        }

        public int Mutation
        {
            get => mutation;
            set => mutation = value;
        }

        public int Competitors
        {
            get => competitors;
            set => competitors = value;
        }

        public int MinimumElite
        {
            get => minimumElite;
            set => minimumElite = value;
        }

        public float AcceptableFitness
        {
            get => acceptableFitness;
            set => acceptableFitness = value;
        }
        
        public FitnessParameters FitnessParameters
        {
            get => fitnessParameters;
            set => fitnessParameters = value;
        }
    }
}