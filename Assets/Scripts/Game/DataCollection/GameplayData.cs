using System;
using FirebaseWebGLBridge = FirebaseWebGL.Scripts.FirebaseBridge;
using UnityEngine;

#if !UNITY_WEBGL || UNITY_EDITOR
using Firebase.Firestore;
using Firebase.Extensions;
#endif

namespace Game.DataCollection
{
    public class GameplayData
    {
        public void SendProfileToServer(PlayerData playerData)
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            DocumentReference docRef = db.Collection("users").Document(playerData.PlayerId.ToString());
            docRef.SetAsync(playerData).ContinueWithOnMainThread(task => {
                Debug.Log("Added data to the alovelace document in the users collection.");
            });
#else
            Debug.Log("Player data: "+playerData.PlayerId);
            String jsonData = JsonUtility.ToJson(playerData);
            FirebaseWebGLBridge.FirebaseFirestore.AddDocument("users", jsonData, playerData.PlayerId.ToString(), "DisplayInfo", "DisplayErrorObject");
            Debug.Log("Added document");
#endif
        }
    }
}