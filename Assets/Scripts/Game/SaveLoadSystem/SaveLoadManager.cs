using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Game.GameManager;
using Game.LevelSelection;
using Game.MenuManager;
using UnityEngine;

namespace Game.SaveLoadSystem
{
    public class SaveLoadManager : MonoBehaviour
    {
        private static string _savePath;
        private static string _keyPath;
        private static string _key;
        private const int XOrKey = 1243522;

        private void Awake()
        {
            _savePath = $"{Application.persistentDataPath}/save.txt";
            _keyPath = $"{Application.persistentDataPath}/key.txt";
            if (!File.Exists(_keyPath))
            {
                using var rijndael = System.Security.Cryptography.Rijndael.Create();
                rijndael.GenerateKey();
                _key = Convert.ToBase64String(rijndael.Key);
            }
            else
            {
                _key = File.ReadAllText(_keyPath);
                _key = EncryptionManager.EncryptDecrypt(_key, XOrKey);
            }
        }

        private void OnEnable()
        {
            RealTimeLevelSelectManager.SaveStateHandler += Save;
            GameOverPanelBhv.RestartLevelEventHandler += Save;
            GameManagerSingleton.LoadStateHandler += Load;
        }
        
        private void OnDisable()
        {
            RealTimeLevelSelectManager.SaveStateHandler -= Save;
            GameOverPanelBhv.RestartLevelEventHandler -= Save;
            GameManagerSingleton.LoadStateHandler -= Load;
        }

        private static void Save(object sender, EventArgs e)
        {
	        Save();
        }

        public static bool HasSaveFile()
        {
            return File.Exists(_savePath);
        }

        private static void Save()
        {
            var state = LoadFile();
            SaveState(state);
            SaveFile(state);
            _key = EncryptionManager.EncryptDecrypt(_key, XOrKey);
            File.WriteAllText(_keyPath, _key);
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
            using var stream = File.Open(_savePath, FileMode.Create);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }

        private static Dictionary<string, object> LoadFile()
        {
            if (!File.Exists(_savePath))
            {
                Debug.LogWarning("No save file found");
                return new Dictionary<string, object>();
            }

            using var stream = File.Open(_savePath, FileMode.Open);
            if (stream.Length == 0)
            {
                return new Dictionary<string, object>();
            }
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