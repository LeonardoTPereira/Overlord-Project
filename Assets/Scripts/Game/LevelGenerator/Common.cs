using System;

namespace Game.LevelGenerator
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
        public static class RoomType
        {
            public static readonly int EMPTY = 0;
            public static readonly int CORRIDOR = 100;
            public static readonly int NOTHING = 101;
            public static readonly int BOSS = 102;
            public static readonly int LEAF = 103;
            public static readonly int START = 104;
            public static readonly int LOCKED = 105;
        }
    }
}