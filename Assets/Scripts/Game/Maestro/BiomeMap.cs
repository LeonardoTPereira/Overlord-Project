using System;
using System.Collections.Generic;
using System.Linq;
using Game.LevelGenerator.EvolutionaryAlgorithm;
using Util;

namespace Game.Maestro
{
    public class BiomeMap
    {

        //exploration x leniency
        private static readonly Dictionary<Biomes, Coordinates[]> CoordinatesByBiomes =
            new()
            {
                [Biomes.Cave] = new[] {new Coordinates(3, 2), new Coordinates(3, 3), new Coordinates(4, 2)},
                [Biomes.Ocean] = new[] {new Coordinates(3, 4), new Coordinates(4, 3), new Coordinates(4, 4)},
                [Biomes.Meadow] = new[] {new Coordinates(0, 0), new Coordinates(0, 1), new Coordinates(1, 0)},
                [Biomes.Volcano] = new[] {new Coordinates(3, 0), new Coordinates(4, 0), new Coordinates(4, 1)},
                [Biomes.Ice] = new[] {new Coordinates(2, 2), new Coordinates(2, 3), new Coordinates(2, 4)},
                [Biomes.Graveyard] = new[] {new Coordinates(0, 3), new Coordinates(0, 4), new Coordinates(1, 4)},
                [Biomes.Forest] = new[] {new Coordinates(0, 2), new Coordinates(1, 2), new Coordinates(1, 3)},
                [Biomes.Desert] = new[] {new Coordinates(1, 1), new Coordinates(2, 0), new Coordinates(2, 1), new Coordinates(3, 1)}
            };

        public enum Biomes
        {
            Cave,
            Ocean,
            Meadow,
            Volcano,
            Ice,
            Graveyard,
            Forest,
            Desert
        }
        public Dictionary<Biomes, List<Individual>> LevelsByBiome { get; set; }
        public int BiomesWithElites { get; private set; }

        public BiomeMap()
        {
            LevelsByBiome = new Dictionary<Biomes, List<Individual>>();
            foreach (var biome in Enum.GetValues(typeof(Biomes)).Cast<Biomes>())
            {
                LevelsByBiome.Add(biome, new List<Individual>());
            }
            BiomesWithElites = 0;
        }

        public void UpdateBiomes(in MapElites mapElites)
        {
            BiomesWithElites = 0;
            foreach (var biome in Enum.GetValues(typeof(Biomes)).Cast<Biomes>())
            {
                var elites = UpdateBiome(mapElites, CoordinatesByBiomes[biome], biome.ToString());
                if (elites.Count > 0)
                {
                    BiomesWithElites++;
                }
                LevelsByBiome[biome] = elites;
            }
        }

        private List<Individual> UpdateBiome(in MapElites mapElites, in Coordinates[] coordinates, in string biomeName)
        {
            var elites = new List<Individual>();
            foreach (var coordinate in coordinates)
            {
                var elite = mapElites.GetElite(coordinate.X, coordinate.Y);
                if (elite == null) continue;
                elite.BiomeName = biomeName;
                elites.Add(elite);
            }
            return elites;
        }

        public int BiomesWithElitesBetterThan(float value)
        {
            var betterBiomesCount = 0;
            foreach (var biome in Enum.GetValues(typeof(Biomes)).Cast<Biomes>())
            {
                var elites = LevelsByBiome[biome];
                if (elites.Any(elite => elite.Fitness.NormalizedResult < value))
                {
                    betterBiomesCount++;
                }
            }
            return betterBiomesCount;
        }

        public List<Individual> GetBestEliteForEachBiome()
        {
            var selectedLevels = new List<Individual>();
            foreach (var biome in Enum.GetValues(typeof(Biomes)).Cast<Biomes>())
            {
                var elites = LevelsByBiome[biome];
                selectedLevels.Add(GetBestEliteForBiome(elites));
            }
            return selectedLevels;
        }

        private static Individual GetBestEliteForBiome(List<Individual> elites)
        {
            Individual bestElite = null;
            foreach (var elite in elites)
            {
                if (bestElite == null)
                {
                    bestElite = elite;
                }
                else
                {
                    if (elite.IsBetterThan(bestElite))
                    {
                        bestElite = elite;
                    }
                }
            }
            return bestElite;
        }
    }
}