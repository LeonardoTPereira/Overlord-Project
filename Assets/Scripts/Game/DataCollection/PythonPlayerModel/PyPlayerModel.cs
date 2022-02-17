using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Scripting.Python;
#endif
using UnityEngine;
using Python.Runtime;
using Game.Events;
using Game.DataInterfaces;
using System;

namespace Game.DataCollection
{
    public class PyPlayerModel : MonoBehaviour
    {
        const string kStateName = "com.unity.scripting.python.samples.pyside";
        // Used for testing
        // private void Start() {
        //     PredictPlayerModel( null, null );
        // }

        protected void OnEnable()
        {
            GameplayData.SendGameAndPlayerDataEventHandler += PredictPlayerModel;
        }

        protected void OnDisable()
        {
            GameplayData.SendGameAndPlayerDataEventHandler -= PredictPlayerModel;
        }

       static void PredictPlayerModel ( object sender, SendGameAndPlayerDataArgs eventArgs )
       {
            PlayerAndGameplayData data = eventArgs.PlayerGameplayData;
            PythonRunner.EnsureInitialized();
            using (Py.GIL())
            {
                dynamic pickle = Py.Import("pickle");
                // dynamic modelQ7 = pickle.load( open("modeloQ7.sav", "rb") );
                // dynamic question7 = modelQ7.predict
                // ( 
                //     data.PreTestAnswers[6], data.PostTestAnswers[6], data.HasDied, 
                //     data.HasFinished, data.TotalVisits, data.TotalRooms, data.NumberOfVisitedRooms, 
                //     data.CollectedKeys, data.TotalKeys, data.OpenedLocks, data.TotalLocks, 
                //     data.CollectedTreasures, data.TotalTreasures, data.EnemiesDefeated,
                //     data.TotalEnemies 
                // );
            }
        }
    }
}
