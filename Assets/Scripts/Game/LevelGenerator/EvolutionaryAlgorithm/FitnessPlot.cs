using System;
using UnityEngine;

namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    [Serializable]
    public class FitnessPlot : MonoBehaviour
    {
        [SerializeField] private AnimationCurve resultPlot = new();
        [SerializeField] private AnimationCurve distancePlot = new();
        [SerializeField] private AnimationCurve usagePlot = new();
        [SerializeField] private AnimationCurve sparsityPlot = new();
        [SerializeField] private AnimationCurve stdDeviationPlot = new();
        public float distance;
        public float usage;
        public float sparsity;
        public float enemyStandardDeviation;
        public float result;
        private int _currentGeneration = -1;
        private int _individualsOnGeneration;
        public float distanceCurve;
        public float usageCurve;
        public float sparsityCurve;
        public float enemyStandardDeviationCurve;
        public float resultCurve;
        
        [SerializeField] private AnimationClip animationClip;
        
        private void Start()
        {
            animationClip.legacy = true;
        }

        public void UpdateFitnessPlotData(Individual individual, int generation)
        {
            if (generation != _currentGeneration)
            {
                Plot();
                distance = 0;
                usage = 0;
                sparsity = 0;
                enemyStandardDeviation = 0;
                result = 0;
                _currentGeneration = generation;
                _individualsOnGeneration = 0;
            }
            _individualsOnGeneration++;
            distance += individual.Fitness.Distance;
            usage += individual.Fitness.Usage;
            sparsity += individual.Fitness.EnemySparsity;
            enemyStandardDeviation += individual.Fitness.EnemyStandardDeviation;
            result += individual.Fitness.NormalizedResult;
        }
        
        public void UpdateFitnessPlotClassicData(Individual individual, int generation)
        {
            if (generation != _currentGeneration)
            {
                ClassicPlot();
                distance = 0;
                usage = 0;
                sparsity = 0;
                enemyStandardDeviation = 0;
                result = 0;
                _currentGeneration = generation;
                _individualsOnGeneration = 0;
            }
            distance = individual.Fitness.NormalizedDistance;
            usage = individual.Fitness.NormalizedUsage;
            sparsity = individual.Fitness.NormalizedEnemySparsity;
            enemyStandardDeviation = individual.Fitness.NormalizedEnemyStandardDeviation;
            result = individual.Fitness.NormalizedResult;
        }

        private void Plot()
        {
            resultPlot.AddKey(_currentGeneration, result / _individualsOnGeneration);
            distancePlot.AddKey(_currentGeneration, distance / _individualsOnGeneration);
            usagePlot.AddKey(_currentGeneration, usage / _individualsOnGeneration);
            sparsityPlot.AddKey(_currentGeneration, sparsity / _individualsOnGeneration);
            stdDeviationPlot.AddKey(_currentGeneration, enemyStandardDeviation / _individualsOnGeneration);
        }
        
        private void ClassicPlot()
        {
            resultPlot.AddKey(_currentGeneration, result);
            distancePlot.AddKey(_currentGeneration, distance);
            usagePlot.AddKey(_currentGeneration, usage);
            sparsityPlot.AddKey(_currentGeneration, sparsity);
            stdDeviationPlot.AddKey(_currentGeneration, enemyStandardDeviation);
        }

        public void AddAnimationCurves()
        {
            animationClip.SetCurve("", typeof(FitnessPlot), "distanceCurve", resultPlot);
            animationClip.SetCurve("", typeof(FitnessPlot), "usageCurve", usagePlot);
            animationClip.SetCurve("", typeof(FitnessPlot), "sparsityCurve", sparsityPlot);
            animationClip.SetCurve("", typeof(FitnessPlot), "enemyStandardDeviationCurve", stdDeviationPlot);
            animationClip.SetCurve("", typeof(FitnessPlot), "resultCurve", distancePlot);
        }
    }
}