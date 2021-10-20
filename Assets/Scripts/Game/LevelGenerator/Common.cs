using System;
using System.Collections.Generic;

namespace LevelGenerator
{
    /// This class holds the project common functions and constants.
    public static class Common
    {
        /// Unknown reference.
        public static readonly int UNKNOWN = -1;
        /// The error message of cannot compare individuals.
        public static readonly string CANNOT_COMPARE_INDIVIDUALS =
            "There is no way of comparing two null individuals.";
        /// The error message of problem in swap children.
        public static readonly string PROBLEM_IN_THE_DUNGEON =
            "There is something wrong with the dungeon representation.";

        /// The directions in which a room may connect to other rooms.
        ///
        /// The direction `up` is not listed here to avoid positioning conflict
        /// during the room placement.
        public enum Direction
        {
            Right = 0,
            Down = 1,
            Left = 2
        };

        /// Return the array of all weapon types.
        public static Direction[] AllDirections()
        {
            return (Direction[]) Enum.GetValues(typeof(Direction));
        }

        /// Define the room codes for printing purposes.
        public enum RoomCode
        {
            N = 0,   // Room
            C = 100, // Corridor
            B = 101, // Boss room or dungeon exit
            E = 102, // Empty space
        }

        /// Return a random integer percentage (from 0 to 99, 100 numbers).
        public static int RandomPercent(
            ref Random _rand
        ) {
            return _rand.Next(100);
        }

        /// Return a random integer number from the entered inclusive range.
        public static int RandomInt(
            (int min, int max) _range,
            ref Random _rand
        ) {
            return _rand.Next(_range.min, _range.max + 1);
        }

        /// Return a random element from the entered array.
        public static T RandomElementFromArray<T>(
            T[] _range,
            ref Random _rand
        ) {
            return _range[_rand.Next(0, _range.Length)];
        }

        /// Return a random element from the entered list.
        public static T RandomElementFromList<T>(
            List<T> _range,
            ref Random _rand
        ) {
            return _range[_rand.Next(0, _range.Count)];
        }
    }
}