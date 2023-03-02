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
            var docRef = db.Collection("users").Document(playerData.SerializedData.PlayerId.ToString());
            docRef.SetAsync(playerData.SerializedData).ContinueWithOnMainThread(_ => {
                Debug.Log($"Added data to the {playerData.SerializedData.PlayerId.ToString()} document in the users collection.");
            });
            var dungeonData = playerData.CurrentDungeon;
            docRef = db.Collection("dungeons").Document(playerData.SerializedData.PlayerId+"-"+dungeonData.LevelName+"|"+dungeonData.TotalAttempts);
            docRef.SetAsync(dungeonData, SetOptions.MergeAll).ContinueWithOnMainThread(_ => {
                Debug.Log($"Added data to the {dungeonData.LevelName} document in the dungeons collection.");
            });
/*#else
            var jsonData = JsonUtility.ToJson(playerData);
            FirebaseWebGLBridge.FirebaseFirestore.AddDocument("users", jsonData, playerData.SerializedData.PlayerId.ToString(), "DisplayInfo", "DisplayErrorObject");
            var dungeonData = playerData.CurrentDungeon;
            jsonData = JsonUtility.ToJson(dungeonData);
            FirebaseWebGLBridge.FirebaseFirestore.AddDocument("dungeons", jsonData, dungeonData.LevelName, "DisplayInfo", "DisplayErrorObject");
            */
#endif
        }
    }
}