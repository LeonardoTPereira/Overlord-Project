using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Firebase.Extensions;
using Firebase.Storage;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using UnityEditor;
using UnityEngine;
using Util;
#if !UNITY_WEBGL || UNITY_EDITOR
using Firebase.Extensions;
using Firebase.Storage;
#else
using System;
using System.Text;
using FirebaseWebGLBridge = FirebaseWebGL.Scripts.FirebaseBridge;
#endif

namespace Game.DataProcessing
{
    public class TestPlayerRegression : MonoBehaviour
    {
        private const int TrainingSize = 1000;
        private const int TestSize = 100;
        private const string LabelName = "Label";
        private const string FeatureName = "Features";
        private const string CategoricalFeatureName = "CategoricalFeatures";
        private const string ScoreName = "Score";
        private static string _modelPath;
        private static string _dataPrepPath;
        
        private MLContext _mlContext;
        private List<PlayerModel> _playerTrainData;
        private List<PlayerModel> _playerTestData;

        private void Awake()
        {
            _playerTrainData = new List<PlayerModel>();
            for (var i = 0; i < TrainingSize; i++)
            {
                _playerTrainData.Add(PlayerModel.CreateRandomData());
            }
            
            _playerTestData = new List<PlayerModel>();
            for (var i = 0; i < TestSize; i++)
            {
                _playerTestData.Add(PlayerModel.CreateRandomData());
            }
            _modelPath = "Assets" + Constants.SEPARATOR_CHARACTER + "Resources" 
                         + Constants.SEPARATOR_CHARACTER + "Models" + Constants.SEPARATOR_CHARACTER + "Player.zip";
            _dataPrepPath = "Assets" + Constants.SEPARATOR_CHARACTER + "Resources" 
                            + Constants.SEPARATOR_CHARACTER + "Models" + Constants.SEPARATOR_CHARACTER + "DataPrep.zip";
        }

        private void Start()
        {
            _mlContext = new MLContext(seed: 0);
            
            GetModelFromServer();

            ITransformer predictionModel;
            ITransformer dataPrepPipeline;
            
            if (!File.Exists(_modelPath))
            {
                Debug.Log("Create Model");

                var dataView = _mlContext.Data.LoadFromEnumerable(_playerTrainData);
                var dataProcessPipeline = GetPlayerDataPipeline();
                dataPrepPipeline = dataProcessPipeline.Fit(dataView);
                var transformedData = dataPrepPipeline.Transform(dataView);
                var estimator = _mlContext.Regression.Trainers.OnlineGradientDescent();
                predictionModel = estimator.Fit(transformedData);
                SaveModel(predictionModel, transformedData, _modelPath);
                SaveModel(dataPrepPipeline, dataView, _dataPrepPath);
                AssetDatabase.Refresh();
                SendModelToServer();
                var testDataView = _mlContext.Data.LoadFromEnumerable(_playerTestData);
                var dataTestPreparatorTransformer = dataProcessPipeline.Fit(testDataView);
                var transformedTestData = dataTestPreparatorTransformer.Transform(testDataView);
                var predictedTestData = predictionModel.Transform(transformedTestData);
                var metrics = _mlContext.Regression.Evaluate(predictedTestData, LabelName, ScoreName);
                PrintRegressionMetrics(dataProcessPipeline.ToString(), metrics);
            }
            else
            {
                Debug.Log("Retrain Model");
                DataViewSchema modelSchema, dataPreparationSchema;
                predictionModel = _mlContext.Model.Load(_modelPath, out modelSchema);
                dataPrepPipeline = _mlContext.Model.Load(_dataPrepPath, out dataPreparationSchema);
                LinearRegressionModelParameters originalModelParameters =
                    ((ISingleFeaturePredictionTransformer<object>)predictionModel).Model as LinearRegressionModelParameters;
                var dataView = _mlContext.Data.LoadFromEnumerable(_playerTestData);
                var transformedNewData = dataPrepPipeline.Transform(dataView);
                predictionModel = _mlContext.Regression.Trainers.OnlineGradientDescent()
                        .Fit(transformedNewData, originalModelParameters);
                var testDataView = _mlContext.Data.LoadFromEnumerable(_playerTestData);
                var transformedTestData = dataPrepPipeline.Transform(testDataView);
                var predictedTestData = predictionModel.Transform(transformedTestData);
                var metrics = _mlContext.Regression.Evaluate(predictedTestData, LabelName, ScoreName);
                PrintRegressionMetrics(dataPrepPipeline.ToString(), metrics);
            }
            LoadModelAndPredict(predictionModel, dataPrepPipeline);
        }

