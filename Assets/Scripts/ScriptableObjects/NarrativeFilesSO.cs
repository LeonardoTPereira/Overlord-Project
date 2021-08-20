using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NarrativeFilesSO : ScriptableObject
{
    [SerializeField]
    private List<string> narrativeFolders;

    public List<string> NarrativeFolders { get => narrativeFolders; set => narrativeFolders = value; }
}

