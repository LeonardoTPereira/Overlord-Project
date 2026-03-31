using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlord.GenerationController
{ 
    public static class GenerationStatus
    {
        public static bool StartedDungeonGeneration { get; set; } = false;
        public static bool EndedDungeonGeneration { get; set; } = false;
    }
}