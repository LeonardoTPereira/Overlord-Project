using System;
using System.Collections.Generic;

namespace LevelGenerator
{
    /// Alias for the coordinate of MAP-Elites matrix.
    using Coordinate = System.ValueTuple<int, int>;

    /// This struct represents a MAP-Elites population.
    ///
    /// The MAP-Elites population is an N-dimensional array of individuals,
    /// where each matrix's ax corresponds to a different feature.
    ///
    /// This particular population is mapped into the level's coefficient of
    /// exploration and leniency. Thus, each Elite (or matrix cell) corresponds
    /// to a combination of a different degree of exploration and leniency.
    public struct Population
    {
        /// The MAP-Elites dimension. The dimension is defined by the number of
        /// ranges of coefficient of exploration and leniency.
        public (int exp, int len) dimension { get; }
        /// The MAP-Elites map (a matrix of individuals).
        public Individual[,] map { get; }

        /// Population constructor.
        public Population(
            int _exp,
            int _len
        ) {
            dimension = (_exp, _len);
            map = new Individual[dimension.exp, dimension.len];
        }

        /// Return the number of Elites of the population.
        public int Count()
        {
            int count = 0;
            for (int e = 0; e < dimension.exp; e++)
            {
                for (int l = 0; l < dimension.len; l++)
                {
                    if (map[e, l] != null)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /// Return a list corresponding to the Elites coordinates.
        public List<Coordinate> GetElitesCoordinates()
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            for (int e = 0; e < dimension.exp; e++)
            {
                for (int l = 0; l < dimension.len; l++)
                {
                    if (map[e, l] != null)
                    {
                        coordinates.Add((e, l));
                    }
                }
            }
            return coordinates;
        }

        /// Add an individual in the MAP-Elites population.
        ///
        /// First, we identify which Elite the individual is classified in.
        /// Then, if the corresponding Elite is empty, the individual is placed
        /// there. Otherwise, we compare the both old and new individuals, and
        /// the best individual is placed in the corresponding Elite.
        public void PlaceIndividual(
            Individual _individual
        ) {
            // Calculate the individual slot (Elite)
            float ce = _individual.exploration;
            float le = _individual.leniency;
            int e = SearchSpace.GetCoefficientOfExplorationIndex(ce);
            int l = SearchSpace.GetLeniencyIndex(le);
            // Check if the level is within the search space
            if (e < 0 ||
                e >= dimension.exp ||
                l < 0 ||
                l >= dimension.len
            ) {
                return;
            }
            // If the new individual deserves to survive
            if (Fitness.IsBest(_individual, map[e, l]))
            {
                // Then, place the individual in the MAP-Elites population
                map[e, l] = _individual;
            }
        }

        /// Return a list with the population individuals.
        public List<Individual> ToList()
        {
            List<Individual> list = new List<Individual>();
            for (int e = 0; e < dimension.exp; e++)
            {
                for (int l = 0; l < dimension.len; l++)
                {
                    list.Add(map[e, l]);
                }
            }
            return list;
        }

        /// Print all the individuals of the MAP-Elites population.
        public void Debug()
        {
            for (int e = 0; e < dimension.exp; e++)
            {
                for (int l = 0; l < dimension.len; l++)
                {
                    (float, float)[] listCE = SearchSpace.
                        CoefficientOfExplorationRanges();
                    (float, float)[] listLE = SearchSpace.
                        LeniencyRanges();
                    string log = "Elite ";
                    log += "CE" + listCE[e] + "-";
                    log += "LE" + listLE[l];
                    Console.WriteLine(log);
                    if (map[e, l] is null)
                    {
                        Console.WriteLine(LevelDebug.INDENT + "Empty");
                    }
                    else
                    {
                        map[e, l].Debug();
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}