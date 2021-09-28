using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class NarrativeConfigSO : ScriptableObject
    {
        public string narrativeFileName;
    }
}