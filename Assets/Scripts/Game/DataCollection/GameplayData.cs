using UnityEngine;
#if !UNITY_WEBGL || UNITY_EDITOR
    using Firebase.Firestore;
    using Firebase.Extensions;
#else
    using FirebaseWebGLBridge = FirebaseWebGL.Scripts.FirebaseBridge;
#endif

namespace Game.DataCollection
{
    public class GameplayData
    {
        public void SendProfileToServer(PlayerData playerData)
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            var db = FirebaseFirestore.DefaultInstance;
            var docRef = db.Collection("users").Document(playerData.PlayerId.ToString());
            docRef.SetAsync(playerData).ContinueWithOnMainThread(_ => {
                Debug.Log($"Added data to the {playerData.PlayerId.ToString()} document in the users collection.");
            });
            var dungeonData = playerData.CurrentDungeon;
            docRef = db.Collection("dungeons").Document(dungeonData.LevelName);
            docRef.SetAsync(playerData).ContinueWithOnMainThread(_ => {
                Debug.Log($"Added data to the {dungeonData.LevelName} document in the dungeons collection.");
            });
#else
            var jsonData = JsonUtility.ToJson(playerData);
            FirebaseWebGLBridge.FirebaseFirestore.AddDocument("users", jsonData, playerData.PlayerId.ToString(), "DisplayInfo", "DisplayErrorObject");
            var dungeonData = playerData.CurrentDungeon;
            jsonData = JsonUtility.ToJson(dungeonData);
            FirebaseWebGLBridge.FirebaseFirestore.AddDocument("dungeons", jsonData, dungeonData.LevelName, "DisplayInfo", "DisplayErrorObject");
#endif
        }
    }
}