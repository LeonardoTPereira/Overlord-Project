using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Mission_Log")]
public class Mission_Log : ScriptableObject
{
    public int index1 = 0, index2 = -1;
    public List<int> type = null;
    public bool isFinished = true;
}
