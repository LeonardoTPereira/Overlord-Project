﻿using System;

namespace LevelGenerator
{
    public static class Util
    {
        public enum Direction
        {
            right = 0,
            down = 1,
            left = 2
        };
        public static Random rnd = new Random();
        private static int ID = 0;

        public static int getNextId()
        {
            return ID++;
        }
        public static bool IsValidUri(string uri)
        {
            if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                return false;
            Uri tmp;
            if (!Uri.TryCreate(uri, UriKind.Absolute, out tmp))
                return false;
            return tmp.Scheme == Uri.UriSchemeHttp || tmp.Scheme == Uri.UriSchemeHttps;
        }

        public static bool OpenUri(string uri)
        {
            if (!IsValidUri(uri))
                return false;
            System.Diagnostics.Process.Start(uri);
            return true;
        }
    }

    static class Constants
    {
        public const int MAX_CHILDREN = 3;
        public const float PROB_HAS_CHILD = 100f;
        public static readonly float[] CHILD_PROB = { 100f / 3, 100f / 3, 100f / 3 };
        public const float PROB_1_CHILD = 100f / 3;
        public const float PROB_2_CHILD = 100f / 3;
        public const float PROB_3_CHILD = 100f / 3;
        public const float PROB_CHILD = 100f / 3;
        public const float PROB_NORMAL_ROOM = 70f;
        public const float PROB_KEY_ROOM = 15f;
        public const float PROB_LOCKER_ROOM = 15f;
        public const int MAX_DEPTH = 20;
        public const float NEIGHBORINGFACTOR = 0.7f;
        public const float CROSSOVER_RATE = 90f;
        public const float MUTATION_RATE = 5f;
        public const float MUTATION0_RATE = 50f;
        public const float MUTATION1_RATE = 50f;
        public const int POP_SIZE = 50;
        public const int GENERATIONS = 30;
        public const int PROB_SHORTCUT = 0;
        public const int MATRIXOFFSET = 150;
        public const string RESULTSFILE = "RESULTS";
        //Fitness parameters
        public const int nV = 30, nK = 4, nL = 4;
        public const float lCoef = 1.5f;
    }


}