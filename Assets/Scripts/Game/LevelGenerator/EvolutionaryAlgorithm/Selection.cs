using System;
using System.Diagnostics;

namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    /// This class holds the selector operator.
    public static class Selection
    {
        private static readonly string NOT_ENOUGH_COMPETITORS =
            "There are not enough individuals in the entered population to " +
            "perform this operation.";

        /// Select individuals from the MAP-Elites population.
        ///
        /// This function ensures that the same individual will not be selected
        /// for the same selection process. To do so, we use an auxiliary list
        /// composed of the individuals' coordinates in the MAP-Elites
        /// population. Instead of selecting directly an individual, we select
        /// its coordinate from the auxiliary list and remove it then it is not
        /// available for the next selection.
        public static Individual[] SelectParents(in int parentsNeeded, in int competitorsAmount, in Population population) 
        {
            Debug.Assert(population.TotalElites < competitorsAmount, NOT_ENOUGH_COMPETITORS);
            var parents = new Individual[parentsNeeded];
            for (var i = 0; i < parentsNeeded; ++i)
            {
                var competitors = new Individual[competitorsAmount];
                var selectedCount = 0;
                do
                {
                    Individual selected;
                    do
                    {
                        selected = population.GetRandomIndividualFromList();
                    } while (Array.IndexOf(competitors, selected) != -1);
                    competitors[selectedCount++] = selected;
                } while (selectedCount < competitorsAmount);
                parents[i] = new Individual(Tournament(competitors));
            }
            return parents;
        }

        /// Perform tournament selection of a single individual.
        ///
        /// This function ensures that the same individual will not be selected
        /// for the same tournament selection process. To do so, we apply the
        /// same process explained in `Select` function.
        private static Individual Tournament(in Individual[] competitors)
        {
            var totalCompetitors = competitors.Length;
            Individual winner = null;
            for (int i = 0; i < totalCompetitors; i++)
            {
                if (winner?.IsBetterThan(competitors[i])??false) continue;
                winner = competitors[i];
            }
            return winner;
        }
    }
}