using System.Collections.Generic;
using UnityEngine;

namespace Game.DataCollection
{
    [CreateAssetMenu]
    public class FormQuestionsData : ScriptableObject
    {
        public List<FormQuestionData> questions;
    }
}
