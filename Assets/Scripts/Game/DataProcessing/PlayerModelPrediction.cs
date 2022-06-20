using System;
using Microsoft.ML.Data;
using UnityEngine;

namespace Game.DataProcessing
{
    [Serializable]
    public class PlayerModelPrediction
    {
        [ColumnName("Score")] public float score;
    }
}