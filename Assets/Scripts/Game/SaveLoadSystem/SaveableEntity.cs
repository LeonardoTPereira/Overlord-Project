using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.SaveLoadSystem
{
    public class SaveableEntity : MonoBehaviour
    {
        [field: SerializeField] public string Id { get; private set; }
        [ContextMenu("Generate Id")]
        public void GenerateId()
        {
            Id = Guid.NewGuid().ToString();
        }

        public object SaveState()
        {
            var state = new Dictionary<string, object>();
            var saveableComponents = GetComponents<ISaveable>();
            foreach (var saveable in saveableComponents)
            {
                state[saveable.GetType().ToString()] = saveable.SaveState();
            }

            return state;
        }

        public void LoadState(object state)
        {
            var stateDictionary = (Dictionary<string, object>) state;
            var saveableComponents = GetComponents<ISaveable>();
            foreach (var saveable in saveableComponents)
            {
                var typeName = saveable.GetType().ToString();
                if (stateDictionary.TryGetValue(typeName, out var savedState))
                {
                    saveable.LoadState(savedState);
                }
            }
        }
    }
}