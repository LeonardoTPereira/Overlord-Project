using System;
using System.Collections.Generic;
using System.Diagnostics;
using Util;

namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    /// Alias for the coordinate of MAP-Elites matrix.
    using Coordinate = ValueTuple<int, int>;

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
        public static Individual[] Select(int amount, int competitors, Population population) {
            List<Coordinate> availableCompetitors = population.GetElitesCoordinates();
            Debug.Assert(availableCompetitors.Count - amount > competitors, NOT_ENOUGH_COMPETITORS);
            Individual[] individuals = new Individual[amount];
            for (int i = 0; i < amount; i++)
            {
                (Coordinate coordinate, Individual individual) = Tournament(competitors, population, new List<Coordinate> (availableCompetitors));
                individuals[i] = individual;
                availableCompetitors.Remove(coordinate);
            }
            return individuals;
        }

        /// Perform tournament selection of a single individual.
        ///
        /// This function ensures that the same individual will not be selected
        /// for the same tournament selection process. To do so, we apply the
        /// same process explained in `Select` function.
        private static (Coordinate, Individual) Tournament(int totalCompetitors, Population population, List<Coordinate> availableCompetitors) {
            Individual[] competitors = new Individual[totalCompetitors];
            Coordinate[] coordinates = new Coordinate[totalCompetitors];
            for (int i = 0; i < totalCompetitors; i++)
            {
                Coordinate selectedCoordinate = RandomSingleton.GetInstance().RandomElementFromList(availableCompetitors);
                competitors[i] = population.map[selectedCoordinate.Item1, selectedCoordinate.Item2];
                coordinates[i] = selectedCoordinate;
                availableCompetitors.Remove(selectedCoordinate);
            }
            Individual winner = null;
            Coordinate coordinate = (Common.UNKNOWN, Common.UNKNOWN);
            for (int i = 0; i < totalCompetitors; i++)
            {
                if (winner is not null && competitors[i].Fitness.result <= winner.Fitness.result) continue;
                winner = competitors[i];
                coordinate = coordinates[i];
            }
            return (coordinate, winner);
        }
    }
}