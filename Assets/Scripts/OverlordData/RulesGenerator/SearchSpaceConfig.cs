using MyBox;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Overlord.UI;

[CreateAssetMenu(fileName = "SearchSpaceSO", menuName = "Overlord-Project/Rules-Generator/SearchSpaceConfigSO")]
public class SearchSpaceConfig : ScriptableObject
{
    public MinMaxSliderRange Status1 = new MinMaxSliderRange(.1f, 10f, 2, 7, "Status1");
    public MinMaxSliderRange Status2 = new MinMaxSliderRange(.1f, 10f, 2f, 5f, "Status2");
    public MinMaxSliderRange Status3 = new MinMaxSliderRange(.1f, 10f, 0.75f, 4f, "Status3");
    public MinMaxSliderRange Status4 = new MinMaxSliderRange(.1f, 10f, 0.8f, 3.2f, "Status4");
    public MinMaxSliderRange Status5 = new MinMaxSliderRange(.1f, 10f, 1.5f, 10f, "Status5");
    public MinMaxSliderRange Status6 = new MinMaxSliderRange(.1f, 10f, 0.3f, 1.5f, "Status6");
    public MinMaxSliderRange WeaponStatus1 = new MinMaxSliderRange(.1f, 10f, 1f, 4f, "WeaponStatus1");

    [DisplayInspector]
    public EnemyMovementsSOInterface MovementSet;
    [DisplayInspector]
    public EnemyWeaponsSOInterface WeaponSet;
}
#if UNITY_EDITOR
[CustomEditor(typeof(SearchSpaceConfig))]
public class SearchSpaceConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Cabe�alho
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Status", GUILayout.Width(75));
        GUILayout.Label("Min", GUILayout.Width(40));
        GUILayout.Label("", GUILayout.Width(40));
        GUILayout.Label("RangeBar", GUILayout.ExpandWidth(true));
        GUILayout.Label("", GUILayout.Width(40));
        GUILayout.Label("Max", GUILayout.Width(40));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(4);

        // Itera as propriedades
        SerializedProperty prop = serializedObject.GetIterator();
        bool enterChildren = true;
        while (prop.NextVisible(enterChildren))
        {
            enterChildren = false;

            if (prop.name == "m_Script")
                continue;

            EditorGUILayout.PropertyField(prop, true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif