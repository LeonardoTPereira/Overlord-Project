using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnumRaces
{
    public class Races
    {
        public enum raceID
        {
            Ogro,
            Anao,
            Humano,
            Elfo,
            NumberOfRaces
        }

        public static int NumberOfRaces()
        {
            return (int)raceID.NumberOfRaces;
        }
    }
}