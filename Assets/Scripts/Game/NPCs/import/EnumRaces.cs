using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EnumRaces
{
    public class Races
    {
        public enum raceID
        {
            Aarakocra = 0,
            Anao = 1,
            Draconato = 2,
            Elfo = 3,
            Warforged = 4,
            Gnomo = 5,
            Goblin = 6,
            Golias = 7,
            Halfling = 8,
            Humano = 9,
            Kobold = 10,
            Orc = 11,
            Tabaxi = 12,
            Tiefling = 13,
            NumberOfRaces
        }

        public static int NumberOfRaces()
        {
            return (int)raceID.NumberOfRaces;
        }
    }
}