        private void SaveModel(ITransformer transformer, IDataView dataView, string path)
        {
            using (var file = File.OpenWrite(path))
            {
                _mlContext.Model.Save(transformer, dataView.Schema, file);
                file.Close();
            }
        }

        private EstimatorChain<NormalizingTransformer> GetPlayerDataPipeline()
        {
            return _mlContext.Transforms.CopyColumns(
                    outputColumnName: LabelName, inputColumnName: nameof(PlayerModel.resultingPreference))
                .Append(_mlContext.Transforms.Categorical.OneHotEncoding(
                    outputColumnName: CategoricalFeatureName, inputColumnName: nameof(PlayerModel.hasWonLastLevel)))
                .Append(_mlContext.Transforms.Conversion.ConvertType(nameof(PlayerModel.totalAttempts),
                    outputKind: DataKind.Single))
                .Append(_mlContext.Transforms.Conversion.ConvertType(nameof(PlayerModel.maxCombo),
                    outputKind: DataKind.Single))
                .Append(_mlContext.Transforms.Conversion.ConvertType(nameof(PlayerModel.lostHealthLastLevel),
                    outputKind: DataKind.Single))
                .Append(_mlContext.Transforms.Conversion.ConvertType(nameof(PlayerModel.maxComboLastLevel),
                    outputKind: DataKind.Single))
                .Append(_mlContext.Transforms.Concatenate(FeatureName,
                    nameof(PlayerModel.totalAttempts),
                    nameof(PlayerModel.successRate),
                    nameof(PlayerModel.npcInteractionRate),
                    nameof(PlayerModel.enemyKillRate),
                    nameof(PlayerModel.treasureCollectionRate),
                    nameof(PlayerModel.averageLostHealthPerAttempt),
                    nameof(PlayerModel.maxCombo),
                    nameof(PlayerModel.keyCollectionRate),
                    nameof(PlayerModel.lockOpeningRate),
                    nameof(PlayerModel.averageRoomRevisitingRate),
                    nameof(PlayerModel.uniqueRoomVisitingRate),
                    nameof(PlayerModel.masteryPreference),
                    nameof(PlayerModel.immersionPreference),
                    nameof(PlayerModel.creativityPreference),
                    nameof(PlayerModel.achievementPreference),
                    nameof(PlayerModel.averageTimeToFinish),
                    nameof(PlayerModel.keyCollectionRateLastLevel),
                    nameof(PlayerModel.lockOpeningRateLastLevel),
                    nameof(PlayerModel.npcInteractionRateLastLevel),
                    nameof(PlayerModel.enemyKillRateLastLevel),
                    nameof(PlayerModel.treasureCollectionRateLastLevel),
                    nameof(PlayerModel.lostHealthLastLevel),
                    nameof(PlayerModel.maxComboLastLevel),
                    nameof(PlayerModel.timeToFinishLastLevel), CategoricalFeatureName))
                .Append(_mlContext.Transforms.NormalizeMinMax(FeatureName));
        }

        private void LoadModelAndPredict(ITransformer trainedModel, ITransformer dataPrep)
        {
            var data = new List<PlayerModel> {PlayerModel.CreateRandomData()};
            var dataView = _mlContext.Data.LoadFromEnumerable(data);
            var transformedData = dataPrep.Transform(dataView);
            var predictions = trainedModel.Transform(transformedData);

            Debug.Log($"**********************************************************************");
            Debug.Log($"Predicted Preference: {predictions.GetColumn<float>("Score").ToArray()[0] :0.####}");
            Debug.Log($"**********************************************************************");
        }

