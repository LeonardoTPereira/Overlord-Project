using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Game.SaveLoadSystem
{
    public class SaveLoadManager : MonoBehaviour
    {
        private static readonly string SavePath = $"{Application.persistentDataPath}/save.txt";
        private static readonly string KeyPath = $"{Application.persistentDataPath}/key.txt";
        private static string _key;
        private const int XOrKey = 1243522;

        private void Awake()
        {
            if (!File.Exists(KeyPath))
            {
                using var rijndael = System.Security.Cryptography.Rijndael.Create();
                rijndael.GenerateKey();
                _key = Convert.ToBase64String(rijndael.Key);
            }
            else
            {
                _key = File.ReadAllText(KeyPath);
                _key = EncryptionManager.EncryptDecrypt(_key, XOrKey);
            }
        }

        private void Start()
        {
            Load();
        }

        public static bool HasSaveFile()
        {
            return File.Exists(SavePath);
        }

        public static void Save()
        {
            var state = LoadFile();
            SaveState(state);
            SaveFile(state);
            _key = EncryptionManager.EncryptDecrypt(_key, XOrKey);
            File.WriteAllText(KeyPath, _key);
        }

        
        /*
         *         private void SaveFile(object state)
        {
            using var stream = File.Open(_savePath, FileMode.Create);
            using var cleanStream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(cleanStream, state);
            var encryptedStream = EncryptionManager.EncryptString(_key, cleanStream.ToString());
            File.WriteAllText(_savePath, encryptedStream);
        }

        private Dictionary<string, object> LoadFile()
        {
            if (!File.Exists(_savePath))
            {
                Debug.LogWarning("No save file found");
                return null;
            }

            using var stream = File.Open(_savePath, FileMode.Open);
            var encryptedData = File.ReadAllText(_savePath);
            var cleanData = EncryptionManager.DecryptString(_key, encryptedData);
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>) formatter.Deserialize(cleanData);
        }
         */
        public void Load()
        {
            var state = LoadFile();
            LoadState(state);
        }
        
        private static void SaveFile(object state)
        {
            using var stream = File.Open(SavePath, FileMode.Create);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }

        private static Dictionary<string, object> LoadFile()
        {
            if (!File.Exists(SavePath))
            {
                Debug.LogWarning("No save file found");
                return null;
            }

            using var stream = File.Open(SavePath, FileMode.Open);
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>) formatter.Deserialize(stream);
        }

        static void SaveState(Dictionary<string, object> state)
        {
            var saveableEntities = FindObjectsOfType<SaveableEntity>();
            foreach (var saveable in saveableEntities)
            {
                state[saveable.Id] = saveable.SaveState();
            }
        }        
        void LoadState(Dictionary<string, object> state)
        {
            var saveableEntities = FindObjectsOfType<SaveableEntity>();
            foreach (var saveable in saveableEntities)
            {
                if (state.TryGetValue(saveable.Id, out var savedState))
                {
                    saveable.LoadState(savedState);
                }
            }
        }
    }
}