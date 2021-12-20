using System.Collections.Generic;
using UnityEditor;

namespace ScriptableObjects.SerializableDictionaryLite.Editor
{
    #if UNITY_EDITOR
    [InitializeOnLoad]
    public class Definer
    {
        static Definer()
        {
            List<string> defines = new List<string>(1)
            {
                "RH_SerializedDictionary"
            };
            
            RotaryHeart.Lib.Definer.ApplyDefines(defines);
        }
    }
    #endif
}