        private static void PrintRegressionMetrics(string name, RegressionMetrics metrics)
        {
            Debug.Log($"*************************************************");
            Debug.Log($"*       Metrics for {name} regression model      ");
            Debug.Log($"*------------------------------------------------");
            Debug.Log($"*       LossFn:        {metrics.LossFunction:0.##}");
            Debug.Log($"*       R2 Score:      {metrics.RSquared:0.##}");
            Debug.Log($"*       Absolute loss: {metrics.MeanAbsoluteError:#.##}");
            Debug.Log($"*       Squared loss:  {metrics.MeanSquaredError:#.##}");
            Debug.Log($"*       RMS loss:      {metrics.RootMeanSquaredError:#.##}");
            Debug.Log($"*************************************************");
        }
        
        private static void SendModelToServer()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            var storage = FirebaseStorage.DefaultInstance;
            var storageRef = storage.GetReferenceFromUrl("gs://overlord-1b095.appspot.com").Child("Models");
            var modelReference = storageRef.Child("model.zip");
            modelReference.PutFileAsync(_modelPath)
                .ContinueWith((task) => {
                    if (task.IsFaulted || task.IsCanceled) {
                        Debug.Log(task.Exception?.ToString());
                        // Uh-oh, an error occurred!
                    }
                    else {
                        // Metadata contains file metadata such as size, content-type, and download URL.
                        var metadata = task.Result;
                        var md5Hash = metadata.Md5Hash;
                        Debug.Log("Finished uploading...");
                        Debug.Log("md5 hash = " + md5Hash);
                    }
                });
            modelReference = storageRef.Child("dataPrep.zip");
            modelReference.PutFileAsync(_modelPath)
                .ContinueWith((task) => {
                    if (task.IsFaulted || task.IsCanceled) {
                        Debug.Log(task.Exception?.ToString());
                        // Uh-oh, an error occurred!
                    }
                    else {
                        // Metadata contains file metadata such as size, content-type, and download URL.
                        var metadata = task.Result;
                        var md5Hash = metadata.Md5Hash;
                        Debug.Log("Finished uploading...");
                        Debug.Log("md5 hash = " + md5Hash);
                    }
                });
#else
            var modelData = File.ReadAllBytes(_modelPath);
            FirebaseWebGL.Scripts.FirebaseBridge.FirebaseStorage.UploadFile(_modelPath, Convert.ToBase64String(modelData), "model.zip", "DisplayInfo", "DisplayErrorObject");
            Debug.Log("Added document");
#endif
        }

        private static void GetModelFromServer()
        {
            var storage = FirebaseStorage.DefaultInstance;
            var storageRef = storage.GetReferenceFromUrl("gs://overlord-1b095.appspot.com").Child("Models");
            var modelReference = storageRef.Child("model.zip");
            // Download in memory with a maximum allowed size of 1MB (1 * 1024 * 1024 bytes)
            byte[] fileContents;
            const long maxAllowedSize = 10 * 1024 * 1024;
            modelReference.GetBytesAsync(maxAllowedSize).ContinueWithOnMainThread(task => {
                if (task.IsFaulted || task.IsCanceled) {
                    Debug.LogException(task.Exception);
                    // Uh-oh, an error occurred!
                }
                else {
                    fileContents = task.Result;
                    Debug.Log("Finished downloading!");
                    File.WriteAllBytes(_modelPath, fileContents);
                }
            });
            modelReference = storageRef.Child("model.zip");
            // Download in memory with a maximum allowed size of 1MB (1 * 1024 * 1024 bytes)
            modelReference.GetBytesAsync(maxAllowedSize).ContinueWithOnMainThread(task => {
                if (task.IsFaulted || task.IsCanceled) {
                    Debug.LogException(task.Exception);
                    // Uh-oh, an error occurred!
                }
                else {
                    fileContents = task.Result;
                    Debug.Log("Finished downloading!");
                    File.WriteAllBytes(_modelPath, fileContents);
                }
            });
            AssetDatabase.Refresh();
        }
    }
}