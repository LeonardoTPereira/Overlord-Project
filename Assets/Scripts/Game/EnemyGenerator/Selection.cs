using System.Diagnostics;
using System.Collections.Generic;
using Util;

namespace Game.EnemyGenerator
{
    /// Alias for the coordinate of MAP-Elites matrix.
    using Coordinate = System.ValueTuple<int, int>;

    /// This class holds the selector operator.
    public static class Selection
    {
        /// The error message of not enough competitors.
        public static readonly string NOT_ENOUGH_COMPETITORS =
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
        public static Individual[] Select(int _amount,  int _competitors,  Population _pop) {
            // Get the list of Elites' coordinates (the available competitors)
            List<Coordinate> avco = _pop.GetElitesCoordinates();
            // Ensure the population size is enough for the tournament
            Debug.Assert(
                avco.Count - _amount > _competitors,
                NOT_ENOUGH_COMPETITORS
            );
            // Select `_amount` individuals
            Individual[] individuals = new Individual[_amount];
            for (int i = 0; i < _amount; i++)
            {
                // Perform tournament selection with `_competitors` competitors
                (Coordinate coordinate, Individual individual) = Tournament(
                    _competitors, // Number of competitors
                    _pop,         // Population
                    avco         // List of available competitors
                );
                // Select an individual and remove it from available competitors
                individuals[i] = individual;
                avco.Remove(coordinate);
            }
            return individuals;
        }

        /// Perform tournament selection of a single individual.
        ///
        /// This function ensures that the same individual will not be selected
        /// for the same tournament selection process. To do so, we apply the
        /// same process explained in `Select` function.
        static (Coordinate, Individual) Tournament(
            int _competitors,
            Population _pop,
            List<Coordinate> _avco
        ) {
            // List of available competitors
            List<Coordinate> avco = new List<Coordinate>(_avco);
            // Initialize the auxiliary variables
            Individual[] competitors = new Individual[_competitors];
            Coordinate[] coordinates = new Coordinate[_competitors];
            // Select the competitors randomly from the available coordinate
            // then remove the competitor from available competitors
            for (int i = 0; i < _competitors; i++)
            {
                Coordinate rc = RandomSingleton.GetInstance().RandomElementFromList(avco);
                competitors[i] = _pop.map[rc.Item1, rc.Item2];
                coordinates[i] = rc;
                avco.Remove(rc);
            }
            // Find the tournament winner and its coordinate in the population
            Individual winner = null;
            Coordinate coordinate = (Common.UNKNOWN, Common.UNKNOWN);
            for (int i = 0; i < _competitors; i++)
            {
                if (Fitness.IsBest(competitors[i], winner))
                {
                    winner = competitors[i];
                    coordinate = coordinates[i];
                }
            }
            // Return the tournament winner and its coordinate
            return (coordinate, winner);
        }
    }
}