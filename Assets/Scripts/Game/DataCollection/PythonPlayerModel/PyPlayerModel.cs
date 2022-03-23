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
using System.Collections.Generic;
using Util;

namespace Game.DataCollection
{
    public class PyPlayerModel : MonoBehaviour
    {
        const string kStateName = "com.unity.scripting.python.samples.pyside";
        // Used for testing
        private void Start() {
            List<int> preFormAnswers =  new List<int>();
            int[] array1 = { 0, 1, 2, 3, 4, 5, 6, 7, 8 ,9, 10, 11 } ;
            preFormAnswers.AddRange( array1 );
            List<int> PostFormAnswers = new List<int>();
            PostFormAnswers.AddRange( array1 );
            var playerAndGameplayData = new PlayerAndGameplayData( 
                preFormAnswers, PostFormAnswers, true, false, 20, 30, 3, 
                0, 2, 0, 2, 3, 10, 3, 10 
                );
            PredictPlayerModel( this, new SendGameAndPlayerDataArgs( playerAndGameplayData) );
        }

        protected void OnEnable()
        {
            GameplayData.SendGameAndPlayerDataEventHandler += PredictPlayerModel;
        }

        protected void OnDisable()
        {
            GameplayData.SendGameAndPlayerDataEventHandler -= PredictPlayerModel;
        }

        /// <summary>
        /// Hack to get the current file's directory
        /// </summary>
        /// <param name="fileName">Leave it blank to the current file's directory</param>
        /// <returns></returns>
        private static string __DIR__([System.Runtime.CompilerServices.CallerFilePath] string fileName = "")
        {
            return Path.GetDirectoryName(fileName);
        }

       static void PredictPlayerModel ( object sender, SendGameAndPlayerDataArgs eventArgs )
       {
            PlayerAndGameplayData data = eventArgs.PlayerGameplayData;
            PythonRunner.EnsureInitialized();
            using (Py.GIL())
            {
                using (PyScope scope = Py.CreateScope())
                {
                    PythonEngine.ImportModule("pickle");

                    PyObject pyData = data.ToPython();
                    scope.Set( "pyData", pyData );
                    string path = Application.dataPath+"/Resources"+"/PlayerQuestionModels"+"/modeloQ7.sav";
                    path.Replace('/',Constants.SEPARATOR_CHARACTER);
                    Debug.Log(path);
                    scope.Exec( 
                        "import pandas as pd\n"
                        +"import pickle\n"
                        +"import numpy as np\n"
                        +$"fileStream = open(r'{path}', 'rb')\n"    
                        +"modelQ7 = pickle.load(fileStream)\n"
                        +"data={'has_finished':[pyData.HasFinished],'has_died':[pyData.HasDied], 'total_keys':[pyData.TotalKeys], 'keys_found':[pyData.CollectedKeys], 'total_locks':[pyData.TotalLocks], 'doors_unlocked':[pyData.OpenedLocks], 'total_rooms':[pyData.TotalRooms], 'unique_rooms_conclusion':[pyData.NumberOfVisitedRooms], 'rooms_visited':[pyData.TotalVisits], 'player_lost_health':[0],'number_of_enemies':[pyData.TotalEnemies], 'enemies_fought':[pyData.EnemiesDefeated], 'number_of_npcs':[0], 'number_of_interacted_npcs':[0], 'total_treasures':[pyData.TotalTreasures], 'treasures_foud':[pyData.CollectedTreasures], 'max_combo':[0], 'ID':[0], 'PostQuestion 0':[pyData.PostTestAnswers[0]], 'PostQuestion 1':[pyData.PostTestAnswers[1]], 'PostQuestion 2':[pyData.PostTestAnswers[2]], 'PostQuestion 3':[pyData.PostTestAnswers[3]], 'PostQuestion 4':[pyData.PostTestAnswers[4]], 'PostQuestion 5':[pyData.PostTestAnswers[5]], 'PostQuestion 6':[pyData.PostTestAnswers[6]], 'PostQuestion 7':[pyData.PostTestAnswers[7]], 'PostQuestion 8':[pyData.PostTestAnswers[8]], 'PostQuestion 9':[pyData.PostTestAnswers[9]], 'PostQuestion 10':[pyData.PostTestAnswers[10]], 'PostQuestion 11':[pyData.PostTestAnswers[11]], 'PreQuestion 0':[pyData.PreTestAnswers[0]], 'PreQuestion 1':[pyData.PreTestAnswers[1]], 'PreQuestion 2':[pyData.PreTestAnswers[2]], 'PreQuestion 3':[pyData.PreTestAnswers[3]], 'PreQuestion 4':[pyData.PreTestAnswers[4]], 'PreQuestion 5':[pyData.PreTestAnswers[5]], 'PreQuestion 6':[pyData.PreTestAnswers[6]], 'PreQuestion 7':[pyData.PreTestAnswers[7]], 'PreQuestion 8':[pyData.PreTestAnswers[8]], 'PreQuestion 9':[pyData.PreTestAnswers[9]], 'PreQuestion 10':[pyData.PreTestAnswers[10]], 'PreQuestion 11':[pyData.PreTestAnswers[11]]}\n"
                        +"df = pd.DataFrame(data)\n"
                        +"question7 = modelQ7.predict( df )\n"
                        +"fileStream.close()\n"
                     );

                    PyObject result = scope.Get("question7");
                    float csResult = result.As<float>();
                    Debug.Log(csResult);
                }
            }
        }
    }
}
