using UnityEditor;
using UnityEngine;

namespace Overlord.UI
{
    [System.Serializable]
    public struct MinMaxSliderRange
    {
        public string statusName;
        public float minLimit;
        public float maxLimit;
        public float Min;
        public float Max;

        public MinMaxSliderRange(float minLimit, float maxLimit, float minValue, float maxValue, string statusName = "")
        {
            this.statusName = statusName;
            this.minLimit = minLimit;
            this.maxLimit = maxLimit;
            this.Min = minValue;
            this.Max = maxValue;
        }
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(MinMaxSliderRange))]
    public class RangeSliderUI : PropertyDrawer
    {
        const float NumberWidth = 40f; // menor para dar mais espa�o ao slider
        const float NameWidth = 75f;
        const float InnerSpacing = 4f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var statusName = property.FindPropertyRelative("statusName");
            var pMinLimit = property.FindPropertyRelative("minLimit");
            var pMaxLimit = property.FindPropertyRelative("maxLimit");
            var pMinValue = property.FindPropertyRelative("Min");
            var pMaxValue = property.FindPropertyRelative("Max");

            float lineH = EditorGUIUtility.singleLineHeight;
            float x = position.x;
            float y = position.y;

            // Colunas: Filename | MinLimit | MinValue | Slider | MaxValue | MaxLimit
            Rect rName = new Rect(x, y, NameWidth, lineH);
            Rect rMinLimit = new Rect(rName.xMax + InnerSpacing, y, NumberWidth, lineH);
            Rect rMinVal = new Rect(rMinLimit.xMax + InnerSpacing, y, NumberWidth, lineH);
            Rect rSlider = new Rect(rMinVal.xMax + InnerSpacing, y, position.width - (NameWidth + NumberWidth * 4 + InnerSpacing * 5), lineH);
            Rect rMaxVal = new Rect(rSlider.xMax + InnerSpacing, y, NumberWidth, lineH);
            Rect rMaxLimit = new Rect(rMaxVal.xMax + InnerSpacing, y, NumberWidth, lineH);

            // Campo de texto (com placeholder "Status")
            string newName = EditorGUI.TextField(rName, GUIContent.none, statusName.stringValue);
            if (newName != statusName.stringValue) statusName.stringValue = newName;
            if (string.IsNullOrEmpty(statusName.stringValue))
            {
                var phStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel) { alignment = TextAnchor.MiddleLeft };
                EditorGUI.LabelField(rName, "Status", phStyle);
            }

            // Limites
            pMinLimit.floatValue = EditorGUI.FloatField(rMinLimit, GUIContent.none, pMinLimit.floatValue);
            pMaxLimit.floatValue = EditorGUI.FloatField(rMaxLimit, GUIContent.none, pMaxLimit.floatValue);

            // Valores
            float minVal = pMinValue.floatValue;
            float maxVal = pMaxValue.floatValue;
            minVal = EditorGUI.FloatField(rMinVal, GUIContent.none, minVal);
            maxVal = EditorGUI.FloatField(rMaxVal, GUIContent.none, maxVal);

            // Slider largo
            EditorGUI.MinMaxSlider(rSlider, ref minVal, ref maxVal, pMinLimit.floatValue, pMaxLimit.floatValue);

            // Clamps
            minVal = Mathf.Clamp(minVal, pMinLimit.floatValue, pMaxLimit.floatValue);
            maxVal = Mathf.Clamp(maxVal, pMinLimit.floatValue, pMaxLimit.floatValue);
            if (minVal > maxVal) minVal = maxVal;

            pMinValue.floatValue = minVal;
            pMaxValue.floatValue = maxVal;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight + 2f;
        }
    }
#endif
}
